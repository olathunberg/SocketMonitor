using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TTech.SocketMonitor.Models;

namespace TTech.SocketMonitor.SocketHelpers
{
    public static class ManagedIpHelper
    {
        public static IList<IpHelper.TcpRow> GetExtendedTcpTable()
        {
            var tcpRows = new List<IpHelper.TcpRow>();

            var tcpTable = IntPtr.Zero;
            var tcpTableLength = 0;

            if (IpHelper.NativeMethods.GetExtendedTcpTable(tcpTable, ref tcpTableLength, false, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                    if (IpHelper.NativeMethods.GetExtendedTcpTable(tcpTable, ref tcpTableLength, false, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        var table = (IpHelper.TcpTable)Marshal.PtrToStructure(tcpTable, typeof(IpHelper.TcpTable));
                        var rowPtr = (IntPtr)((long)tcpTable + Marshal.SizeOf(table.length));

                        for (int i = 0; i < table.length; ++i)
                        {
                            var marshalPtrToStructure = (IpHelper.TcpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.TcpRow));

                            if (!tcpRows.Contains(marshalPtrToStructure))
                                tcpRows.Add(marshalPtrToStructure);

                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.TcpRow)));
                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(tcpTable);
                    }
                }
            }

            return tcpRows;
        }

        public static IList<IpHelper.UdpRow> GetExtendedUdpTable()
        {
            List<IpHelper.UdpRow> udpRows = new List<IpHelper.UdpRow>();

            IntPtr udpTable = IntPtr.Zero;

            int udpTableLength = 0;

            if (IpHelper.NativeMethods.GetExtendedUdpTable(udpTable, ref udpTableLength, false, IpHelper.AfInet, IpHelper.UdpTableType.OwnerPid, 0) != 0)
            {
                try
                {
                    udpTable = Marshal.AllocHGlobal(udpTableLength);
                    if (IpHelper.NativeMethods.GetExtendedUdpTable(udpTable, ref udpTableLength, false, IpHelper.AfInet, IpHelper.UdpTableType.OwnerPid, 0) == 0)
                    {
                        IpHelper.UdpTable table = (IpHelper.UdpTable)Marshal.PtrToStructure(udpTable, typeof(IpHelper.UdpTable));

                        IntPtr rowPtr = (IntPtr)((long)udpTable + Marshal.SizeOf(table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            var marshalPtrToStructure = (IpHelper.UdpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.UdpRow));

                            if (!udpRows.Contains(marshalPtrToStructure))
                                udpRows.Add(marshalPtrToStructure);

                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.UdpRow)));
                        }
                    }
                }
                finally
                {
                    if (udpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(udpTable);
                    }
                }
            }

            return udpRows;
        }
    }
}
