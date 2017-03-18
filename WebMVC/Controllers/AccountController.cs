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

namespace WebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult SignIn(string returnUrl)
        {
            var user = User as ClaimsPrincipal;
            
            //TODO - Not retrieving AccessToken yet
            var token = user.FindFirst("access_token");
            if (token != null)
            {
                ViewData["access_token"] = token.Value;
            }

            // "Catalog" because UrlHelper doesn't support nameof() for controllers
            // https://github.com/aspnet/Mvc/issues/5853
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Signout()
        {
            HttpContext.Authentication.SignOutAsync("Cookies");
            HttpContext.Authentication.SignOutAsync("oidc");

            // "Catalog" because UrlHelper doesn't support nameof() for controllers
            // https://github.com/aspnet/Mvc/issues/5853
            var homeUrl = Url.Action(nameof(HomeController.Index), "Home");
            return new SignOutResult("oidc", new AuthenticationProperties { RedirectUri = homeUrl });
        }

        public async Task<IActionResult> MyLogins()
        {
            var userId = User.Identities.GetUserId();

            ViewData["logins"] = await _loginService.GetWithStatusAsync(userId);

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(MyLogins));
            }

            var userId = User.Identities.GetUserId();

            var result = await _loginService.CreateNewLoginAsync(userId, model.UserName, model.Password);
            if (result == false)
            {
                //
            }
            return RedirectToAction(nameof(MyLogins));
        }
    }
}
