using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API.Models
{
    public class Server
    {
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }

    }
}
