// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionObserver.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the ConditionObserver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    using System;
    using System.Threading;


    public class ConditionObserver : IObserver, IDisposable
    {
        private readonly IObservable _observable;


        public ConditionObserver(IObservable observable)
        {
            _observable = observable;
            ConditionChangedEvent = new AutoResetEvent(false);
            _observable.Register(this);
        }


        public AutoResetEvent ConditionChangedEvent { get; private set; }
        

        public void OnChanged(IObservable subject, object args)
        {
            ConditionChangedEvent.Set();
        }


        public void Dispose()
        {
            _observable.UnRegister(this);
            ConditionChangedEvent.Close();
            ConditionChangedEvent.Dispose();
        }
    }
}
