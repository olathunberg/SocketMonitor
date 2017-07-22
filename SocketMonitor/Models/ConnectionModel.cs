using System;
using System.Net;
using System.Net.NetworkInformation;
using TTech.SocketMonitor.SocketHelpers;

namespace TTech.SocketMonitor.Models
{
    public class ConnectionModel : GalaSoft.MvvmLight.ObservableObject, IEquatable<ConnectionModel>, IComparable
    {
        private readonly IPEndPoint localEndPoint;
        private readonly IPEndPoint remoteEndPoint;
        private readonly int processId;
        private readonly SocketProtocol protocol;
        private readonly DateTime connectTime;
        private TcpState? state;
        private string remoteHostName;

        private string path;
        private SocketState sockState;
        private string processName;
        private string localHostName;
        private DateTime lastChange;

        public ConnectionModel(IpHelper.TcpRow tcpRow)
        {
            processId = tcpRow.owningPid;

            int localPort = (tcpRow.localPort1 << 8) + (tcpRow.localPort2) + (tcpRow.localPort3 << 24) + (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            localEndPoint = new IPEndPoint(localAddress, localPort);

            int remotePort = (tcpRow.remotePort1 << 8) + (tcpRow.remotePort2) + (tcpRow.remotePort3 << 24) + (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            remoteEndPoint = new IPEndPoint(remoteAddress, remotePort);

            protocol = SocketProtocol.TCP;
            connectTime = DateTime.Now;

            lastChange = connectTime;
            state = tcpRow.state;
            sockState = SocketState.New;
        }

        public ConnectionModel(IpHelper.UdpRow udpRow)
        {
            processId = udpRow.owningPid;

            int localPort = (udpRow.localPort1 << 8) + (udpRow.localPort2) + (udpRow.localPort3 << 24) + (udpRow.localPort4 << 16);
            long localAddress = udpRow.localAddr;
            localEndPoint = new IPEndPoint(localAddress, localPort);
            remoteEndPoint = new IPEndPoint(0, 0);

            protocol = SocketProtocol.UDP;
            connectTime = DateTime.Now;
            lastChange = connectTime;
            sockState = SocketState.New;
        }

        public void Update(ConnectionModel model)
        {
            if (state != model.state)
            {
                state = model.state;
                sockState = SocketState.Changed;
                lastChange = DateTime.Now;
                RaisePropertyChanged(() => TcpState);
                RaisePropertyChanged(() => State);
            }
        }

        #region Public properties
        public IPEndPoint LocalEndPoint => localEndPoint;

        public IPEndPoint RemoteEndPoint => remoteEndPoint;

        public TcpState? TcpState => state;

        public DateTime Connected => connectTime;

        public DateTime LastChange => lastChange;

        public int ProcessId => processId;

        public string Process => processName;

        public SocketState State => sockState;

        public SocketProtocol Protocol => protocol;

        public string Path => path;

        public string LocalHostName => localHostName;

        public string RemoteHostName => protocol == SocketProtocol.TCP ? remoteHostName : string.Empty;
        #endregion

        public void SetProcessName(string processName)
        {
            this.processName = processName;
        }

        public void SetLocalHostName(string hostName)
        {
            this.localHostName = hostName;
            RaisePropertyChanged(() => LocalHostName);
        }

        public void SetRemoteHostName(string hostName)
        {
            if (protocol == SocketProtocol.TCP)
            {
                this.remoteHostName = hostName;
                RaisePropertyChanged(() => RemoteHostName);
            }
        }

        public void SetPath(string path)
        {
            this.path = path;
            RaisePropertyChanged(() => Path);
        }

        public void SetSocketState(SocketState state)
        {
            this.lastChange = DateTime.Now;
            this.sockState = state;
            RaisePropertyChanged(() => State);
        }

        public override bool Equals(object otherObj)
        {
            var x = otherObj as ConnectionModel;
            var y = this;

            if (x == null)
                return false;

            return x.LocalEndPoint.Address.Equals(y.LocalEndPoint.Address) &&
                   x.LocalEndPoint.Port.Equals(y.LocalEndPoint.Port) &&
                   x.RemoteEndPoint.Address.Equals(y.RemoteEndPoint.Address) &&
                   x.RemoteEndPoint.Port.Equals(y.RemoteEndPoint.Port) &&
                   x.ProcessId == y.ProcessId &&
                   x.Protocol == y.Protocol;
        }

        public override int GetHashCode()
        {
            int hCode = LocalEndPoint.GetHashCode() ^ RemoteEndPoint.GetHashCode() ^ ProcessId;
            return hCode.GetHashCode();
        }

        public bool Equals(ConnectionModel other)
        {
            return Equals((object)other);
        }

        public int CompareTo(object obj)
        {
            return processId.CompareTo((obj as ConnectionModel).ProcessId);
        }
    }
}
