using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Windows
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
