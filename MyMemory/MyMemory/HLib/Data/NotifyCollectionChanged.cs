

namespace HLib.Data
{
    
    using System.Collections.Specialized;
    using System.Threading;


    public class NotifyCollectionChanged : INotifyCollectionChanged
    {

        private readonly SynchronizationContext _synchronizationContext;


        public NotifyCollectionChanged(bool notifyOnUiThread = false)
        {
            if (notifyOnUiThread && Threading.Ui.SynchronizationContext != null)
            {
                _synchronizationContext = Threading.Ui.SynchronizationContext;
            }
            else
            {
                _synchronizationContext = SynchronizationContext.Current;
            }
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void NotifyAddEvent(object item)
        {
            FireCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }


        protected virtual void NotifyRemoveEvent(object item)
        {
            FireCollectionChanged(NotifyCollectionChangedAction.Remove, item);
        }


        protected virtual void NotifyClearEvent()
        {
            FireCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }


        private void FireCollectionChanged(NotifyCollectionChangedAction action, object item)
        {
            var args = item == null
            ? new NotifyCollectionChangedEventArgs(action)
            : new NotifyCollectionChangedEventArgs(action, item);

            if (SynchronizationContext.Current == _synchronizationContext)
            {
                FireCollectionChangedSync(args);
            }
            else if (_synchronizationContext != null)
            {
                _synchronizationContext.Send(FireCollectionChangedSync, args);
            }
            else
            {
                // Collection created in a thread with NULL SyncContext. Firing event will sure
                // cause an exception here. Please at least create this collection in the same
                // thread that is bound to it.
                FireCollectionChangedSync(args);
            }
        }


        public void FireCollectionChangedSync(object args)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, (NotifyCollectionChangedEventArgs)args);
            }
        }
    }
}
