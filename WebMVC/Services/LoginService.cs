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

        public async Task<bool> CreateNewLoginAsync(string userId, string loginName, string password)
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}";

            Login login = new Login()
            {
                AllowDialIn = true,
                Enabled = true,
                GroupName = "VPN Group",
                LoginName = loginName,
                NormalizedLoginName = loginName.ToLower(),
                Password = password,
                UserId = userId
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");

            var response = await _apiClient.PostAsync(loginsUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return false;

            return true;
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

        public async Task<bool> ResetPasswordAsync(string userId, LoginBindingModel model)
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/Reset/{userId}";

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

            var response = await _apiClient.PostAsync(loginsUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return true;
        }
    }
}
