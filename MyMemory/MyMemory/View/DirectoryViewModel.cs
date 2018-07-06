using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyMemory.Annotations;
using MyMemory.Domain;
using MyMemory.Domain.Abstract;


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


        public System.Windows.Visibility IsProgressBarVisible { get; private set; }

        public int Progress { get; private set; }


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


        internal bool CanPlaySelected()
        {
            return _playlist != null
                && !_playlist.IsEmpty
                && _filesViewModel != null
                && _filesViewModel.Selected != null;
        }


        internal void PlaySelected()
        {
            // FileListViewModel.Selected
            _playlist.FindById(_filesViewModel.Selected.Id).Play();
        }


        public DirectoryViewModel()
        {
            _filesViewModel = new PlayableList();
            SetProgress();
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
            SetProgress();
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

            SetProgress();
        }


        private void SetProgress()
        {
            if (_playlist != null)
            {
                Progress = _playlist.ActiveIndex * 100 / _playlist.Count;
                OnPropertyChanged(nameof(Progress));
            }

            IsProgressBarVisible = Progress > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            OnPropertyChanged(nameof(IsProgressBarVisible));
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


        public bool CanPlayPrevious()
        {
            return _playlist != null && !_playlist.IsEmpty && _playlist.Prev != null;
        }


        public void PlayPrevious()
        {
            var prev = _playlist?.Prev;
            if (prev != null)
            {
                _playlist.ActiveItem = prev;
                prev.Play();
            }
        }


        public bool CanPlayActive()
        {
            return _playlist != null && !_playlist.IsEmpty && _playlist.ActiveItem != null;
        }


        public void PlayActive()
        {
            _playlist?.ActiveItem?.Play();
        }


        public bool CanPlayNext()
        {
            return _playlist != null && !_playlist.IsEmpty && _playlist.Next != null;
        }


        public void PlayNext()
        {
            var next = _playlist?.Next;
            if (next != null)
            {
                _playlist.ActiveItem = next;
                next.Play();
            }
        }
    }
}
