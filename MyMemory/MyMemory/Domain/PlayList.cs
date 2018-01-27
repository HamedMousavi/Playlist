using System;
using System.Collections.Generic;
using System.Linq;


namespace MyMemory.Domain
{

    public class Playlist : IPlaylist
    {

        private readonly IPlaylistLoader _loader;
        private readonly IPlaylistSaver _saver;
        private readonly IPlaylistItemPlayer _itemPlayer;
        private List<IPlaylistItem> _list;

        public bool IsEmpty => _list == null || !_list.Any();
        public int Count => IsEmpty ? 0 : _list.Count;
        public int SelectedIndex { get; private set; }


        public Playlist(IPlaylistLoader loader, IPlaylistSaver saver, IPlaylistItemPlayer itemPlayer)
        {
            _loader = loader;
            _saver = saver;
            _itemPlayer = itemPlayer;
            SelectedIndex = 0;

            _itemPlayer.WhenPlayed += (sender, args) => OnWhenPlayed();
        }


        private IPlaylistItem Find(int index)
        {
            if (IsEmpty)
            {
                Load();
                if (IsEmpty) return null;
                index = SelectedIndex;
            }

            if (index >= _list.Count || index < 0)
            {
                Load();
                index = 0;
            }

            if (SelectedIndex != index)
            {
                SelectedIndex = index;
                Save();
            }

            return _list[index];
        }


        protected virtual IPlaylistState GetState()
        {
            return new PlaylistState(_list, SelectedIndex);
        }


        protected virtual void SetState(IPlaylistState state)
        {
            if (state == null) return;

            _list = state.Resources.Select(CreateItem).ToList();
            SelectedIndex = state.Index;

            OnWhenLoaded();
        }


        protected virtual IPlaylistItem CreateItem(INameableResource resource)
        {
            return new PlaylistItem(resource.Name, _itemPlayer, resource.Path);
        }


        public IPlaylistItem Current()
        {
            return Find(SelectedIndex);
        }


        public IPlaylistItem Next()
        {
            return Find(SelectedIndex + 1);
        }


        public IPlaylistItem Prev()
        {
            return Find(SelectedIndex - 1);
        }


        public int IndexOf(IPlaylistItem item)
        {
            return IsEmpty ? -1 : _list.IndexOf(item);
        }


        public void Save()
        {
            _saver.Save(GetState());
        }


        public void Save(IPlaylistSaver saver)
        {
            saver.Save(GetState());
        }


        public void Load()
        {
            SetState(_loader.Load());
        }


        public void Load(IPlaylistLoader loader)
        {
            SetState(loader.Load());
        }


        public event EventHandler WhenLoaded;
        public event EventHandler WhenPlayed;

        protected virtual void OnWhenLoaded()
        {
            WhenLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnWhenPlayed()
        {
            WhenPlayed?.Invoke(this, !IsEmpty ? new ItemEventArgs(_list[SelectedIndex]) : EventArgs.Empty);
        }

        public class ItemEventArgs : EventArgs
        {
            public ItemEventArgs(IPlaylistItem item)
            {
                Item = item;
            }

            public IPlaylistItem Item { get; }
        }
    }
}