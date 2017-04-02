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

            var logins = await _loginService.GetWithStatusAsync(userId);
            ViewData["logins"] = logins;
            var user = User as ClaimsPrincipal;
            ViewData["traffic"] = user.FindFirst("monthly_traffic").Value;
            ViewData["used"] = logins.Sum(c => c.BasicAcct.TotalIn) + logins.Sum(c => c.BasicAcct.TotalOut);
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

            var result = await _loginService.CreateNewLoginAsync(userId, model.UserName, model.Password);
            if (result == false)
            {
                //
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

            var result = await _loginService.ResetPasswordAsync(userId, model);
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

            var result = await _loginService.SetMonthlyTrafficAsync(userId, model);
            if (result == false)
            {
                //
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
