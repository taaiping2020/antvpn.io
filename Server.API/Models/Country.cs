using System.Collections.Generic;

namespace Server.API.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }

        public virtual ICollection<Server> Servers { get; set; }
    }
}