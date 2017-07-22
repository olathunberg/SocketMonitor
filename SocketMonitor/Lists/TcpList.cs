using System.Collections.Generic;
using System.Linq;
using TTech.SocketMonitor.SocketHelpers;

namespace TTech.SocketMonitor.Lists
{
    internal class TcpList
    {
        private readonly List<IpHelper.TcpRow> tcpRows = new List<IpHelper.TcpRow>();
        private readonly List<IpHelper.TcpRow> newRows = new List<IpHelper.TcpRow>();
        private readonly List<IpHelper.TcpRow> changedRows = new List<IpHelper.TcpRow>();
        private readonly List<IpHelper.TcpRow> removedRows = new List<IpHelper.TcpRow>();

        internal (List<IpHelper.TcpRow> newRows, List<IpHelper.TcpRow> changedRows, List<IpHelper.TcpRow> removedRows) Update()
        {
            var tcpTable = ManagedIpHelper.GetExtendedTcpTable();
            newRows.Clear();
            changedRows.Clear();
            removedRows.Clear();

            foreach (var item in tcpTable)
            {
                var existing = tcpRows.FirstOrDefault(x => Compare(x, item));
                if (!IsZero(existing) && existing.state != item.state)
                {
                    changedRows.Add(existing);
                }
                else if (IsZero(existing))
                {
                    newRows.Add(item);
                    tcpRows.Add(item);
                }
            }
            foreach (var item in tcpRows)
            {
                var remaining = tcpTable.FirstOrDefault(x => Compare(x, item));
                if (IsZero(remaining))
                {
                    removedRows.Add(item);
                }
            }
            foreach (var item in removedRows)
            {
                tcpRows.Remove(item);
            }

            return (newRows, changedRows, removedRows);
        }

        private bool IsZero(IpHelper.TcpRow row)
        {
            return row.localAddr == 0
                && row.localPort1 == 0
                && row.localPort2 == 0
                && row.localPort3 == 0
                && row.localPort4 == 0
                && row.remoteAddr == 0
                && row.remotePort1 == 0
                && row.remotePort2 == 0
                && row.remotePort3 == 0
                && row.remotePort4 == 0
                && row.owningPid == 0;
        }

        private bool Compare(IpHelper.TcpRow rowA, IpHelper.TcpRow rowB)
        {
            return rowA.localAddr == rowB.localAddr
                && rowA.localPort1 == rowB.localPort1
                && rowA.localPort2 == rowB.localPort2
                && rowA.localPort3 == rowB.localPort3
                && rowA.localPort4 == rowB.localPort4
                && rowA.remoteAddr == rowB.remoteAddr
                && rowA.remotePort1 == rowB.remotePort1
                && rowA.remotePort2 == rowB.remotePort2
                && rowA.remotePort3 == rowB.remotePort3
                && rowA.remotePort4 == rowB.remotePort4
                && rowA.owningPid == rowB.owningPid;
        }
    }
}
