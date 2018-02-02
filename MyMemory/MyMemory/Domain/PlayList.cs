using System;
using System.Collections.Generic;
using System.Linq;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class Playlist : IPlaylist
    {

        private readonly IPlaylistLoader _loader;
        private readonly IPlaylistSaver _saver;
        private readonly IPlaylistItemPlayer _itemPlayer;

        private List<IPlaylistItem> _list;
        private IPlaylistItem _activeItem;

        public bool IsEmpty => _list == null || !_list.Any();
        public int Count => IsEmpty ? 0 : _list.Count;

        public IPlaylistItem ActiveItem
        {
            get
            {
                if (IsEmpty) Load();
                if (_activeItem == null && !IsEmpty) _activeItem = _list[0];

                return _activeItem;
            }
            set
            {
                if (_activeItem == value) return;

                _activeItem = value;
                Save();
                OnWhenActiveItemChanged();
            }
        }


        public IPlaylistItem Next
        {
            get
            {
                if (ActiveItem != null)
                    ActiveItem = _list[NormalizeIndex(_list.IndexOf(_activeItem) + 1)];

                return ActiveItem;
            }
        }


        public IPlaylistItem Prev
        {
            get
            {
                if (ActiveItem != null)
                    ActiveItem = _list[NormalizeIndex(_list.IndexOf(_activeItem) - 1)];

                return ActiveItem;
            }
        }


        public Playlist(IPlaylistLoader loader, IPlaylistSaver saver, IPlaylistItemPlayer itemPlayer)
        {
            _loader = loader;
            _saver = saver;
            _itemPlayer = itemPlayer;
            _activeItem = null;

            _itemPlayer.WhenPlayed += (sender, args) => OnWhenPlayed();
        }


        protected virtual IPlaylistState GetState()
        {
            return new PlaylistState(_list, _activeItem?.Id);
        }


        protected virtual void SetState(IPlaylistState state)
        {
            if (state == null) return;

            _list = state.Resources.Select(CreateItem).ToList();
            _activeItem = FindById(state.SelectedItemId);

            OnWhenLoaded();
        }


        protected virtual IPlaylistItem CreateItem(IResource resource)
        {
            return new PlaylistItem(
                _itemPlayer,
                resource.Id,
                resource.Name,
                resource.Path);
        }


        public IPlaylistItem FindById(string itemId)
        {
            return _list?.SingleOrDefault(item =>
                item != null && string.Equals(item.Id,
                itemId, StringComparison.InvariantCultureIgnoreCase));
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


        private int NormalizeIndex(int index)
        {
            if (index < 0) index = 0;
            if (index >= _list.Count) index = _list.Count - 1;

            return index;
        }


        #region Events

        public event EventHandler WhenLoaded;
        public event EventHandler WhenPlayed;
        public event EventHandler WhenActiveItemChanged;

        protected virtual void OnWhenLoaded()
        {
            WhenLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnWhenPlayed()
        {
            WhenPlayed?.Invoke(this, !IsEmpty ? new ItemEventArgs(_activeItem) : EventArgs.Empty);
        }

        protected virtual void OnWhenActiveItemChanged()
        {
            WhenActiveItemChanged?.Invoke(this, !IsEmpty ? new ItemEventArgs(_activeItem) : EventArgs.Empty);
        }

        public class ItemEventArgs : EventArgs
        {
            public ItemEventArgs(IPlaylistItem item)
            {
                Item = item;
            }

            public IPlaylistItem Item { get; }
        }

        #endregion Events
    }
}