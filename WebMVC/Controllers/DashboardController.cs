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
using Extensions;
using WebMVC.Services;
using Microsoft.Extensions.Options;
using System.Linq;
using System;

namespace WebMVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        public DashboardController(ILoginService loginService, IOptionsSnapshot<AppSettings> settings)
        {
            _loginService = loginService;
            _settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identities.GetUserId();

            var logins = await _loginService.GetWithStatusAsync();
            ViewData["logins"] = logins;
            var acctRaws = await _loginService.GetAcctRawAsync();
            ViewData["acctRaws"] = acctRaws.Select(c => new AcctRawViewModel(c)).OrderByDescending(c => c.EventTime);

            var user = User as ClaimsPrincipal;
            var m_traffic = String.IsNullOrEmpty(user.FindFirst("monthly_traffic")?.Value) ? 0d : double.Parse(user.FindFirst("monthly_traffic").Value);
            var used = logins.Sum(c => c.BasicAcct.TotalIn) + logins.Sum(c => c.BasicAcct.TotalOut);

            ViewData["traffic"] = m_traffic;
            ViewData["used"] = used;

            ViewData["traffic_gb"] = m_traffic / 1024d / 1024d / 1024d;
            ViewData["used_gb"] = used / 1024d / 1024d / 1024d;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.Identities.GetUserId();

            var result = await _loginService.CreateNewLoginAsync(model.UserName, model.Password);
            if (!result.IsSuccess)
            {
                ViewData["Message"] = result.ErrorMessage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.Identities.GetUserId();

            var result = await _loginService.ResetPasswordAsync(model);
            if (result == false)
            {
                //
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Configure(LoginConfigureBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.Identities.GetUserId();
            if (model.MonthlyTraffic == 0)
            {
                model.MonthlyTraffic = null;
            }

            var result = await _loginService.SetMonthlyTrafficAsync(model);
            if (result == false)
            {
                //
            }
            NotifyServerUpdateAccountStatus();
            return RedirectToAction(nameof(Index));
        }

        private void NotifyServerUpdateAccountStatus()
        {
            try
            {
                var client = new HttpClient();
                client.GetStringAsync($"{_settings.Value.AccountingUrl}/api/task");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"job fail. message: {ex.Message}");
            }
        }
    }
}
