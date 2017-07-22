using GalaSoft.MvvmLight;
using System;
using System.Timers;
using TTech.SocketMonitor.Lists;

namespace TTech.SocketMonitor
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Timer timer = new Timer(500);

        public MainWindowViewModel()
        {
            Settings = new Settings.SettingsViewModel();
            Connections = new ConnectionList(Settings.Filters);

            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public Settings.SettingsViewModel Settings { get; set; }

        public ConnectionList Connections { get; set; }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            Connections.Update();

            timer.Start();
        }

        public override void Cleanup()
        {
            timer.Dispose();
            Connections = null;
            base.Cleanup();
        }
    }
}
