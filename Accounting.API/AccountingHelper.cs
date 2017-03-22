using Accounting.API.Models;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API
{
    public static class AccountingHelper
    {
        public static BasicAcct Statistics(IGrouping<string, Acct> item, IEnumerable<Current> connections)
        {
            var input = connections?.FirstOrDefault(c => c.UserName == item.Key)?.TotalBytesIn ?? 0;
            var outpub = connections?.FirstOrDefault(c => c.UserName == item.Key)?.TotalBytesOut ?? 0;
            var inputTraffic = item.Distinct().Sum(c => c.AcctInputOctets) + input;
            var outputTraffic = item.Distinct().Sum(c => c.AcctOutputOctets) + outpub;

            return new BasicAcct
            {
                UserName = item.Key,
                TotalIn = inputTraffic,
                TotalOut = outputTraffic
            };
        }
        public static BasicAcct Statistics(Current item)
        {
            return new BasicAcct
            {
                UserName = item.UserName,
                TotalIn = item.TotalBytesIn,
                TotalOut = item.TotalBytesOut
            };
        }
    }
}
