// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPool.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the CompilerPool type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Threading
{

    using System;
    using System.Collections.Generic;

    using HLib.Logging;


    public class ObjectPool<T> : IDisposable where T : class
    {

        public delegate T ObjectCreatorDelegate();


        private readonly int _capacity;
        private Stack<T> _pool;


        public ObjectPool(int capacity)
        {
            _capacity = capacity;

            Logger = Loggers.Null;
            _pool = new Stack<T>(capacity);
        }


        public ILoggable Logger { get; set; }


        public void CreatePool(ObjectCreatorDelegate objectCreator)
        {
            for (var i = 0; i < _capacity; i++)
            {
                _pool.Push(objectCreator.Invoke());
            }
        }


        public void Push(T t)
        {
            lock (_pool)
            {
                _pool.Push(t);
            }
        }


        public T Pop()
        {
            lock (_pool)
            {
                return _pool.Pop();
            }
        }


        public int Count
        {
            get
            {
                int count;
                lock (_pool)
                {
                    count = _pool.Count;
                }

                return count;
            }
        }


        public void Dispose()
        {
            try
            {
                if (_pool != null)
                {
                    int count = _pool.Count;
                    for (int i = 0; i < count; i++)
                    {
                        T t = _pool.Pop();
                        if (t != null)
                        {
                            if (t is IDisposable)
                            {
                                ((IDisposable)t).Dispose();
                            }

                            Logger.LogEvent(string.Format("--> Disposed:{0}", i));
                        }
                    }

                    _pool.Clear();
                    _pool = null;
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}