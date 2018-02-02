using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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


        public ICommand OpenDirectoryCommand => new RelayCommand(a => Process.Start(new ProcessStartInfo
        {
            FileName = Path,
            UseShellExecute = true,
            Verb = "open"
        }));


        private PlayableList LoadPlaylist()
        {
            if (_playlist == null)
            {
                var cache = new FileCache(
                    $"{Path}{System.IO.Path.DirectorySeparatorChar}playlist.json",
                    new JsonStringSerializer());

                var loader = new DirFileLoader(cache, Path);

                _playlist = new Playlist(loader, cache, new FilePlayer());
                _playlist.WhenLoaded += (sender, args) => WhenDirectoryLoaded();
                _playlist.WhenPlayed += (sender, args) => WhenFilePlayed(((Playlist.ItemEventArgs)args).Item);
                _playlist.WhenActiveItemChanged += (sender, args) => WhenActiveFileChanged(((Playlist.ItemEventArgs)args).Item);
                _playlist.Load();
            }

            return _filesViewModel;
        }


        public DirectoryViewModel()
        {
            _filesViewModel = new PlayableList();
        }


        private Playlist _playlist;
        private readonly PlayableList _filesViewModel;
        private string _name;
        private string _path;


        private void WhenActiveFileChanged(IPlaylistItem item)
        {
            // Update playlist
            var active = _filesViewModel.SetActive(item);
            AppStatus.Instance.Info($"Selected {item}", $"({active?.RowIndex}/{_playlist.Count})");
            OnPropertyChanged(nameof(FileListViewModel));
        }


        private void WhenFilePlayed(IPlaylistItem item)
        {
            // Update playlist
            var active = _filesViewModel.SetActive(item);
            AppStatus.Instance.Info($"Playing {item}", $"({active?.RowIndex}/{_playlist.Count})");
            OnPropertyChanged(nameof(FileListViewModel));
        }


        private void WhenDirectoryLoaded()
        {
            // Update playlist
            _playlist.Save(_filesViewModel);
            AppStatus.Instance.Info("", $"({_playlist.Count})");
            OnPropertyChanged(nameof(FileListViewModel));
        }

        public IPlaylistItem PreviousFile()
        {
            return _playlist.Prev;
        }

        public IPlaylistItem ActiveFile()
        {
            return _playlist.ActiveItem;
        }

        public IPlaylistItem NextFile()
        {
            return _playlist.Next;
        }

        public void SelectFile()
        {
            _playlist.ActiveItem = _playlist.FindById(FileListViewModel.Selected.Id);
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
