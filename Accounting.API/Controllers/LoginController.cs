using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.API.Data;
using Accounting.API.Models;
using Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Accounting.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private readonly LoginContext _context;
        static IAcctRepo repo = new AcctRepoSqlServer();
        public LoginController(LoginContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IEnumerable<Login> GetLogins(string userId)
        {
            return _context.Logins.Where(c => c.UserId == userId);
        }

        [HttpGet("Status/{userId}")]
        public async Task<IActionResult> GetLoginStatus(string userId)
        {
            var logins = await _context.Logins.Where(c => c.UserId == userId).ToListAsync();
            var loginNames = logins.Select(c => c.LoginName).ToArray();
            var currentConnections = await repo.GetCurrentAsync();
            var accts = await repo.GetStoppedAcctWithDocsByUserNamesAsync(loginNames, null, null);

            var group = accts.GroupBy(c => c.UserName);
            var basicAccts = new List<BasicAcct>();
            foreach (var item in group)
            {
                basicAccts.Add(AccountingHelper.Statistics(item, currentConnections));
            }
            if (currentConnections != null)
            {
                foreach (var item in currentConnections.Where(c => !accts.Any(d => d.UserName.ToLower() == c.UserName.ToLower())))
                {
                    basicAccts.Add(AccountingHelper.Statistics(item));
                }
            }

            var model = logins.Select(c => new LoginStatus
            {
                AllowDialIn = c.AllowDialIn,
                Enabled = c.Enabled,
                LoginName = c.LoginName,
                UserId = c.UserId,
                LastUpdated = group.FirstOrDefault(k => k.Key == c.LoginName)?.Max(t => t.EventTimestampUtc),
                BasicAcct = basicAccts.FirstOrDefault(u => u.UserName == c.LoginName)
            });
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Reset/{userId}")]
        public async Task<IActionResult> PutLogin([FromRoute] string userId, [FromBody] LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var login = await _context.Logins.FindAsync(model.UserName);
            if (login == null)
            {
                return NotFound();
            }
            login.Password = model.Password;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLogin([FromRoute] string id, [FromBody] Login login)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != login.LoginName)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(login).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LoginExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetLogin([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var login = await _context.Logins.SingleOrDefaultAsync(m => m.LoginName == id);

        //    if (login == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(login);
        //}




        //// DELETE: api/Login/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLogin([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var login = await _context.Logins.SingleOrDefaultAsync(m => m.LoginName == id);
        //    if (login == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Logins.Remove(login);
        //    await _context.SaveChangesAsync();

        //    return Ok(login);
        //}

        private bool LoginExists(string id)
        {
            return _context.Logins.Any(e => e.LoginName == id);
        }
    }
}