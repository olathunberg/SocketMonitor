using System.Windows;

namespace TTechSocketMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
            base.OnStartup(e);
        }
    }
}
