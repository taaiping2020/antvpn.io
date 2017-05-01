using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;

namespace SharedProject
{
    public class AcctS
    {
        public AcctS(DbDataReader reader)
        {
            this.TotalInput = reader.GetInt64(0);
            this.TotalOutput = reader.GetInt64(1);
            this.MachineName = reader.GetString(2);
        }
        public AcctS(string machineName, long totalInput, long totalOutput)
        {
            this.TotalInput = totalInput;
            this.TotalOutput = totalOutput;
            this.MachineName = machineName;
        }
        public AcctS()
        {

        }
        public long TotalInput { get; set; }
        public long TotalOutput { get; set; }
        public string MachineName { get; set; }

        private static string ToMegaByte(double inputTraffic)
        {
            return (inputTraffic / 1024d / 1024d).ToString("0.00") + " MB";
        }

        public static AcctS operator +(AcctS left, AcctS right)
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
            if (left.MachineName != right.MachineName)
            {
                throw new InvalidOperationException();
            }
            return new AcctS(left.MachineName, left.TotalInput + right.TotalInput, left.TotalOutput + right.TotalOutput);
        }

        public static IEnumerable<AcctS> GetFromReader(DbDataReader reader)
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
                yield return new AcctS(reader);
            }
        }
    }
}
