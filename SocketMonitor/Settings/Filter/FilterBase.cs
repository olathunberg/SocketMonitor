using GalaSoft.MvvmLight;
using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.Settings.Filter
{
    public class FilterBase
    {
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public virtual bool IsAffected(ConnectionModel model)
        {
            return false;
        }
    }
}
