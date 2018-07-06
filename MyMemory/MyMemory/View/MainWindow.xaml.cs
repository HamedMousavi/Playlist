using System;
using System.Windows;


namespace MyMemory
{

    public partial class MainWindow
    {

        private WindowPlacementManager _placement;
        private bool _scrolling;


        public MainWindow(MainWindowViewModel dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;

            TrackWindowSizeAndPosition();

            dataContext.PropertyChanged += MainWindowViewModelPropertyChanged;
        }


        private void MainWindowViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(MainWindowViewModel.SelectedPlayableViewModel), System.StringComparison.InvariantCultureIgnoreCase))
            {
                var selection = ((MainWindowViewModel)DataContext).SelectedPlayableViewModel;
                ScrollToSelection(selection);
            }
        }


        private void ScrollToSelection(Playable selection)
        {
            _scrolling = true;

            FileListDataGrid.UpdateLayout();
            FileListDataGrid.ScrollIntoView(selection);

            _scrolling = false;
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
            if (!_scrolling) e.Handled = true;
        }
    }
}
