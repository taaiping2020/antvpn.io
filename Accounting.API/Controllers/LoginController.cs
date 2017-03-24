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
        public readonly IAcctRepo _repo;
        public LoginController(IAcctRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{userId}")]
        public IEnumerable<Login> GetLogins(string userId)
        {
            return _repo.Context.Logins.Where(c => c.UserId == userId);
        }

        [HttpGet("Status/{userId}")]
        public async Task<IActionResult> GetLoginStatus(string userId)
        {
            var logins = await _repo.GetLogins(userId);
            var accts = await _repo.GetAcctNAsync(userId, null, null);

            var model = logins.Select(c => new LoginStatus
            {
                AllowDialIn = c.AllowDialIn,
                Enabled = c.Enabled,
                LoginName = c.LoginName,
                UserId = c.UserId,
                BasicAcct = new BasicAcct()
                {
                    TotalIn = accts.FirstOrDefault(d => d.UserName == c.LoginName)?.TotalInput ?? 0,
                    TotalOut = accts.FirstOrDefault(d => d.UserName == c.LoginName)?.TotalOutput ?? 0,
                }
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

            _repo.Context.Logins.Add(login);
            await _repo.Context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Reset/{userId}")]
        public async Task<IActionResult> PutLogin([FromRoute] string userId, [FromBody] LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var login = await _repo.Context.Logins.FindAsync(model.UserName);
            if (login == null)
            {
                return NotFound();
            }
            login.Password = model.Password;

            await _repo.Context.SaveChangesAsync();

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
            return _repo.Context.Logins.Any(e => e.LoginName == id);
        }
    }
}