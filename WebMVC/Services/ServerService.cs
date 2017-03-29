using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebMVC.Services
{
    [Authorize]
    public class ServerService : IServerService
    {
        private HttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public ServerService(IOptionsSnapshot<AppSettings> settings, IHttpContextAccessor httpContextAccesor)
        {
            _remoteServiceBaseUrl = $"{settings.Value.ServerUrl}/api/server";
            _settings = settings;
            _httpContextAccesor = httpContextAccesor;
        }

        public async Task<IEnumerable<Server>> GetServersAsync()
        {
            _apiClient = new HttpClient();

            var serverUrl = $"{_remoteServiceBaseUrl}";
            var dataString = await _apiClient.GetStringAsync(serverUrl);

            var response = JsonConvert.DeserializeObject<Server[]>(dataString);
            return response;
        }
    }
}
