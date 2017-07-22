using GalaSoft.MvvmLight;
using System;
using System.Timers;
using TTech.SocketMonitor.Lists;

namespace TTech.SocketMonitor
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Dispose();
                }

                Connections = null;

                disposedValue = true;
            }
        }
        #endregion
    }
}
