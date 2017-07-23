using System.Net;
using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.Settings.Filter
{
    public class LocalEndPointAnyFilter : FilterBase
    {
        public LocalEndPointAnyFilter()
        {
            Description = "LocalEndPointAny";
        }

        public override bool IsAffected(ConnectionModel model)
        {
            return model.LocalEndPoint.Address.Equals(IPAddress.Any);
        }
    }
}
