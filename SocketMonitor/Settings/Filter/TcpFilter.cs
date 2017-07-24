using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.Settings.Filter
{
    public class TcpFilter : FilterBase
    {
        public TcpFilter()
        {
            Description = "Tcp";
        }

        public override bool IsAffected(ConnectionModel model)
        {
            return model.Protocol == SocketProtocol.TCP;
        }
    }
}
