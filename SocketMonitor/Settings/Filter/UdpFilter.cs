using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.Settings.Filter
{
    public class UdpFilter : FilterBase
    {
        public UdpFilter()
        {
            Description = "Udp";
        }

        public override bool IsAffected(ConnectionModel model)
        {
            return model.Protocol == SocketProtocol.UDP;
        }
    }
}
