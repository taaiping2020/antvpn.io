using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.API.Data;
using Accounting.API.Models;
using SharedProject;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Accounting.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    [Authorize]
    public class LoginController : Controller
    {
        public readonly IAcctRepo _repo;
        private readonly ADContext _adContext;
        private readonly AccountingContext _accountingContext;
        public LoginController(IAcctRepo repo, ADContext adContext, AccountingContext accountingContext)
        {
            _repo = repo;
            _adContext = adContext;
            _accountingContext = accountingContext;
        }

        [HttpGet]
        public IEnumerable<Login> GetLogins()
        {
            var userId = User.FindFirst("sub").Value;
            return _adContext.Logins.Where(c => c.UserId == userId);
        }

        [HttpGet("Status")]
        public async Task<IActionResult> GetLoginStatus(bool asAdministrator = false)
        {
            IEnumerable<Login> logins;
            if (User.FindFirst("role")?.Value == "Administrator" && asAdministrator)
            {
                logins = await _repo.GetLogins();
            }
            else
            {
                var userId = User.FindFirst("sub").Value;
                logins = await _repo.GetLogins(userId);
            }

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var accts = await _repo.GetAcctNAsync(logins.Select(c => c.LoginName).ToString(','), firstDayOfMonth, lastDayOfMonth);
            var acctsss = await _repo.GetSSAcctNAsync(logins.Select(c => c.LoginName).ToString(','), firstDayOfMonth, lastDayOfMonth);
            var onlines = await GetOnlineUserNamesAsync();
            var model = logins.Select(c => new LoginStatus
            {
                AllowDialIn = c.AllowDialIn,
                Enabled = c.Enabled,
                LoginName = c.LoginName,
                UserId = c.UserId,
                MonthlyTraffic = c.MonthlyTraffic,
                SSMonthlyTraffic = acctsss.FirstOrDefault(d => d.Item1 == c.LoginName).Item2,
                IsOnline = onlines.Contains(c.LoginName),
                Port = c.Port,
                BasicAcct = new BasicAcct()
                {
                    TotalIn = accts.FirstOrDefault(d => d.UserName == c.LoginName)?.TotalInput ?? 0,
                    TotalOut = accts.FirstOrDefault(d => d.UserName == c.LoginName)?.TotalOutput ?? 0,
                }
            }).ToArray();
            return Ok(model);
        }

        private async Task<IEnumerable<string>> GetOnlineUserNamesAsync()
        {
            var before = DateTime.UtcNow.AddMinutes(-5);
            var ssonlineuser = await _repo.GetSSOnlineUsersAsync();

            var query = from c in _accountingContext.Current
                        join cm in _accountingContext.CurrentMeta on c.TimeStamp equals cm.TimeStamp
                        where cm.TimeStamp > before
                        select c.UserName;
            var currentUserNames = await query.ToArrayAsync();

            return currentUserNames.Concat(ssonlineuser);
        }

        [HttpGet("History/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> GetHistory(int pageSize, int pageIndex)
        {
            var userId = User.FindFirst("sub").Value;
            var logins = await _repo.GetLogins(userId);
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var acctRaws = _repo.GetAcctRaw(logins.Select(c => c.LoginName).ToString(','), pageSize, pageIndex).ToArray();

            return Ok(acctRaws);
        }

        [HttpGet("Server")]
        public async Task<IActionResult> GetServer()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var servers = await _repo.GetServerAcctNAsync(firstDayOfMonth, lastDayOfMonth);
            return Ok(servers);
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin([FromBody] Login login)
        {
            var userId = User.FindFirst("sub").Value;
            login.UserId = userId;
            login.LoginName = login.LoginName.ToLower();
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest("id can not be null");
            }

            // check db if exist.
            if (await _adContext.Logins.AnyAsync(c => c.LoginName == login.LoginName))
            {
                return BadRequest("the account already exist.");
            }

            _adContext.Logins.Add(login);
            await _adContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Reset")]
        public async Task<IActionResult> PutLogin([FromBody] LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst("sub").Value;

            var login = await _adContext.Logins.FindAsync(model.UserName);
            if (login == null)
            {
                return NotFound();
            }
            login.Password = model.Password;

            await _adContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Configure")]
        public async Task<IActionResult> ConfigureLogin([FromBody] LoginConfigureBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst("sub").Value;

            var login = await _adContext.Logins.FindAsync(model.UserName);
            if (login == null)
            {
                return NotFound();
            }
            login.MonthlyTraffic = model.MonthlyTraffic;

            await _adContext.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginExists(string id)
        {
            return _adContext.Logins.Any(e => e.LoginName == id);
        }
    }
}