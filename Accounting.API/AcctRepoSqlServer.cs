using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Accounting.API
{
    public class AcctRepoSqlServer : IAcctRepo
    {
        public AcctRepoSqlServer()
        {

        }

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
            using (var db = new AAAContext())
            {
                var current = await db.Current.GroupBy(c => c.TimeStamp)?.OrderByDescending(c => c.Key)?.FirstOrDefaultAsync();

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
            using (var db = new AAAContext())
            {
                //var username = usernames.First();
                //var sql = @"select *
                //        from eventraw
                //        where JSON_VALUE(InfoJSON, '$.Acct_Status_Type') = 2 and JSON_VALUE(InfoJSON, '$.User_Name') = @username";
                var connection = db.Database.GetDbConnection();
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "select totalinput, totaloutput,username from dbo.GetAccountings(@begintime, @endtime)";
                command.Parameters.Add(new SqlParameter("@begintime", DateTime.Parse("1753-1-1")));
                command.Parameters.Add(new SqlParameter("@endtime", DateTime.MaxValue));
                var reader = await command.ExecuteReaderAsync();

                var acctns = AcctN.GetFromReader(reader).ToArray();
                

                return null;
            }
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime)
        {
            return await GetAcctNAsync(null, beginTime, endTime);
        }

        public async Task<IEnumerable<AcctN>> GetAcctNAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime)
        {
            using (var db = new AAAContext())
            {
                beginTime = beginTime ?? DateTime.Parse("1753-1-1");
                endTime = beginTime ?? DateTime.MaxValue;
                var connection = db.Database.GetDbConnection();
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
}
