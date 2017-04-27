using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public class ShadowsocksBase
    {
        public ShadowsocksBase(string managerAddress, int port, Action<EventLogEntryType, string> logger)
        {
            if (String.IsNullOrEmpty(managerAddress))
            {
                throw new ArgumentNullException(nameof(managerAddress));
            }

            _logger = logger;

            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(managerAddress, port);

            _socket.Send(SSCommand.Ping);
            ResultHandler();
        }
        protected Socket _socket;
        protected Action<EventLogEntryType, string> _logger;
        protected Timer _timer { get; set; }
        protected readonly static ICollection<Login> _inMemoryUsers = new List<Login>();
        public void SetTimer(Timer timer)
        {
            this._timer = timer;
        }
        protected void ResultHandler(Action onAddedOrRemoveUser = null, Action onReceiveStat = null)
        {
            byte[] bytes = new byte[32];
            _socket.Receive(bytes);
            int lastIndex = Array.FindLastIndex(bytes, b => b != 0);
            var result = Encoding.ASCII.GetString(bytes, 0, lastIndex + 1);
            if (result == "pong")
            {
                _logger(EventLogEntryType.SuccessAudit, "Connect to ssserver manage api.");
            }
            if (result == "ok")
            {
                onAddedOrRemoveUser?.Invoke();
                return;
            }

            _logger(EventLogEntryType.Information, result);
            onReceiveStat?.Invoke();
        }
    }
}
