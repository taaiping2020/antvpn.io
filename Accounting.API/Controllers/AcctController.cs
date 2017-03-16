using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Accounting.API.Models;
using Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Accounting.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class AcctController : Controller
    {
        static AcctRepo repo = new AcctRepo();
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentConnections = await repo.GetCurrentAsync();
            var usernames = await repo.GetAllUserNamesAsync();
            var list4 = await repo.GetStoppedAcctWithDocsByUserNamesAsync(usernames, null, null);
            var g1 = list4.GroupBy(c => c.UserName);
            var result = new List<BasicAcct>();
            foreach (var item in g1)
            {
                result.Add(AccountingHelper.Statistics(item, currentConnections));
            }
            foreach (var item in currentConnections.Where(c => !list4.Any(d => d.UserName.ToLower() == c.UserName.ToLower())))
            {
                result.Add(AccountingHelper.Statistics(item));
            }
            return Ok(result.OrderBy(c => c.UserName));
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

       

       
    }
}
