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
    public class ServerController : Controller
    {
        private readonly IServerService _serverService;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        public ServerController(IServerService serverService, IOptionsSnapshot<AppSettings> settings)
        {
            _serverService = serverService;
            _settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["servers"] = await _serverService.GetServersAsync();

            return View();
        }
    }
}
