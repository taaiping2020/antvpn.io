using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public class LoginComparer : IEqualityComparer<Login>
    {
        public bool Equals(Login x, Login y)
        {
            return x.LoginName == y.LoginName && x.Password == y.Password && x.Port == y.Port;
        }

        public int GetHashCode(Login obj)
        {
            return $"{obj.LoginName}{obj.Password}{obj.Port}".GetHashCode();
        }
    }
}
