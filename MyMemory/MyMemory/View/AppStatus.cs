using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyMemory.Annotations;


namespace MyMemory
{

    public sealed class AppStatus : INotifyPropertyChanged
    {

        private static AppStatus _instance;
        private string _message;
        private string _count;
        public static AppStatus Instance => _instance ?? (_instance = new AppStatus());


        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }


        public string Count
        {
            get => _count;
            set { _count = value; OnPropertyChanged(); }
        }


        private AppStatus()
        {

        }


        public void Info(string content, string count)
        {
            Message = content;
            Count = count;
        }


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion PropertyChanged

    }
}
