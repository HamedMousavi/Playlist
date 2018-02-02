using System.Windows;


namespace MyMemory
{

    public partial class App
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loader = new BootStrapper();

            loader.Bootstrap();
            loader.Locate<MainWindow>().Show();
        }
    }
}
