using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MyMemory.Annotations;
using MyMemory.Domain;
using MyMemory.View;


namespace MyMemory
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private DirectoryViewModel _selectedDirectory;
        private readonly DirList _directories;


        public MainWindowViewModel()
        {
            var dirStore = new DirectoryListStore(
                System.IO.Path.Combine(HLib.Io.PathUtil.ApplicationDirectory, "playlist.json"),
                new JsonStringSerializer());

            _directories = new DirList(dirStore, dirStore);
            _directories.Load();

            Directories = new Directories();
            _directories.Save(Directories);

            AppStatus.Instance.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Status));
        }


        public ICommand AddDirectoryCommand => new RelayCommand(AddDirectory);
        public ICommand PlayPreviousCommand => new RelayCommand(a => SelectedDirectory?.PlayPreviousFile());
        public ICommand PlayCurrentCommand => new RelayCommand(a => SelectedDirectory?.PlayCurrentFile());
        public ICommand PlayNextCommand => new RelayCommand(a => SelectedDirectory?.PlayNextFile());
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
            _directories.Add(dirTitle, dirPath);
            _directories.Save();

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
