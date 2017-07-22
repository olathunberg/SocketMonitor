using System.Collections.Generic;
using System.Linq;
using TTech.SocketMonitor.SocketHelpers;

namespace TTech.SocketMonitor.Lists
{
    internal class UdpList
    {
        private readonly List<IpHelper.UdpRow> UdpRows = new List<IpHelper.UdpRow>();
        private readonly List<IpHelper.UdpRow> newRows = new List<IpHelper.UdpRow>();
        private readonly List<IpHelper.UdpRow> removedRows = new List<IpHelper.UdpRow>();

        internal (List<IpHelper.UdpRow> newRows, List<IpHelper.UdpRow> removedRows) Update()
        {
            var udpTable = ManagedIpHelper.GetExtendedUdpTable();
            newRows.Clear();
            removedRows.Clear();

            foreach (var item in udpTable)
            {
                var existing = UdpRows.FirstOrDefault(x => Compare(x, item));
                if (IsZero(existing))
                {
                    newRows.Add(item);
                    UdpRows.Add(item);
                }
            }
            foreach (var item in UdpRows)
            {
                var remaining = udpTable.FirstOrDefault(x => Compare(x, item));
                if (IsZero(remaining))
                {
                    removedRows.Add(item);
                }
            }
            foreach (var item in removedRows)
            {
                UdpRows.Remove(item);
            }

            return (newRows, removedRows);
        }

        private bool IsZero(IpHelper.UdpRow row)
        {
            return row.localAddr == 0
                && row.localPort1 == 0
                && row.localPort2 == 0
                && row.localPort3 == 0
                && row.localPort4 == 0
                && row.owningPid == 0;
        }

        private bool Compare(IpHelper.UdpRow rowA, IpHelper.UdpRow rowB)
        {
            return rowA.localAddr == rowB.localAddr
                && rowA.localPort1 == rowB.localPort1
                && rowA.localPort2 == rowB.localPort2
                && rowA.localPort3 == rowB.localPort3
                && rowA.localPort4 == rowB.localPort4
                && rowA.owningPid == rowB.owningPid;
        }
    }
}
