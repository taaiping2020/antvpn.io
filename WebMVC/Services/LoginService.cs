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

namespace WebMVC.Services
{
    public class LoginService : ILoginService
    {
        private HttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public LoginService(IOptionsSnapshot<AppSettings> settings, IHttpContextAccessor httpContextAccesor)
        {
            _remoteServiceBaseUrl = $"{settings.Value.AccountingUrl}/api/login";
            _settings = settings;
            _httpContextAccesor = httpContextAccesor;
        }

        public async Task CreateNewLoginAsync(string userId, string loginName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LoginStatus>> GetWithStatusAsync(string userId)
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/status/{userId}";
            var dataString = await _apiClient.GetStringAsync(loginsUrl);

            var response = JsonConvert.DeserializeObject<LoginStatus[]>(dataString);

            return response;
        }

        public async Task ResetPasswordAsync(string userId, string loginName)
        {
            throw new NotImplementedException();
        }
    }
}
