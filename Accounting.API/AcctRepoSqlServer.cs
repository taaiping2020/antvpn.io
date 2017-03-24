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
        public AcctRepoSqlServer(AccountingContext context)
        {
            _context = context;
        }

        public AccountingContext Context => _context;

        //public IEnumerable<Login> GetLogins()
        //{
        //    return _context.Logins.string userId
        //}

        public IEnumerable<Acct> GetAllAcct(IEnumerable<string> userNames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllUserNamesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AcctN>> GetCurrentAcctNAsync()
        {
            var currentMeta = await _context.CurrentMeta.ToListAsync();
            var timestamps = currentMeta.Select(c => c.TimeStamp).ToArray();

            var currents = await _context.Current.Where(c => timestamps.Contains(c.TimeStamp)).ToListAsync();
            var g = currents.GroupBy(c => c.UserName).Select(c => new AcctN(c.Key, c.Sum(i => i.TotalBytesIn), c.Sum(o => o.TotalBytesOut)));
            return g;
        }

        public async Task<DateTimeOffset?> GetLastUpdateAsync(string username)
        {
            var time = await _context.Current.Where(c => c.UserName == username).OrderByDescending(c => c.TimeStamp).Select(c => c.TimeStamp).FirstOrDefaultAsync();
            return time;
        }

        public IEnumerable<Acct> GetLastedAcct(IEnumerable<string> strings)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Acct> GetLastedAcctWithDocs(IEnumerable<Acct> accts)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Acct> GetStartedAcct(IEnumerable<string> userNames)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Acct> GetStoppedAcct(IEnumerable<string> userNames)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Acct> GetStoppedAcctWithDocs(IEnumerable<Acct> accts, DateTime? beginTime, DateTime? endTime)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Acct>> GetStoppedAcctWithDocsByUserNamesAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime)
        {
            return await GetAcctNAsync(null, beginTime, endTime);
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime)
        {
    
                beginTime = beginTime ?? DateTime.Parse("1753-1-1");
                endTime = endTime ?? DateTime.MaxValue;
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "select totalinput, totaloutput,username from dbo.GetAccountings(@begintime, @endtime)";
                command.Parameters.Add(new SqlParameter("@begintime", beginTime));
                command.Parameters.Add(new SqlParameter("@endtime", endTime));
                var reader = await command.ExecuteReaderAsync();
                var acctns = AcctN.GetFromReader(reader).ToArray();
                if (usernames != null)
                {
                    return acctns.Where(c => usernames.Contains(c.UserName)).ToArray();
                }
                return acctns;
        
        }

        public Task<IEnumerable<Current>> GetCurrentAsync()
        {
            throw new NotImplementedException();
        }
    }
}
