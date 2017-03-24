using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.API.Models;
using MongoDB.Bson;
using Accounting.API.Data;

namespace Accounting.API
{
    public interface IAcctRepo
    {
        AccountingContext Context { get; }
        //IEnumerable<BsonDocument> Get(string userName, bool getStart, bool getStop);
        //IEnumerable<BsonDocument> GetAll();
        IEnumerable<Acct> GetAllAcct(IEnumerable<string> userNames);
        Task<IEnumerable<string>> GetAllUserNamesAsync();
        Task<IEnumerable<Current>> GetCurrentAsync();
        IEnumerable<Acct> GetLastedAcct(IEnumerable<string> strings);
        IEnumerable<Acct> GetLastedAcctWithDocs(IEnumerable<Acct> accts);
        IEnumerable<Acct> GetStartedAcct(IEnumerable<string> userNames);
        IEnumerable<Acct> GetStoppedAcct(IEnumerable<string> userNames);
        IEnumerable<Acct> GetStoppedAcctWithDocs(IEnumerable<Acct> accts, DateTime? beginTime, DateTime? endTime);
        Task<IEnumerable<Acct>> GetStoppedAcctWithDocsByUserNamesAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime);
        Task<IEnumerable<AcctN>> GetCurrentAcctNAsync();
        Task<IEnumerable<AcctN>> GetAcctNAsync(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime);
        Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime);
        Task<DateTimeOffset?> GetLastUpdateAsync(string username);
        //Acct ToAcct(BsonDocument c);
    }
}