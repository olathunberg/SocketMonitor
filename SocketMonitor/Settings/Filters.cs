using GalaSoft.MvvmLight;

namespace TTech.SocketMonitor.Settings
{
    public class Filters : ObservableObject
    {
        public Filters()
        {
            LocalHost = true;
        }

        public bool LocalHost { get; set; }
    }

}
