using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Accounting.API.Data;

namespace Accounting.API
{
    public class AcctRepoSqlServer : IAcctRepo
    {
        private readonly AccountingContext _context;
        private readonly ADContext _adContext;
        public AcctRepoSqlServer(AccountingContext context, ADContext adContext)
        {
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
            return  await _adContext.Logins.Where(c => c.UserId == userId).ToListAsync();
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
            return acctns;
        }
    }
}
