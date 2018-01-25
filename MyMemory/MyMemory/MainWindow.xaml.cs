using System.ComponentModel;
using System.Windows;

namespace MyMemory
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {

        private PlayList _playlist;
        private string _playBackDetail;
        private string _path;

        public event PropertyChangedEventHandler PropertyChanged;


        public PlayableList FileList { get; private set; }


        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));

                PlayList.Load();
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


        public MainWindow()
        {
            InitializeComponent();

            FileList = new PlayableList();
            DataContext = this;
        }


        private void DisableScrollEventHandler(object sender, RequestBringIntoViewEventArgs e)
        {
            // Prevent autoscroll uppon cell selections
            // Such an stupid smart idea
            e.Handled = true;
        }


        private void BrowseClicked(object sender, RoutedEventArgs e)
        {
            var path = string.Empty;
            if (HLib.Gui.StandarWindowsDialogs.BrowseForDirectory(ref path))
            {
                Path = path;
            }
        }


        private IPlayList PlayList
        {
            get
            {
                if (_playlist == null)
                {
                    var cache = new FileCache(
                        $"{Path}{System.IO.Path.DirectorySeparatorChar}playlist.json",
                        new JsonStringSerializer());

                    var loader = new DirectoryLoader(cache, Path);

                    _playlist = new PlayList(loader, cache, new FilePlayer(Path));
                    _playlist.WhenLoaded += (sender, args) => UpdateStatus(null);
                    _playlist.WhenPlayed += (sender, args) => UpdateStatus(((PlayList.ItemEventArgs)args).Item);
                }

                return _playlist;
            }
        }


        private void UpdateStatus(IPlayListItem item)
        {
            System.Threading.SynchronizationContext.Current.Send(
                state => PlayBackDetail = $"Playing {item}: ({_playlist.SelectedIndex + 1}/{_playlist.Count})", null);

            if (item == null) PlayList.Save(FileList);
            else FileList.Select(PlayList.IndexOf(item));
        }


        private void PlayClicked(object sender, RoutedEventArgs e)
        {
            PlayList.Current().Play();
        }


        private void PlayPrevousClicked(object sender, RoutedEventArgs e)
        {
            PlayList.Prev().Play();
        }


        private void PlayNextClicked(object sender, RoutedEventArgs e)
        {
            PlayList.Next().Play();
        }
    }
}
