using GalaSoft.MvvmLight;
using System;

namespace TTech.SocketMonitor.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Filters= new Filters();
        }

        public Filters Filters { get; set; }
    }
}
