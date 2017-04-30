using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Accounting.API.Data;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Dapper;
using SharedProject;
using System.Data.Common;

namespace Accounting.API
{
    public class AcctRepoSqlServer : IAcctRepo
    {
        private readonly AccountingContext _context;
        private readonly ADContext _adContext;
        private readonly string _remoteServiceBaseUrl;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        public AcctRepoSqlServer(AccountingContext context, ADContext adContext, IOptionsSnapshot<AppSettings> settings)
        {
            _settings = settings;
            _remoteServiceBaseUrl = $"{settings.Value.IdentityUrl}/api/task";
            _context = context;
            _adContext = adContext;
        }

        public AccountingContext Context => _context;

        public async Task<IEnumerable<Login>> GetLogins(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return await _adContext.Logins.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Login>> GetLogins()
        {
            return await _adContext.Logins.ToListAsync();
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(string usernames, DateTime? beginTime, DateTime? endTime)
        {
            if (String.IsNullOrEmpty(usernames))
            {
                //throw new ArgumentNullException(nameof(usernames));
                usernames = usernames ?? "";
            }
            beginTime = beginTime ?? DateTime.Parse("1753-1-1");
            endTime = endTime ?? DateTime.MaxValue;
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "select totalinput, totaloutput,username from dbo.GetAccountings(@usernames, @begintime, @endtime)";
            command.Parameters.Add(new SqlParameter("@usernames", usernames));
            command.Parameters.Add(new SqlParameter("@begintime", beginTime));
            command.Parameters.Add(new SqlParameter("@endtime", endTime));
            var reader = await command.ExecuteReaderAsync();
            var acctns = AcctN.GetFromReader(reader).ToArray();
            connection.Close();
            return acctns;
        }

        public async Task<IEnumerable<ValueTuple<string, long>>> GetSSAcctNAsync(string usernames, DateTime? beginTime, DateTime? endTime)
        {
            if (String.IsNullOrEmpty(usernames))
            {
                //throw new ArgumentNullException(nameof(usernames));
                usernames = usernames ?? "";
            }
            beginTime = beginTime ?? DateTime.Parse("1753-1-1");
            endTime = endTime ?? DateTime.MaxValue;
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "select totalinputoutput,username from dbo.GetSSAccountings(@usernames, @begintime, @endtime)";
            command.Parameters.Add(new SqlParameter("@usernames", usernames));
            command.Parameters.Add(new SqlParameter("@begintime", beginTime));
            command.Parameters.Add(new SqlParameter("@endtime", endTime));
            var reader = await command.ExecuteReaderAsync();
            var acctns = GetValueTupleFromReader(reader).ToArray();
            connection.Close();
            return acctns;
        }
        public static IEnumerable<ValueTuple<string, long>> GetValueTupleFromReader(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            if (reader.IsClosed)
            {
                throw new InvalidOperationException("DbDataReader is closed.");
            }
            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return new ValueTuple<string, long>(reader.GetString(1), reader.GetInt64(0));
            }
        }
        public async Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime)
        {
            beginTime = beginTime ?? DateTime.Parse("1753-1-1");
            endTime = endTime ?? DateTime.MaxValue;
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "select totalinput, totaloutput,username from dbo.GetAccountingsWithoutUsernames(@begintime, @endtime)";
            command.Parameters.Add(new SqlParameter("@begintime", beginTime));
            command.Parameters.Add(new SqlParameter("@endtime", endTime));
            var reader = await command.ExecuteReaderAsync();
            var acctns = AcctN.GetFromReader(reader).ToArray();
            return acctns;
        }

        public IEnumerable<AcctRaw> GetAcctRaw(string usernames, int pageSize, int pageIndex)
        {
            if (String.IsNullOrEmpty(usernames))
            {
                //throw new ArgumentNullException(nameof(usernames));
                usernames = usernames ?? "";
            }
            var offset = (pageIndex - 1) * pageSize;
            var connection = _context.Database.GetDbConnection();
            connection.Open();

            var jsons = connection.Query<string>($@"select infojson
	                      from dbo.eventraw
                          where JSON_VALUE(InfoJSON, '$.Acct_Status_Type') = 2
                          and JSON_VALUE(InfoJSON, '$.User_Name') in(SELECT value FROM STRING_SPLIT(@usernames, ','))
	                     
                          order by cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as datetime) desc
                          OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
                          ", new { usernames = usernames, offset = offset, pageSize = pageSize });
            connection.Close();

            // and cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as datetime) >= @begintime and cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as datetime) < @endtime
            foreach (var j in jsons)
            {
                yield return JsonConvert.DeserializeObject<AcctRaw>(j);
            }
        }

        public async Task<IEnumerable<UserInfo>> GetUserInfosAsync()
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(_remoteServiceBaseUrl);
            return JsonConvert.DeserializeObject<UserInfo[]>(json);
        }

        public async Task<IEnumerable<string>> GetSSOnlineUsersAsync()
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            var usernames = await connection.QueryAsync<string>("select distinct username from sseventraw where TimeStamp > DATEADD(minute,-5, GETUTCDATE())");
            connection.Close();
            return usernames;
        }
    }
}
