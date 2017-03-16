using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    public class LoginStatus
    {
        public string UserId { get; set; }
        public string LoginName { get; set; }
        public bool AllowDialIn { get; set; }
        public bool Enabled { get; set; }
        public BasicAcct BasicAcct { get; set; }
    }
}
