using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.WebMVC.Services;
using Microsoft.AspNetCore.Http.Authentication;
using WebMVC.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using WebMVC.Extensions;
using Newtonsoft.Json;
using WebMVC.Services;
using Microsoft.Extensions.Options;
using System.Linq;
using System;

namespace WebMVC.Controllers
{
    [Authorize()]
    public class ManageController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IServerService _serverService;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        public ManageController(IServerService serverService, ILoginService loginService, IOptionsSnapshot<AppSettings> settings)
        {
            _serverService = serverService;
            _loginService = loginService;
            _settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identities.GetUserId();
            var user = User as ClaimsPrincipal;
            if (user.FindFirst("role")?.Value != "Administrator")
            {
                return StatusCode(403);
            }
            var serversTask = _serverService.GetServersAsync();
            var acctSTask = _loginService.GetAcctServerAsync();
            var loginsTask = _loginService.GetWithStatusAsync(true);
            var servers = await serversTask;
            var acctS = await acctSTask;
            var logins = await loginsTask;
            
            foreach (var s in servers)
            {
                if (!s.IsHybrid)
                {
                    s.TrafficIn = acctS.FirstOrDefault(c => c.MachineName.ToLower() == s.Name.ToLower())?.TotalInput;
                    s.TrafficOut = acctS.FirstOrDefault(c => c.MachineName.ToLower() == s.Name.ToLower())?.TotalOutput;
                }
                else
                {
                    s.TrafficIn = acctS.FirstOrDefault(c => c.MachineName.ToLower() == s.TrafficServerName?.ToLower())?.TotalInput;
                    s.TrafficOut = acctS.FirstOrDefault(c => c.MachineName.ToLower() == s.TrafficServerName?.ToLower())?.TotalOutput;
                }
               
            }
            ViewData["servers"] = servers;
            ViewData["logins"] = logins.OrderByDescending(c => c.IsOnline).ThenByDescending(c => c.BasicAcct.TotalInOut);
            return View();
        }
    }
}
