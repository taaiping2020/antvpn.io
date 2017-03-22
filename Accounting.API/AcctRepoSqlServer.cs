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
                var username = usernames.First();
                var sql = @"select *
                        from eventraw
                        where JSON_VALUE(InfoJSON, '$.Acct_Status_Type') = 2 and JSON_VALUE(InfoJSON, '$.User_Name') = @username";

                var result = await db.Eventraw.FromSql(sql, new SqlParameter("@username", username)).ToListAsync();

                List<Acct> accts = new List<Acct>();
                foreach (var item in result)
                {
                    var acct = JsonConvert.DeserializeObject<Acct>(result.First().InfoJson);
                    accts.Add(acct);
                }


                return accts;
            }
        }
    }
}
