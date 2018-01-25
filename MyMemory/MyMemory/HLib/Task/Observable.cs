// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Observable.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Observable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    // UNDONE:
    // REPLACE ALL USAGE OF LOCK IN THIS CLASS
    // TO Monitor with Timeout support
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class Observable : IObservable, IDisposable
    {

        private readonly List<IObserver> _observers;


        public Observable()
        {
            _observers = new List<IObserver>();
        }


        public void Register(IObserver observer)
        {
            lock (_observers)
            {
                _observers.Add(observer);
            }
        }


        public void UnRegister(IObserver observer)
        {
            lock (_observers)
            {
                _observers.Remove(observer);
            }
        }


        public void Notify(object args)
        {
            lock (_observers)
            {
                Parallel.ForEach(_observers, observer => observer.OnChanged(this, args));
            }
        }


        public void Dispose()
        {
            _observers.Clear();
        }
    }
}
