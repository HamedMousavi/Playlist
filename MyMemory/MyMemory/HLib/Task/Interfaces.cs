// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interfaces.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   //   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the IObserver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    using System;


    public delegate void TaskExecutionStateChangedHandler(ITask task, TaskExecutionStateEventArgs eventArgs);

    public delegate bool TaskDelegate();

    public delegate void CompletedHandler(IDelegateTask task, bool results, object args);


    public enum TaskExecutionState
    {
        Init,
        Skipped,
        Pending,
        Running,
        CompletedSuccessfully,
        Failed
    }

    
    public interface IObserver
    {
        void OnChanged(IObservable subject, object args);
    }


    public interface IObservable
    {
        void Register(IObserver observer);

        void UnRegister(IObserver observer);

        void Notify(object args);
    }


    public interface ICondition
    {
        IObservable ConditionStateChanged { get; }

        bool IsMet();
    }


    public interface ITask : IDisposable
    {
        event TaskExecutionStateChangedHandler TaskExecutionStateChanged;

        TaskExecutionState TaskState { get; }

        string Name { get; set; }

        bool AddStartCondition(ICondition condition);

        bool AddSkipCondition(ICondition condition);

        bool Run();

        bool Cancel();
    }


    public interface IDelegateTask
    {
        event CompletedHandler Completed;

        void Schedule(TaskDelegate taskFunction, object args);

        bool Run();
    }


    public class TaskExecutionStateEventArgs : EventArgs
    {
        public TaskExecutionState OldState { get; set; }
    }
}
