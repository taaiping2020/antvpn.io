using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebMVC.Extensions
{
    public static class IdentityExtension
    {
        public static string GetUserId(this IEnumerable<ClaimsIdentity> identities)
        {
            if (!identities?.Any() ?? throw new ArgumentNullException())
            {
                return null;
            }
            return identities.First().Claims.First(c => c.Type == "sub").Value;
        }
    }
}
