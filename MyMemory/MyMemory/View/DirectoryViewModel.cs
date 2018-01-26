using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyMemory.Annotations;
using MyMemory.Domain;

namespace MyMemory
{

    public class DirectoryViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
        public string Path
        {
            get => _path;
            set { _path = value; OnPropertyChanged(); }
        }
        public PlayableList FileListViewModel => LoadPlaylist();

        private PlayableList LoadPlaylist()
        {
            if (_playlist == null)
            {
                var cache = new FileCache(
                    $"{Path}{System.IO.Path.DirectorySeparatorChar}playlist.json",
                    new JsonStringSerializer());

                var loader = new DirectoryLoader(cache, Path);

                _playlist = new PlayList(loader, cache, new FilePlayer(Path));
                _playlist.WhenLoaded += (sender, args) => WhenDirectoryLoaded();
                _playlist.WhenPlayed += (sender, args) => WhenFilePlayed(((PlayList.ItemEventArgs)args).Item);
                _playlist.Load();
            }

            return _filesViewModel;
        }


        public DirectoryViewModel()
        {
            _filesViewModel = new PlayableList();
        }


        private PlayList _playlist;
        private readonly PlayableList _filesViewModel;
        private string _name;
        private string _path;


        private void WhenFilePlayed(IPlayListItem item)
        {
            // Update playlist
            _filesViewModel.Select(_playlist.IndexOf(item));
            AppStatus.Instance.Info($"Playing {item}", $"{_playlist.SelectedIndex + 1}/{_playlist.Count})");
            OnPropertyChanged(nameof(FileListViewModel));
        }


        private void WhenDirectoryLoaded()
        {
            // Update playlist
            _playlist.Save(_filesViewModel);
            AppStatus.Instance.Info("", $"{_playlist.SelectedIndex + 1}/{_playlist.Count})");
            OnPropertyChanged(nameof(FileListViewModel));
        }

        public void PlayPreviousFile()
        {
            _playlist.Prev()?.Play();
        }

        public void PlayCurrentFile()
        {
            _playlist.Current()?.Play();
        }

        public void PlayNextFile()
        {
            _playlist.Next()?.Play();
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged
    }
}
