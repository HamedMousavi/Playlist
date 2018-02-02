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
            _playlistContainerStore = playlistContainerStore;

            PropagateStatusChangeEvent();

            Directories = new Directories();
            
            LoadPlaylists(_playlistContainerStore);
        }
        

        private void LoadPlaylists(IPlaylistContainerLoader loader)
        {
            _playlistContainer = new PlaylistContainer();
            _playlistContainer.Load(loader);
            _playlistContainer.Save(Directories);
        }


        private void PropagateStatusChangeEvent()
        {
            Status.PropertyChanged += (s, a) => OnPropertyChanged(nameof(Status));
        }


        public ICommand AddDirectoryCommand => new RelayCommand(AddDirectory);
        public ICommand PlayPreviousCommand => new RelayCommand(a => SelectedDirectory?.PreviousFile()?.Play(), a => SelectedDirectory != null);
        public ICommand PlayCurrentCommand => new RelayCommand(a => SelectedDirectory?.ActiveFile()?.Play(), a => SelectedDirectory != null);
        public ICommand PlayNextCommand => new RelayCommand(a => SelectedDirectory?.NextFile()?.Play(), a => SelectedDirectory != null);
        public ICommand SetIndexCommand => new RelayCommand(a => SelectedDirectory?.SelectFile(), a => SelectedDirectory != null);
        public AppStatus Status => AppStatus.Instance;
        public Directories Directories { get; }
        public DirectoryViewModel SelectedDirectory
        {
            get => _selectedDirectory;
            set { _selectedDirectory = value; OnPropertyChanged(); }
        }


        private void AddDirectory(object[] args)
        {
            new AddDirectory { DataContext = new AddDirectoryViewModel(AddDirectory), Owner = args?[0] as Window }.Show();
        }


        private void AddDirectory(string dirTitle, string dirPath)
        {
            _playlistContainer.Add(dirTitle, dirPath);
            _playlistContainer.Save(_playlistContainerStore);

            SelectedDirectory = Directories.Add(dirTitle, dirPath);
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
