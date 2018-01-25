// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Task.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Task type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using HLib.Logging;


    public abstract class Task : ITask
    {

        private readonly List<ICondition> _skipConditions;
        private readonly List<ICondition> _startConditions;
        private readonly bool _canCancel;
        private volatile object _cancelLock;
        private volatile object _startConditionslLock;
        private AutoResetEvent _cancelEvent;
        private AutoResetEvent _executionComplete;

        private volatile TaskExecutionState _taskState;


        protected Task()
        {
            Logger = Loggers.Null;

            _skipConditions = new List<ICondition>();
            _startConditions = new List<ICondition>();

            _cancelLock = new object();
            _cancelEvent = new AutoResetEvent(false);
            _startConditionslLock = new object();
            
            _canCancel = true;

            _executionComplete = new AutoResetEvent(false);
            IsAsync = false;

            UpdateState(TaskExecutionState.Init);
        }



        public event TaskExecutionStateChangedHandler TaskExecutionStateChanged;

        public TaskExecutionState TaskState
        {
            get
            {
                return _taskState;
            }
        }

        public ILoggable Logger { get; set; }

        public string Name { get; set; }


        public bool AddStartCondition(ICondition condition)
        {
            lock (_startConditionslLock)
            {
                _startConditions.Add(condition);
            }

            return true;
        }


        public bool AddSkipCondition(ICondition condition)
        {
            _skipConditions.Add(condition);
            return true;
        }


        /// <summary>
        /// Runs a task asynchronously
        /// </summary>
        /// <returns>True if task scheduled for execution or skipped as a result of Skip conditions, false in case of error. </returns>
        public bool Run()
        {
            var shouldSkip = false;

            // Make sure we shouldn't skip this task
            Parallel.ForEach(
                _skipConditions,
                (condition, state) =>
                    {
                        if (condition.IsMet())
                        {
                            shouldSkip = true;
                            state.Stop();
                        }
                    });

            if (shouldSkip)
            {
                UpdateState(TaskExecutionState.Skipped);
                Logger.LogEvent(string.Format("Task '{0}' will skip because skip condition is met.", Name));
                return true;
            }

            UpdateState(TaskExecutionState.Pending);

            return ThreadPool.QueueUserWorkItem(
                TaskExecutionThread,
                new TaskExecutionThreadData
                    {
                        Logger = Logger,
                        StartConditions = _startConditions,
                        StartConditionLock = _startConditionslLock,
                        CancelEvent = _cancelEvent,
                        RunDelegate = ExecuteTask,
                        Task = this,
                        TaskCompleteEvent = IsAsync ? _executionComplete : null
                    });
        }


        public virtual void Dispose()
        {
            if (_cancelEvent != null)
            {
                Cancel();

                _executionComplete.Close();
                _executionComplete.Dispose();
                _executionComplete = null;

                _cancelEvent.Close();
                _cancelEvent.Dispose();
                _cancelEvent = null;
            }
        }


        /// <summary>
        /// Attempts to cancel task
        /// </summary>
        /// <returns>True if task cancel scheduled successfully (Task will not be cancelled by force and instantly). False if given task cannot be canceled. </returns>
        public bool Cancel()
        {
            lock (_cancelLock)
            {
                // This double check is required
                if (_canCancel)
                {
                    if (CanCancel())
                    {
                        _cancelEvent.Set();
                        return true;
                    }
                }
            }

            return false;
        }


        protected virtual bool CanCancel()
        {
            // Didn't put all logic of _canCancel in this function intentionally
            // to prevent overriden in subclasses mess with logic of this class
            return _canCancel;
        }


        protected abstract bool ExecuteTask();


        protected bool IsAsync { get; set; }


        protected void ExecutionComplete()
        {
            _executionComplete.Set();
        }

        /// <summary>
        /// Blocks until condition changes or cancel event is set
        /// </summary>
        /// <param name="condition">Condition which is epected to change</param>
        /// <param name="cancelEvent">Event that will raise when user Cancels task or when object is disposed</param>
        /// <returns> false if either Cancel event is set or error occurs, True if condition changes</returns>
        private static bool BlockUntilConditionChanges(ICondition condition, AutoResetEvent cancelEvent)
        {
            var res = false;
            using (var co = new ConditionObserver(condition.ConditionStateChanged))
            {
                // We have registered for condition change event here
                // Since the [IsMet] and [Registeration] aren't atomic
                // We must ensure nothing has changed in between
                // On solution is this:
                // Before waiting, check condition one more time
                if (condition.IsMet())
                {
                    co.Dispose();
                    res = true;
                }
                else
                {
                    var handles = new WaitHandle[2];
                    handles[0] = cancelEvent;
                    handles[1] = co.ConditionChangedEvent;

                    var waitRes = WaitHandle.WaitAny(handles, Timeout.Infinite, false);
                    switch (waitRes)
                    {
                        case 0: // Cancel event
                            break;

                        case 1: // Condition has changed
                            res = true;
                            break;
                    }
                }
            }

            return res;
        }


        private static ICondition GetNextUnsatisfiedCondition(IEnumerable<ICondition> conditions)
        {
            ICondition res = null;

            Parallel.ForEach(
                conditions,
                (condition, pState) =>
                {
                    if (!condition.IsMet())
                    {
                        res = condition;
                        pState.Break();
                    }
                });

            return res;
        }


        private void TaskExecutionThread(object state)
        {
            var data = state as TaskExecutionThreadData;
            if (data == null)
            {
                System.Diagnostics.Debug.WriteLine("Task execution Thread is leaving because given data is null or of wrong type.");

                // Don't throw exception here, log is enough
                return;
            }

            lock (data.StartConditionLock)
            {
                ICondition unsatisfiedCondition;
                do
                {
                    unsatisfiedCondition = GetNextUnsatisfiedCondition(data.StartConditions);

                    if (unsatisfiedCondition != null)
                    {
                        data.Logger.LogEvent(string.Format("Task {0} execution Waiting for condition to be met...", data.Task.Name));

                        if (!BlockUntilConditionChanges(unsatisfiedCondition, data.CancelEvent))
                        {
                            data.Logger.LogEvent(string.Format("Task {0} execution Cancelled or an error occurd. Exiting wait now", data.Task.Name));
                            return;
                        }
                    }
                }
                while (unsatisfiedCondition != null);
            }

            UpdateState(TaskExecutionState.Running);

            // All conditions are met, execute task here
            var executionResult = data.RunDelegate.Invoke();
            if (data.TaskCompleteEvent != null && executionResult)
            {
                var res = WaitHandle.WaitAny(new WaitHandle[] { data.TaskCompleteEvent, data.CancelEvent }, Timeout.Infinite);
                if (res != 0)
                {
                    Logger.LogEvent(string.Format("Task {0} execution cancelled or failed.", data.Task.Name));
                }
            }

            Logger.LogEvent(
                string.Format(
                    "Task {0} execution completed {1}.", data.Task.Name, executionResult ? "SUCCESSFULLY" : "With error(s)"));

            UpdateState(executionResult ? TaskExecutionState.CompletedSuccessfully : TaskExecutionState.Failed);
        }


        private void UpdateState(TaskExecutionState state)
        {
            var temp = TaskState;
            _taskState = state;

            if (TaskExecutionStateChanged != null)
            {
                TaskExecutionStateChanged(
                    this, new TaskExecutionStateEventArgs { OldState = temp });
            }        
        }
    }
}
