using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using WebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SharedProject;

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

        public async Task<CreateLoginResult> CreateNewLoginAsync(string loginName, string password)
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
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");

            var response = await _apiClient.PostAsync(loginsUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var message = await response.Content.ReadAsStringAsync();
                return new CreateLoginResult() { IsSuccess = false, ErrorMessage = message };
            }

            return new CreateLoginResult() { IsSuccess = true };
        }

        public async Task<IEnumerable<AcctRaw>> GetAcctRawAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/history/6/1";
            var dataString = await _apiClient.GetStringAsync(loginsUrl);

            var response = JsonConvert.DeserializeObject<AcctRaw[]>(dataString);

            return response;
        }

        public async Task<IEnumerable<LoginStatus>> GetWithStatusAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/status";
            var dataString = await _apiClient.GetStringAsync(loginsUrl);

            var response = JsonConvert.DeserializeObject<LoginStatus[]>(dataString);

            return response;
        }

        public async Task<bool> ResetPasswordAsync(LoginBindingModel model)
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/Reset";

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

            var response = await _apiClient.PostAsync(loginsUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return true;
        }

        public async Task<bool> SetMonthlyTrafficAsync(LoginConfigureBindingModel model)
        {
            var context = _httpContextAccesor.HttpContext;
            var token = await context.Authentication.GetTokenAsync("access_token");

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var loginsUrl = $"{_remoteServiceBaseUrl}/Configure";

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

            var response = await _apiClient.PostAsync(loginsUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            return true;
        }
    }
}
