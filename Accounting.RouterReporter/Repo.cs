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
        public Repo(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            this._connectionString = connectionString;
        }
        public void InsertDatas(List<RemoteAccessConnection> racs)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Execute(@"insert [dbo].[current](AuthMethod, Bandwidth, ClientExternalAddress, ClientIPv4Address, ConnectionDuration, ConnectionStartTime, ConnectionType, MachineName, TimeStamp, TotalBytesIn, TotalBytesOut, TransitionTechnology, TunnelType, UserActivityState, UserName) values (@AuthMethod, @Bandwidth, @ClientExternalAddress, @ClientIPv4Address, @ConnectionDuration, @ConnectionStartTime, @ConnectionType, @MachineName, @TimeStamp, @TotalBytesIn, @TotalBytesOut, @TransitionTechnology, @TunnelType, @UserActivityState, @UserName)", racs);
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
