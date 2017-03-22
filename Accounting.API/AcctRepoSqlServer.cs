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

        public async Task<IEnumerable<Current>> GetCurrentAsync()
        {
            return null;

                var current = await _context.Current.GroupBy(c => c.TimeStamp)?.OrderByDescending(c => c.Key)?.FirstOrDefaultAsync();

                if (current == null)
                {
                    return null;
                }

                if (current.Any())
                {
                    return current;
                }

                return null;
        
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
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "select totalinput, totaloutput,username from dbo.GetAccountings(@begintime, @endtime)";
                command.Parameters.Add(new SqlParameter("@begintime", DateTime.Parse("1753-1-1")));
                command.Parameters.Add(new SqlParameter("@endtime", DateTime.MaxValue));
                var reader = await command.ExecuteReaderAsync();

                var acctns = AcctN.GetFromReader(reader).ToArray();
                

                return null;
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime)
        {
            return await GetAcctNAsync(null, beginTime, endTime);
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime)
        {
    
                beginTime = beginTime ?? DateTime.Parse("1753-1-1");
                endTime = beginTime ?? DateTime.MaxValue;
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
    }
}
