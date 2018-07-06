using MyMemory.Annotations;
using MyMemory.Domain;
using MyMemory.Domain.Abstract;
using MyMemory.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace MyMemory
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private DirectoryViewModel _selectedDirectory;
        private PlaylistContainer _playlistContainer;
        private readonly IPlaylistContainerStore _playlistContainerStore;


        public MainWindowViewModel(IPlaylistContainerStore playlistContainerStore)
        {
            Directories = new Directories();

            _playlistContainerStore = playlistContainerStore;

            PropagateStatusChangeEvent();

            LoadPlaylists();
        }


        private void LoadPlaylists()
        {
            _playlistContainer = new PlaylistContainer();
            _playlistContainer.Load(_playlistContainerStore);
            _playlistContainer.Save(Directories);
        }


        private void PropagateStatusChangeEvent()
        {
            Status.PropertyChanged += (s, a) => OnPropertyChanged(nameof(Status));
        }


        public string DirectoriesDetail => $"Directories ({(Directories == null ? 0 : Directories.Count)})";
        public string FilesDetails => $"Files ({(PlayListViewModel == null ? 0 : PlayListViewModel.Count)})";
        public ICommand PlaySelectedCommand => new RelayCommand(a => SelectedDirectory?.PlaySelected(), a => SelectedDirectory != null && SelectedDirectory.CanPlaySelected());
        public ICommand AddDirectoryCommand => new RelayCommand(WhenAddDirectoryButtonClicked);
        public ICommand RemoveDirectoryCommand => new RelayCommand(WhenRemoveDirectoryButtonClicked, a => SelectedDirectory != null);
        public ICommand PlayPreviousCommand => new RelayCommand(a => SelectedDirectory?.PlayPrevious(), a => SelectedDirectory != null && SelectedDirectory.CanPlayPrevious());
        public ICommand PlayCurrentCommand => new RelayCommand(a => SelectedDirectory?.PlayActive(), a => SelectedDirectory != null && SelectedDirectory.CanPlayActive());
        public ICommand PlayNextCommand => new RelayCommand(a => SelectedDirectory?.PlayNext(), a => SelectedDirectory != null && SelectedDirectory.CanPlayNext());
        public ICommand SetIndexCommand => new RelayCommand(a => SelectedDirectory?.SelectFile(), a => SelectedDirectory != null);
        public AppStatus Status => AppStatus.Instance;
        public Directories Directories { get; }
        public DirectoryViewModel SelectedDirectory
        {
            get => _selectedDirectory;
            set
            {
                _selectedDirectory = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PlayListViewModel));
                OnPropertyChanged(nameof(FilesDetails));

                if (_selectedDirectory != null &&
                    _selectedDirectory.FileListViewModel != null)
                {
                    var active = _selectedDirectory.FileListViewModel.GetActive();
                    if (active != null)
                    {
                        SelectedPlayableViewModel = active;
                    }
                }
            }
        }

        public Playable SelectedPlayableViewModel
        {
            get { return SelectedDirectory?.FileListViewModel.Selected; }
            set { SelectedDirectory.FileListViewModel.Selected = value; OnPropertyChanged(); }
        }

        public PlayableList PlayListViewModel => SelectedDirectory?.FileListViewModel;


        private void WhenAddDirectoryButtonClicked(object[] args)
        {
            CreateAddDirectoryWindow(args?[0] as Window).Show();
        }


        private void WhenRemoveDirectoryButtonClicked(object[] args)
        {
            _playlistContainer.Remove(SelectedDirectory?.Path);
            _playlistContainer.Save(_playlistContainerStore);
            _playlistContainer.Save(Directories);
            SelectedDirectory = null;
            OnPropertyChanged(nameof(DirectoriesDetail));
        }


        private Window CreateAddDirectoryWindow(Window owner)
        {
            return new AddDirectory
            {
                DataContext = new AddDirectoryViewModel(WhenPlaylistAdded),
                Owner = owner
            };
        }


        private void WhenPlaylistAdded(string dirTitle, string dirPath)
        {
            SaveAddedPlaylist(dirTitle, dirPath);
            SelectAddedPlaylistInUi(dirTitle, dirPath);
            OnPropertyChanged(nameof(DirectoriesDetail));
        }


        private void SaveAddedPlaylist(string dirTitle, string dirPath)
        {
            _playlistContainer.Add(dirTitle, dirPath);
            _playlistContainer.Save(_playlistContainerStore);
        }


        private void SelectAddedPlaylistInUi(string dirTitle, string dirPath)
        {
            var viewModel = Directories.CreateViewModel(dirTitle, dirPath);
            Directories.Add(viewModel);
            SelectedDirectory = viewModel;
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
