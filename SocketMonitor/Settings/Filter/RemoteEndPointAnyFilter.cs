using System.Net;
using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.Settings.Filter
{
    public class RemoteEndPointAnyFilter : FilterBase
    {
        public RemoteEndPointAnyFilter()
        {
            Description = "RemoteEndPointAny";
        }

        public override bool IsAffected(ConnectionModel model)
        {
            return model.RemoteEndPoint.Address.Equals(IPAddress.Any);
        }
    }
}
