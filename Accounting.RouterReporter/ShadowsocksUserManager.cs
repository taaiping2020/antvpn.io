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
    internal class ShadowsocksUserManager : ShadowsocksBase
    {
        public ShadowsocksUserManager(string managerAddress, int port, Action<EventLogEntryType, string> logger, Repo repo, string machineName) : base(managerAddress, port, logger)
        {
            this._getNewLogins = repo.GetLiveUsers;
            this._machineName = machineName;
            this._repo = repo;
        }


        Func<IEnumerable<Login>> _getNewLogins;
        byte[] bytes = new byte[1024];
        string _machineName;
        Repo _repo;
        bool isFirstTime = true;
        LoginComparer _loginComparer = new LoginComparer();
        public void Initialize()
        {
            foreach (var user in _inMemoryUsers)
            {
                _socket.Send(SSCommand.AddUser(user.Port, user.LoginName, user.Password));
                ResultHandler();
            }
        }

        public void ReportShadowsocksStat()
        {
            _socket.Receive(bytes);
            int lastIndex = Array.FindLastIndex(bytes, b => b != 0);
            var result = Encoding.ASCII.GetString(bytes, 0, lastIndex + 1);
            Array.Clear(bytes, 0, lastIndex + 1);

            var stat = ShadowsocksStat.Parse(result);
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
        public void UpdateShadowsocksUsers(object state)
        {
            IEnumerable<Login> logins = _getNewLogins();
            if (logins == null)
            {
                throw new ArgumentNullException(nameof(logins));
            }

            if (_inMemoryUsers.Count() == logins.Count())
            {
                if (!isFirstTime)
                {
                    ReportShadowsocksStat();
                }

                _timer.Change(5000, Timeout.Infinite);
                isFirstTime = false;
                return;
            }
            if (_inMemoryUsers.Count() > logins.Count())
            {
                var usersToRemove = _inMemoryUsers.Except(logins, _loginComparer);
                if (usersToRemove != null && usersToRemove.Any())
                {
                    foreach (var user in usersToRemove.ToArray())
                    {
                        _socket.Send(SSCommand.RemoveUser(user.Port));
                        ResultHandler(new Action(() =>
                        {
                            _inMemoryUsers.Remove(user);
                            _logger(EventLogEntryType.Information, $"Remove {user.LoginName}");
                        }), ReportShadowsocksStat);
                    }
                }
            }
            else
            {
                var usersToAdd = logins.Except(_inMemoryUsers, _loginComparer);
                if (usersToAdd != null && usersToAdd.Any())
                {
                    foreach (var user in usersToAdd.ToArray())
                    {
                        _socket.Send(SSCommand.AddUser(user.Port, user.LoginName, user.Password));
                        ResultHandler(new Action(() =>
                        {
                            _inMemoryUsers.Add(user);
                            _logger(EventLogEntryType.Information, $"Add {user.LoginName}");
                        }), ReportShadowsocksStat);
                    }
                }
            }

            _timer.Change(5000, Timeout.Infinite);
        }
    }
}
