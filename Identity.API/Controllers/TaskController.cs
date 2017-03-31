using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Identity.API.Models;
using Identity.API.Data;

namespace Identity.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Task")]
    public class TaskController : Controller
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _db;
        public TaskController(
           ILoggerFactory loggerFactory,
           ApplicationDbContext db)
        {
            _logger = loggerFactory.CreateLogger<TaskController>();
            _db = db;
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var infos = await _db.Users.Select(c => new { userId = c.Id, monthlyTraffic = c.MonthlyTraffic }).ToListAsync();
            return Ok(infos);
        }
    }
}
