using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using TTech.SocketMonitor.Settings.Filter;

namespace TTech.SocketMonitor.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Filters = new List<FilterBase>
            {
                new LocalEndPointAnyFilter
                {
                    IsEnabled = true
                },
                new RemoteEndPointAnyFilter
                {
                    IsEnabled = true
                },
                new TcpFilter(),
                new UdpFilter()
            };
        }

        public IEnumerable<FilterBase> Filters { get; set; }
    }
}
