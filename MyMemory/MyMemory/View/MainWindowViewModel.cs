using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MyMemory.Annotations;


namespace MyMemory
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private DirectoryViewModel _selectedDirectory;


        public MainWindowViewModel()
        {
            Directories = new ObservableCollection<DirectoryViewModel>();
            AppStatus.Instance.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(Status));
        }


        public ICommand AddDirectoryCommand => new RelayCommand(AddDirectory);
        public ICommand PlayPreviousCommand => new RelayCommand(a => SelectedDirectory?.PlayPreviousFile());
        public ICommand PlayCurrentCommand => new RelayCommand(a => SelectedDirectory?.PlayCurrentFile());
        public ICommand PlayNextCommand => new RelayCommand(a => SelectedDirectory?.PlayNextFile());
        public AppStatus Status => AppStatus.Instance;
        public ObservableCollection<DirectoryViewModel> Directories { get; }
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
            var dir = new DirectoryViewModel { Name = dirTitle, Path = dirPath };
            Directories.Add(dir);
            SelectedDirectory = dir;
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
