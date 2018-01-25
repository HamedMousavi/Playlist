using System.ComponentModel;
using System.Windows;

namespace MyMemory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private PlayList _playlist;
        private string _playBackDetail;
        private string _playbackButtonText;
        private string _path;

        public event PropertyChangedEventHandler PropertyChanged;


        public string Path
        {
            get => _path;
            set
            {
                _path = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
            }
        }


        public string PlayBackDetail
        {
            get => _playBackDetail; set
            {
                _playBackDetail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayBackDetail)));
            }
        }


        public string PlaybackButtonText
        {
            get => _playbackButtonText; set
            {
                _playbackButtonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaybackButtonText)));
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            PlaybackButtonText = "Play";
            this.DataContext = this;
        }


        private void BrowseClicked(object sender, RoutedEventArgs e)
        {
            var path = string.Empty;
            if (HLib.Gui.StandarWindowsDialogs.BrowseForDirectory(ref path))
            {
                Path = path;
            }
        }


        private void CreatePlayList()
        {
            if (_playlist != null)
                _playlist.PropertyChanged -= PlaylistChanged;

            _playlist = new PlayList(new PlayListFileStore(Path, new JsonStringSerializer()));

            _playlist.PropertyChanged += PlaylistChanged;
        }


        private void PlaylistChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Threading.SynchronizationContext.Current.Send(
                state => PlayBackDetail = $"Playing {_playlist.PlayingFile}: ({_playlist.PlayingIndex}/{_playlist.Count})", null);
        }


        private void PlayClicked(object sender, RoutedEventArgs e)
        {
            CreatePlayList();
            _playlist.Play();
        }


        private void PlayPrevousClicked(object sender, RoutedEventArgs e)
        {
            CreatePlayList();
            _playlist.PlayPrevious();
        }


        private void PlayNextClicked(object sender, RoutedEventArgs e)
        {
            CreatePlayList();
            _playlist.PlayNext();
        }
    }
}
