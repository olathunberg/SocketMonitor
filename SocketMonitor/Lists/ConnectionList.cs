using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using TTech.SocketMonitor.Models;
using TTech.SocketMonitor.Settings;
using TTech.SocketMonitor.Settings.Filter;
using TTech.SocketMonitor.SocketHelpers;

namespace TTech.SocketMonitor.Lists
{
    public sealed class ConnectionList : ObservableCollection<ConnectionModel>
    {
        private readonly IEnumerable<FilterBase> filters;
        private readonly TcpList tcpRows = new TcpList();
        private readonly UdpList udpRows = new UdpList();

        public ConnectionList(IEnumerable<FilterBase> filters)
        {
            this.filters = filters;
        }

        public void Update()
        {
            var tcpChanges = tcpRows.Update();
            UpdateTcpRows(tcpChanges.newRows, tcpChanges.changedRows, tcpChanges.removedRows);
            var udpChanges = udpRows.Update();
            UpdateUdpRows(udpChanges.newRows, udpChanges.removedRows);

            if (tcpChanges.newRows.Count > 0 || udpChanges.newRows.Count > 0)
                Sort();

            foreach (var item in Items)
            {
                item.SetIsFiltered(filters);
            }
        }

        private new void Add(ConnectionModel model)
        {
            model.SetIsFiltered(filters);
            try
            {
                var process = System.Diagnostics.Process.GetProcessById(model.ProcessId);
                model.SetProcessName(process.ProcessName);
            }
            catch
            {
                // Process has exited...
            }

            if (model.RemoteEndPoint != null && !model.RemoteEndPoint.Address.Equals(new IPAddress(0)))
                System.Net.Dns.BeginGetHostEntry(model.RemoteEndPoint.Address.ToString(),
                    x =>
                        {
                            var row = (ConnectionModel)x.AsyncState;

                            try { row.SetRemoteHostName(Dns.EndGetHostEntry(x).HostName); }
                            catch { row.SetRemoteHostName("Host not found"); }
                        }, model);

            // TODO: Insert at correct position
            base.Add(model);
        }

        private void Sort()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() => this.BubbleSort());
        }

        private void UpdateTcpRows(List<IpHelper.TcpRow> newRows, List<IpHelper.TcpRow> changedRows, List<IpHelper.TcpRow> removedRows)
        {
            foreach (var item in changedRows)
            {
                var model = new ConnectionModel(item);
                var existing = this.First(x => x.Equals(model));
                existing.Update(model);
            }

            foreach (var item in removedRows)
            {
                var model = new ConnectionModel(item);
                var existing = this.First(x => x.Equals(model));
                if (existing.State != SocketState.Closed)
                {
                    existing.SetSocketState(SocketState.Closed);
                }
            }

            var now = DateTime.Now.AddMilliseconds(-2000);
            var removed = this.Where(x => x.State == SocketState.Closed && (now - x.LastChange).TotalMilliseconds > 0).ToList();
            if (newRows.Count > 0 || removed.Count > 0)
            {
                GalaSoft.MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    foreach (var item in newRows)
                    {
                        Add(new ConnectionModel(item));
                    }

                    foreach (var item in removed)
                    {
                        base.Remove(item);
                    }
                });
            }

            foreach (var item in this.Where(x => x.State != SocketState.Steady && (now - x.LastChange).TotalMilliseconds > 0))
                item.SetSocketState(SocketState.Steady);
        }

        private void UpdateUdpRows(List<IpHelper.UdpRow> newRows, List<IpHelper.UdpRow> removedRows)
        {
            foreach (var item in removedRows)
            {
                var model = new ConnectionModel(item);
                var existing = this.FirstOrDefault(x => x.Equals(model));
                if (existing != null && existing.State != SocketState.Closed)
                {
                    existing.SetSocketState(SocketState.Closed);
                }
            }

            var now = DateTime.Now.AddMilliseconds(-2000);
            var removed = this.Where(x => x.State == SocketState.Closed && (now - x.LastChange).TotalMilliseconds > 0).ToList();

            if (newRows.Count > 0 || removed.Count > 0)
            {
                GalaSoft.MvvmLight.Threading.DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    foreach (var item in newRows)
                    {
                        Add(new ConnectionModel(item));
                    }

                    foreach (var item in removed)
                    {
                        base.Remove(item);
                    }
                });
            }

            foreach (var item in this.Where(x => x.State != SocketState.Steady && (now - x.LastChange).TotalMilliseconds > 0))
            {
                item.SetSocketState(SocketState.Steady);
            }
        }
    }
}
