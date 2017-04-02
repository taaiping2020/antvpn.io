using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API.ViewModels
{
    public class ServerViewModels
    {
        public ServerViewModels(Models.Server server)
        {
            this.Id = server.Id;
            this.Name = server.Name;
            this.IPv4 = server.IPv4;
            this.Domain = server.Domain;
            this.UserName = server.UserName;
            this.Description = server.Description;

            this.IsHybrid = server.IsHybrid;
            this.IsPublic = server.IsPublic;
            this.Protocals = server.ServerProtocals?.Select(c => c.ProtocalId).ToArray();
            this.Off = server.Off;

            if (server.IsHybrid)
            {
                this.RedirectorServerCountryName = server.RedirectorServer?.Country?.Name;
                this.RedirectorServerCountryFlag = server.RedirectorServer?.Country?.Flag;

                this.TrafficServerCountryName = server.TrafficServer?.Country?.Name;
                this.TrafficServerCountryFlag = server.TrafficServer?.Country?.Flag;
            }
            else
            {
                this.CountryName = server.Country?.Name;
                this.CountryFlag = server.Country?.Flag;
            }
           
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }

        public string RedirectorServerCountryName { get; set; }
        public string RedirectorServerCountryFlag { get; set; }

        public string TrafficServerCountryName { get; set; }
        public string TrafficServerCountryFlag { get; set; }
        public bool Off { get; set; }
        public bool IsPublic { get; set; }
        public bool IsHybrid { get; set; }
        public int[] Protocals { get; set; }
    }
}
