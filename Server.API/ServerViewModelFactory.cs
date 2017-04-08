using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedProject;
using Newtonsoft.Json;

namespace Server.API
{
    public static class ServerViewModelFactory
    {
        public static ServerViewModel Create(Models.Server server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }
            ServerViewModel sv = new ServerViewModel()
            {
                Id = server.Id,
                Name = server.Name,
                IPv4 = server.IPv4,
                Domain = server.Domain,
                UserName = server.UserName,
                Description = server.Description,
                IsHybrid = server.IsHybrid,
                IsPublic = server.IsPublic,
                Protocals = server.ServerProtocals?.Select(c => c.ProtocalId).ToArray(),
                Off = server.Off
            };
            if (server.IsHybrid)
            {
                sv.RedirectorServerCountryName = server.RedirectorServer?.Country?.Name;
                sv.RedirectorServerCountryFlag = server.RedirectorServer?.Country?.Flag;

                sv.TrafficServerCountryName = server.TrafficServer?.Country?.Name;
                sv.TrafficServerCountryFlag = server.TrafficServer?.Country?.Flag;

                if (!String.IsNullOrEmpty(server.TrafficServer?.HealthReportJson))
                {
                    sv.HealthReport = JsonConvert.DeserializeObject<HealthReport>(server.TrafficServer.HealthReportJson);
                }
            }
            else
            {
                sv.CountryName = server.Country?.Name;
                sv.CountryFlag = server.Country?.Flag;

                if (!String.IsNullOrEmpty(server.HealthReportJson))
                {
                    sv.HealthReport = JsonConvert.DeserializeObject<HealthReport>(server.HealthReportJson);
                }
            }

            return sv;
        }
    }
}
