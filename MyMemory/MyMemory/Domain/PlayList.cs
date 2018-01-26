using System;
using System.Collections.Generic;
using System.Linq;


namespace MyMemory.Domain
{

    public class PlayList : IPlayList
    {

        private readonly IPlayListLoader _loader;
        private readonly IPlayListSaver _saver;
        private readonly IPlayListItemPlayer _itemPlayer;
        private List<IPlayListItem> _list;

        public bool IsEmpty => _list == null || !_list.Any();
        public int Count => IsEmpty ? 0 : _list.Count;
        public int SelectedIndex { get; private set; }


        public PlayList(IPlayListLoader loader, IPlayListSaver saver, IPlayListItemPlayer itemPlayer)
        {
            _loader = loader;
            _saver = saver;
            _itemPlayer = itemPlayer;
            SelectedIndex = 0;

            _itemPlayer.WhenPlayed += (sender, args) => OnWhenPlayed();
        }


        private IPlayListItem Find(int index)
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


        protected virtual IPlayListState GetState()
        {
            return new FileListState(_list.Select(i => i.ToString()), SelectedIndex);
        }


        protected virtual void SetState(IPlayListState state)
        {
            if (state == null) return;

            _list = state.Names.Select(CreateItem).ToList();
            SelectedIndex = state.Index;

            OnWhenLoaded();
        }


        protected virtual IPlayListItem CreateItem(string name)
        {
            return new PlayListItem(name, _itemPlayer, name);
        }


        public IPlayListItem Current()
        {
            return Find(SelectedIndex);
        }


        public IPlayListItem Next()
        {
            return Find(SelectedIndex + 1);
        }


        public IPlayListItem Prev()
        {
            return Find(SelectedIndex - 1);
        }


        public int IndexOf(IPlayListItem item)
        {
            return IsEmpty ? -1 : _list.IndexOf(item);
        }


        public void Save()
        {
            _saver.Save(GetState());
        }


        public void Save(IPlayListSaver saver)
        {
            saver.Save(GetState());
        }


        public void Load()
        {
            SetState(_loader.Load());
        }


        public void Load(IPlayListLoader loader)
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
            public ItemEventArgs(IPlayListItem item)
            {
                Item = item;
            }

            public IPlayListItem Item { get; }
        }
    }
}