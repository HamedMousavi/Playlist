// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskExecutionComplete.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TaskExecutionComplete type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task.Conditions
{

    public class TaskExecutionComplete : ICondition
    {

        private readonly ITask _task;


        public TaskExecutionComplete(ITask task)
        {
            ConditionStateChanged = new Observable();

            _task = task;
            _task.TaskExecutionStateChanged += (sender, args) => CheckTaskState();
            CheckTaskState();
        }


        public IObservable ConditionStateChanged { get; private set; }


        public bool IsMet()
        {
            var res = _task.TaskState == TaskExecutionState.CompletedSuccessfully
                || _task.TaskState == TaskExecutionState.Failed;

            if (!res)
            {
                // In case a task is skipped, it is complete either
                res = _task.TaskState == TaskExecutionState.Skipped;
            }

            return res;
        }


        private void CheckTaskState()
        {
            if (IsMet())
            {
                ConditionStateChanged.Notify(_task);
            }
        }
    }
}
