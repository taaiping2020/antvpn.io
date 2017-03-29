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

namespace WebMVC.Controllers
{
    [Authorize]
    public class ServerController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        public ServerController(ILoginService loginService, IOptionsSnapshot<AppSettings> settings)
        {
            _loginService = loginService;
            _settings = settings;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
