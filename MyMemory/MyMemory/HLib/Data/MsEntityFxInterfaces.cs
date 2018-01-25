

namespace HLib.Data
{

    using System.Collections.Generic;


    public class ListItemAboutToRemoveEventArgs<T>
    {
        public IList<T> List { get; set; }
        public T Item { get; set; }
        public bool ClearAll { get; set; }
        public bool Handled { get; set; }
    }


    public delegate void ListItemAboutToRemoveHandler<T>(ListItemAboutToRemoveEventArgs<T> eventArgs);
}
