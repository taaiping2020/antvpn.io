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

        Task<IEnumerable<AcctN>> GetAcctNAsync(string userId, DateTime? beginTime, DateTime? endTime);
        Task<IEnumerable<AcctN>> GetAcctNAsync(DateTime? beginTime, DateTime? endTime);
        Task<IEnumerable<Login>> GetLogins(string userId);
        Task<IEnumerable<UserInfo>> GetUserInfosAsync();
    }
}