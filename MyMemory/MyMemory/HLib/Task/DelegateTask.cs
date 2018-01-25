// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateTask.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the DelegateTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    public class DelegateTask : Task, IDelegateTask
    {

        private TaskDelegate _taskFunction;

        public event CompletedHandler Completed;

        private object _args;


        public DelegateTask()
        {
            TaskExecutionStateChanged += delegate(ITask task, TaskExecutionStateEventArgs args)
                {
                    if (task.TaskState == TaskExecutionState.CompletedSuccessfully ||
                        task.TaskState == TaskExecutionState.Failed)
                    {
                        if (Completed != null)
                        {
                            Completed(this, task.TaskState == TaskExecutionState.CompletedSuccessfully, _args);
                        }
                    }
                };
        }



        public void Schedule(TaskDelegate taskFunction, object args)
        {
            _taskFunction = taskFunction;
            _args = args;
        }


        protected override bool ExecuteTask()
        {
            if (_taskFunction != null)
            {
                return _taskFunction.Invoke();
            }

            return false;
        }
    }
}
