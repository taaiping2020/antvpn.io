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
        public AcctN(string userName, long totalInput, long totalOutput)
        {
            this.TotalInput = totalInput;
            this.TotalOutput = totalOutput;
            this.UserName = userName;
        }

        public readonly long TotalInput;
        public readonly long TotalOutput;
        public readonly string UserName;

        private static string ToMegaByte(double inputTraffic)
        {
            return (inputTraffic / 1024d / 1024d).ToString("0.00") + " MB";
        }

        public static AcctN operator +(AcctN left, AcctN right)
        {
            if (left == null && right == null)
            {
                return null;
            }
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }
            if (left.UserName != right.UserName)
            {
                throw new InvalidOperationException();
            }
            return new AcctN(left.UserName, left.TotalInput + right.TotalInput, left.TotalOutput + right.TotalOutput);
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
