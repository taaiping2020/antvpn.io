using System.Collections.Generic;

namespace Server.API.Models
{
    public class Protocal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ServerProtocal> ServerProtocals { get; set; }
    }
}