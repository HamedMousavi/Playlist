using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyMemory.Annotations;
using MyMemory.Domain;


namespace MyMemory
{

    public class PlayableList : ObservableCollection<Playable>, IPlaylistSaver
    {
        private Playable _playing;
        public Playable Selected { get; set; }


        public void Select(int index)
        {
            if (_playing != null)
            {
                _playing.IsPlaying = false;
            }

            _playing = this[index];
            _playing.IsPlaying = true;
        }


        public void Save(IPlaylistState playlist)
        {
            Clear();

            foreach (var resource in playlist.Resources)
            {
                var playable = new Playable(resource.Name, Count == playlist.Index);
                Add(playable);

                if (playable.IsPlaying) _playing = playable;
            }
        }
    }


    public class Playable : INotifyPropertyChanged
    {
        private bool _isPlaying;

        public Playable(string label, bool isPlaying)
        {
            Label = label;
            IsPlaying = isPlaying;
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set { _isPlaying = value; OnPropertyChanged();}
        }

        public string Label { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
