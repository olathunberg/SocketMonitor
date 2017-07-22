using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace TTech.SocketMonitor.SocketHelpers
{
    /// <summary>
    /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366073.aspx"/>
    /// </summary>
    public static class IpHelper
    {
        public const string DllName = "iphlpapi.dll";
        public const int AfInet = 2;

        public static class NativeMethods
        {
            /// <summary>
            /// <see cref="http://msdn2.microsoft.com/en-us/library/aa365928.aspx"/>
            /// </summary>
            [DllImport(IpHelper.DllName, SetLastError = true)]
            internal static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);

            /// <summary>
            /// <see cref="http://msdn.microsoft.com/en-us/library/aa365930(VS.85).aspx"/>
            /// </summary>
            [DllImport(IpHelper.DllName, SetLastError = true)]
            internal static extern uint GetExtendedUdpTable(IntPtr udpTable, ref int udpTableLength, bool sort, int ipVersion, UdpTableType udpTableType, int reserved);
        }

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366386.aspx"/>
        /// </summary>
        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }

        /// <summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/aa366388(VS.85).aspx"/>
        /// </summary>
        public enum UdpTableType
        {
            Basic,
            OwnerPid,
            OwnerModule,
        }

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366921.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpTable
        {
            public uint length;
            public TcpRow row;
        }

        /// <summary>
        /// <see cref=""/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct UdpTable
        {
            public uint length;
            public UdpRow row;
        }

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366913.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpRow
        {
            public TcpState state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
        }

        /// <summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/aa366926(VS.85).aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct UdpRow
        {
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public int owningPid;
        }
    }
}
