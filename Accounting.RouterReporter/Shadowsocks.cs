using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedProject;

namespace Accounting.RouterReporter
{
    public sealed class Shadowsocks
    {
        public Shadowsocks(string managerAddress, int port, string machineName, ILog logger, IRepo repo)
        {
            if (String.IsNullOrEmpty(managerAddress))
            {
                throw new ArgumentNullException(nameof(managerAddress));
            }

            _logger = logger;
            _machineName = machineName;
            _repo = repo;
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(managerAddress, port);
            _socket.Send(SSCommand.Ping);
            ResultHandler();
        }

        Socket _socket;
        ILog _logger;
        Timer _timer { get; set; }
        byte[] buffer = new byte[1024];
        readonly ICollection<Login> _inMemoryUsers = new List<Login>();
        string _machineName;
        IRepo _repo;
        LoginComparer _loginComparer = new LoginComparer();

        private void ReportShadowsocksStat(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                _socket.Receive(buffer);
                int lastIndex = Array.FindIndex(buffer, b => b == 0) - 1;
                message = Encoding.ASCII.GetString(buffer, 0, lastIndex + 1);
                Array.Clear(buffer, 0, lastIndex + 1);
            }

            var stat = ShadowsocksStat.Parse(message);
            List<SSEventraw> events = new List<SSEventraw>();
            foreach (var item in stat)
            {
                var login = _inMemoryUsers.FirstOrDefault(c => c.Port == item.Port);
                if (login != null)
                {
                    events.Add(new SSEventraw()
                    {
                        MachineName = _machineName,
                        TimeStamp = DateTime.UtcNow,
                        TotalBytesInOut = item.Traffic,
                        UserName = login.LoginName
                    });
                }
            }
            if (events.Count > 0)
            {
                this._repo.InsertSSEventRaws(events);
            }
        }

        public void Fetch(object state)
        {
            try
            {
                IEnumerable<Login> lastLogins = _repo.GetLiveUsers();

                var loginsToRemove = _inMemoryUsers.Except(lastLogins, _loginComparer).ToList();
                if (loginsToRemove.Count != 0)
                {
                    foreach (var user in loginsToRemove)
                    {
                        _socket.Send(SSCommand.RemoveUser(user.Port));
                        ResultHandler(new Action(() =>
                        {
                            var rm = _inMemoryUsers.FirstOrDefault(c => c.LoginName == user.LoginName);
                            _inMemoryUsers.Remove(rm);
                            _logger.Log(EventLogEntryType.Information, $"Remove {user.LoginName}");
                        }), ReportShadowsocksStat, tryGetOkResult: true);
                    }
                    lastLogins = _repo.GetLiveUsers();
                }
                var loginsToAdd = lastLogins.Except(_inMemoryUsers, _loginComparer).ToList();
                if (loginsToAdd.Count != 0)
                {
                    foreach (var user in loginsToAdd)
                    {
                        _socket.Send(SSCommand.AddUser(user.Port, user.LoginName, user.Password));
                        ResultHandler(new Action(() =>
                        {
                            _inMemoryUsers.Add(user);
                            _logger.Log(EventLogEntryType.Information, $"Add {user.LoginName}");
                        }), ReportShadowsocksStat, tryGetOkResult: true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(EventLogEntryType.FailureAudit, ex.Message);
            }
            finally
            {
                _timer.Change(5000, Timeout.Infinite);
            }
        }
        public void SetTimer(Timer timer)
        {
            this._timer = timer;
        }
        private void ResultHandler(Action onAddedOrRemoveUser = null, Action<string> onReceiveStat = null, bool tryGetOkResult = false)
        {
            TRYAGAIN:
            _socket.Receive(buffer);
            int lastIndex = Array.FindIndex(buffer, b => b == 0) - 1;
            var result = Encoding.ASCII.GetString(buffer, 0, lastIndex + 1);
            Array.Clear(buffer, 0, lastIndex + 1);
            if (result == "pong")
            {
                _logger.Log(EventLogEntryType.SuccessAudit, "Connect to ssserver manage api.");
                return;
            }
            if (result == "ok")
            {
                onAddedOrRemoveUser?.Invoke();
                return;
            }

            if (tryGetOkResult)
            {
                onReceiveStat?.Invoke(result);
                goto TRYAGAIN;
            }
            else
            {
                onReceiveStat?.Invoke(result);
            }
           
        }
    }
}
