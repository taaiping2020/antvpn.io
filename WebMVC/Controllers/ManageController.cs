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
            ViewData["servers"] = await _serverService.GetServersAsync();

            var logins = await _loginService.GetWithStatusAsync(true);
            ViewData["logins"] = logins.OrderByDescending(c => c.IsOnline).ThenByDescending(c => c.BasicAcct.TotalInOut);
            //var logins = await _loginService.GetWithStatusAsync();
            //ViewData["logins"] = logins;
            //var acctRaws = await _loginService.GetAcctRawAsync();
            //ViewData["acctRaws"] = acctRaws.Select(c => new AcctRawViewModel(c)).OrderByDescending(c => c.EventTime);

            //var user = User as ClaimsPrincipal;
            //var m_traffic = String.IsNullOrEmpty(user.FindFirst("monthly_traffic")?.Value) ? 0d : double.Parse(user.FindFirst("monthly_traffic").Value);
            //var used = logins.Sum(c => c.BasicAcct.TotalIn) + logins.Sum(c => c.BasicAcct.TotalOut);

            //ViewData["traffic"] = m_traffic;
            //ViewData["used"] = used;

            //ViewData["traffic_gb"] = m_traffic / 1024d / 1024d / 1024d;
            //ViewData["used_gb"] = used / 1024d / 1024d / 1024d;
            return View();
        }
    }
}
