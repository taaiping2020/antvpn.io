using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;

namespace Accounting.API.Models
{
    public class AcctN
    {
        public AcctN(DbDataReader reader)
        {
            this.TotalInput = reader.GetInt64(0);
            this.TotalOutput = reader.GetInt64(1);
            this.UserName = reader.GetString(2);
        }
        public long TotalInput { get; set; }
        public long TotalOutput { get; set; }
        public string UserName { get; set; }

        public static IEnumerable<AcctN> GetFromReader(DbDataReader reader)
        {
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
                yield return new AcctN(reader);
            }
        }
    }
}
