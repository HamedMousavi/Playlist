using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MyMemory.Annotations;
using MyMemory.Domain.Abstract;


namespace MyMemory
{

    public class PlayableList : ObservableCollection<Playable>, IPlaylistSaver
    {
        private Playable _playing;
        public Playable Selected { get; set; }


        public Playable SetActive(IPlaylistItem item)
        {
            if (_playing != null)
                _playing.IsPlaying = false;

            _playing = this.SingleOrDefault(p => IsEqual(p.Id, item.Id));
            if (_playing != null) _playing.IsPlaying = true;

            return _playing;
        }


        public Playable GetActive()
        {
            return _playing;
        }


        public void Save(IPlaylistState playlist)
        {
            Clear();
            var index = 0;
            var indexFormat = new string('0', playlist.Resources.Count().ToString().Length);

            foreach (var resource in playlist.Resources)
            {
                var playable = new Playable(
                    resource,
                    (++index).ToString(indexFormat),
                    IsEqual(resource.Id, playlist.SelectedItemId));

                Add(playable);

                if (playable.IsPlaying) _playing = playable;
            }
        }


        private static bool IsEqual(string id, string itemId)
        {
            //bool IsEqual(string id); // Check if resource id matches passed id
            return string.Equals(id, itemId, StringComparison.InvariantCultureIgnoreCase);
        }
    }


    public class Playable : INotifyPropertyChanged
    {

        private bool _isPlaying;
        private readonly IResource _resource;


        public Playable()
        { }


        public Playable(IResource resource, string rowIndex, bool isPlaying)
        {
            _resource = resource;
            IsPlaying = isPlaying;
            RowIndex = rowIndex;
        }


        public bool IsPlaying
        {
            get => _isPlaying;
            set { _isPlaying = value; OnPropertyChanged(); }
        }


        public string RowIndex { get; }
        public string Id => _resource?.Id;
        public string Label => _resource?.Name;
        public string FilePath => _resource.Path;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public bool IsEqual(IPlaylistItem item)
        //{
        //    if (item == null) return _resource == null;

        //    return _resource.IsEqual(item.Id);
        //}
    }
}
