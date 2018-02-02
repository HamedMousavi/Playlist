using System.Windows;


namespace MyMemory
{

    public partial class MainWindow
    {

        public MainWindow(MainWindowViewModel dataContext)
        {
            InitializeComponent();
            
            DataContext = dataContext;
        }

        
        private void DisableScrollEventHandler(object sender, RequestBringIntoViewEventArgs e)
        {
            // Prevent autoscroll uppon cell selections
            // Such an stupid smart idea
            e.Handled = true;
        }
    }
}
