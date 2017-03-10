﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.WebMVC.Services;
using Microsoft.AspNetCore.Http.Authentication;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private readonly IIdentityParser<ApplicationUser> _identityParser;
        //public AccountController(IIdentityParser<ApplicationUser> identityParser)
        //{
        //    _identityParser = identityParser;
        //}

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
    }
}
