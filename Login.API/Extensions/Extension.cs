using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.API.Controllers
{
    public static class Extension
    {
        public static bool IsNullOrCountEqualsZero<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }
    }
}
