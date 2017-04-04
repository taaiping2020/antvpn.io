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
using System.Text;
using static Extensions.Extension;
using Microsoft.Extensions.Logging;

namespace Accounting.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Task")]
    public class TaskController : Controller
    {
        public readonly IAcctRepo _repo;
        private readonly ADContext _adContext;
        public readonly ILogger _log;
        public TaskController(IAcctRepo repo, ADContext adContext, ILoggerFactory logger)
        {
            _repo = repo;
            _adContext = adContext;
            _log = logger.CreateLogger<TaskController>();
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var accts = await _repo.GetAcctNAsync(firstDayOfMonth, lastDayOfMonth);
            if (accts == null)
            {
                throw new Exception("not accts found");
            }
            var logins = await _adContext.Logins.ToListAsync();
            if (logins == null)
            {
                throw new Exception("not logins found");
            }
            var userInfos = await _repo.GetUserInfosAsync();
            if (userInfos == null)
            {
                throw new Exception("not userInfos found");
            }
            foreach (var u in userInfos)
            {
                var temp_logins = logins.Where(c => c.UserId == u.UserId);
                if (temp_logins.IsNullOrCountEqualsZero())
                {
                    //TODO log user do not have any logins
                    Console.WriteLine("log user do not have any logins");
                    continue;
                }
                var temp_accts = accts.Where(c => temp_logins.Any(l => l.LoginName == c.UserName));
                if (temp_accts.IsNullOrCountEqualsZero())
                {
                    //TODO user do not connection single time.
                    Console.WriteLine("user do not connection single time.");
                    continue;
                }
                var totalIn = temp_accts.Sum(c => c.TotalInput);
                var totalOut = temp_accts.Sum(c => c.TotalOutput);
                var totalTraffic = totalIn + totalOut;
                if (totalTraffic > u.MonthlyTraffic)
                {
                    Console.WriteLine($"{totalTraffic} > {u.MonthlyTraffic} = {totalTraffic > u.MonthlyTraffic}");
                    // send command disable all user's logins
                    var lgs = _adContext.Logins.Where(c => c.UserId == u.UserId);
                    foreach (var l in lgs)
                    {
                        l.Enabled = false;
                    }
                    // log it
                }
                else
                {
                    Console.WriteLine($"{totalTraffic} > {u.MonthlyTraffic} = {totalTraffic > u.MonthlyTraffic}");
                    // send command enable all user' logins
                    var lgs = _adContext.Logins.Where(c => c.UserId == u.UserId);
                    foreach (var l in lgs)
                    {
                        l.Enabled = true;
                    }
                    // log it
                }
            }

            foreach (var a in accts)
            {
                var totalTraffic = a.TotalInput + a.TotalOutput;
                //TODO when login can configure max traffic then allow dial in set to false, else enable it.
                var login = logins.FirstOrDefault(c => c.LoginName == a.UserName);
                if (login?.MonthlyTraffic == null)
                {
                    login.AllowDialIn = true;
                }
                else if (totalTraffic > login.MonthlyTraffic)
                {
                    login.AllowDialIn = false;
                }
                else
                {
                    login.AllowDialIn = true;
                }
            }

            await _adContext.SaveChangesAsync();
            return Ok();
        }
    }
}