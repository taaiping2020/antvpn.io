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
        public readonly long TotalInput;
        public readonly long TotalOutput;
        public readonly string UserName;

        public string TotalInDisplay => ToMegaByte(TotalInput);
        public string TotalOutDisplay => ToMegaByte(TotalOutput);
        public string TotalInOutDisplay => ToMegaByte(TotalOutput + TotalInput);

        private static string ToMegaByte(double inputTraffic)
        {
            return (inputTraffic / 1024d / 1024d).ToString("0.00") + " MB";
        }

        public static IEnumerable<AcctN> GetFromReader(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
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
