using System.Windows;


namespace MyMemory
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainWindowViewModel();
        }

        
        private void DisableScrollEventHandler(object sender, RequestBringIntoViewEventArgs e)
        {
            // Prevent autoscroll uppon cell selections
            // Such an stupid smart idea
            e.Handled = true;
        }
    }
}
