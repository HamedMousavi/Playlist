// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskProgressCalculator.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TaskProgressCalculator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{
    using System;
    using System.ComponentModel;

    using HLib.Math;


    public class TaskProgressCalculator : ProgressCalculator
    {

        private readonly TaskList _taskList;


        public TaskProgressCalculator(TaskList taskList)
        {
            _taskList = taskList;
            _taskList.ListChanged += TaskListOnListChanged;

            PropertyChanged += (sender, args) =>
                {
                    if (string.Equals(args.PropertyName, "Percent", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FirePropertyChanged(this, "Progress");
                    }
                };
        }


        public short Progress
        {
            get
            {
                return Percent;
            }
        }


        private void TaskListOnListChanged(object sender, ListChangedEventArgs listChangedEventArgs)
        {
            switch (listChangedEventArgs.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    _taskList[listChangedEventArgs.NewIndex].TaskExecutionStateChanged += OnTaskExecutionStateChanged;
                    MaxValue = _taskList.Count;
                    break;

                    case ListChangedType.Reset:
                    Reset(_taskList.Count);
                    break;
            }
        }


        private void OnTaskExecutionStateChanged(ITask task, TaskExecutionStateEventArgs eventArgs)
        {
            if (task.TaskState == TaskExecutionState.CompletedSuccessfully
                || task.TaskState == TaskExecutionState.Failed
                || task.TaskState == TaskExecutionState.Skipped)
            {
                // Completion progress increases, 
                // even if task completed with failure
                Increment();
            }
        }
    }
}
