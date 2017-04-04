using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Windows
{
    public class ServerService : IServerService
    {
        private HttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;

        public ServerService()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["ServerUrl"];
            _remoteServiceBaseUrl = $"{url}/api/server";
        }

        public async Task<IEnumerable<ServerListItemViewModel>> GetServersAsync(string token)
        {
            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var serverUrl = $"{_remoteServiceBaseUrl}";
            var dataString = await _apiClient.GetStringAsync(serverUrl);
            var response = JsonConvert.DeserializeObject<ServerListItemViewModel[]>(dataString);
            return response;
        }
    }
}
