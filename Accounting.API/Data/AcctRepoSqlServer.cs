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
using Extensions;
using Dapper;

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

        public IEnumerable<AcctRaw> GetAcctRaw(string usernames, DateTime? beginTime, DateTime? endTime)
        {
            if (String.IsNullOrEmpty(usernames))
            {
                //throw new ArgumentNullException(nameof(usernames));
                usernames = usernames ?? "";
            }
            beginTime = beginTime ?? DateTime.Parse("1753-1-1");
            endTime = endTime ?? DateTime.MaxValue;
            var connection = _context.Database.GetDbConnection();
            connection.Open();

            var jsons = connection.Query<string>(@"select top 6 infojson
	                      from dbo.eventraw
                          where JSON_VALUE(InfoJSON, '$.Acct_Status_Type') = 2
                          and JSON_VALUE(InfoJSON, '$.User_Name') in(SELECT value FROM STRING_SPLIT(@usernames, ','))
	                      and cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as date) >= @begintime and cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as date) < @endtime
                          order by cast(JSON_VALUE(InfoJSON, '$.Event_Timestamp') as date) desc", new { usernames = usernames, begintime = beginTime, endtime = endTime });
            connection.Close();
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
    }
}
