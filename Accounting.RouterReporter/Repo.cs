using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Windows;
using Dapper;

namespace Accounting.RouterReporter
{
    public class Repo
    {
        private readonly string _connectionString;
        private readonly string _connectionStringDc;
        public Repo(string connectionString, string connectionStringDc)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            if (String.IsNullOrEmpty(connectionStringDc))
            {
                throw new ArgumentNullException(nameof(connectionStringDc));
            }
            this._connectionStringDc = connectionStringDc;
            this._connectionString = connectionString;
        }
        public void InsertDatas(List<RemoteAccessConnection> racs)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Execute(@"insert [dbo].[current](AuthMethod, Bandwidth, ClientExternalAddress, ClientIPv4Address, ConnectionDuration, ConnectionStartTime, ConnectionType, MachineName, TimeStamp, TotalBytesIn, TotalBytesOut, TransitionTechnology, TunnelType, UserActivityState, UserName) values (@AuthMethod, @Bandwidth, @ClientExternalAddress, @ClientIPv4Address, @ConnectionDuration, @ConnectionStartTime, @ConnectionType, @MachineName, @TimeStamp, @TotalBytesIn, @TotalBytesOut, @TransitionTechnology, @TunnelType, @UserActivityState, @UserName)", racs);
            }
        }

        public string[] GetDisconnectUserNames()
        {
            using (var connection = new SqlConnection(this._connectionStringDc))
            {
                return connection.Query<string>(@"select [LoginName] from logins where [AllowDialIn] = 0 or [Enabled] = 0").ToArray();
            }
        }

        public void InsertOrUpdateTimetamp(string machineName, DateTime timestamp)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                var b = connection.Query<int>("select count(*) from [dbo].[currentmeta] where MachineName = @machineName ", new { machineName });
                if (b.FirstOrDefault() == 0)
                {
                    connection.Execute("insert [dbo].[currentmeta](MachineName, TimeStamp) values(@machineName, @timestamp)", new { machineName, timestamp });
                }
                else
                {
                    connection.Execute("update [dbo].[currentmeta] set TimeStamp = @timestamp where MachineName = @machineName", new { machineName, timestamp });
                }
            }
        }
    }
}
