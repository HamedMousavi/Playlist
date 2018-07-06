using System;
using System.Collections.Generic;
using System.Linq;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class PlaylistContainer : IPlaylistContainer
    {

        private List<DirectoryItem> _directories;


        public void Save(IPlaylistContainerSaver saver)
        {
            saver.Save(new DirectoryListState(_directories));
        }


        public void Load(IPlaylistContainerLoader loader)
        {
            _directories = loader.Load()?.Items;
            _directories.Sort();
        }


        public void Add(string dirTitle, string dirPath)
        {
            if (_directories == null) _directories = new List<DirectoryItem>();
            _directories.Add(new DirectoryItem(dirTitle, dirPath));
        }


        internal void Remove(string path)
        {
            var found = _directories.SingleOrDefault(d => string.Equals(d.Path, path, StringComparison.InvariantCultureIgnoreCase));
            if (found != null) _directories.Remove(found);
        }
    }
}