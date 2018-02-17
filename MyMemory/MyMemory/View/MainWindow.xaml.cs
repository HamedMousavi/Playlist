using System.Windows;


namespace MyMemory
{

    public partial class MainWindow
    {

        private WindowPlacementManager _placement;


        public MainWindow(MainWindowViewModel dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;

            TrackWindowSizeAndPosition();
        }


        private void TrackWindowSizeAndPosition()
        {
            _placement = new WindowPlacementManager(new WindowPlacementStore());
            _placement.Attach(this);
        }


        private void DisableScrollEventHandler(object sender, RequestBringIntoViewEventArgs e)
        {
            // Prevent autoscroll uppon cell selections
            // Such an stupid smart idea
            e.Handled = true;
        }
    }
}
