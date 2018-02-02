using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MyMemory.Annotations;


namespace MyMemory
{

    public class AddDirectoryViewModel : INotifyPropertyChanged
    {

        private readonly Action<string, string> _addDirectoryAction;
        private string _dirPath;
        private string _dirTitle;


        public string DirPath
        {
            get => _dirPath;
            set { _dirPath = value; OnPropertyChanged(); }
        }


        public string DirTitle
        {
            get => _dirTitle;
            set { _dirTitle = value; OnPropertyChanged(); }
        }


        public ICommand BrowseDirectoryCommand => new RelayCommand(BrowseDirectory);
        public ICommand AddDirectoryCommand => new RelayCommand(AddDirectory);
        public ICommand CloseDirectoryCommand => new RelayCommand(a => ((Window)a?[0])?.Close());


        public AddDirectoryViewModel(Action<string, string> addDirectoryAction)
        {
            _addDirectoryAction = addDirectoryAction;
        }


        private void AddDirectory(object[] args)
        {
            _addDirectoryAction?.Invoke(DirTitle, DirPath);

            ((Window)args?[0])?.Close();
        }


        private void BrowseDirectory(object[] args)
        {
            var path = string.Empty;
            if (HLib.Gui.StandarWindowsDialogs.BrowseForDirectory(ref path))
            {
                DirPath = path;
            }
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