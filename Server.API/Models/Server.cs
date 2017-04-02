using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public virtual Country Country { get; set; }
        public bool IsPublic { get; set; }
        public bool IsHybrid { get; set; }
        public bool Off { get; set; }
        public int? RedirectorServerId { get; set; }
        public virtual Server RedirectorServer { get; set; }
        public int? TrafficServerId { get; set; }
        public virtual Server TrafficServer { get; set; }
        public virtual ICollection<ServerProtocal> ServerProtocals { get; set; }
    }
}
