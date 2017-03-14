using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Accounting.API.Models;
using Extensions;
using System.Net.Http;
using Newtonsoft.Json;
using Accounting.API.Data;

namespace Accounting.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public readonly LoginContext db;
        public LoginController(LoginContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Backup([FromQuery]string groupname)
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://dc.antvpn.io:5000/api");
            var content = await client.GetStringAsync($"http://dc.antvpn.io:5000/api/Group/Members/{groupname}");
            var aduser = JsonConvert.DeserializeObject<ADUser[]>(content);

            db.Logins.AddRange(aduser.Select(c => LoginFactory.Create(c)));

            await db.SaveChangesAsync();
            //var client = new RestClient("http://dc.antvpn.io:5000");
            //var request = new RestRequest("api/Group/Members/{groupname}", Method.GET);
            //request.AddUrlSegment("groupname", "VPN Group");
            
            return Ok(aduser);
        }

        //[HttpPost]
        //public async Task<IActionResult> Creation()
        //{

        //}
    }
}
