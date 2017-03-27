using Accounting.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API
{
    public static class LoginFactory
    {
        public static Login Create(ADUser user)
        {
            return new Login
            {
                AllowDialIn = user.AllowDialIn ?? false,
                Enabled = user.Enabled ?? false,
                GroupName = user.GroupName,
                LoginName = user.Name
            };
        }
    }
}
