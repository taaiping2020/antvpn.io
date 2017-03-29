using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.Data;
using Server.API.Models;

namespace Server.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Server")]
    public class ServerController : Controller
    {
        private readonly ServerContext _context;

        public ServerController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Server
        [HttpGet]
        public IEnumerable<Server.API.Models.Server> GetServers()
        {
            return _context.Servers;
        }

        // GET: api/Server/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = await _context.Servers.SingleOrDefaultAsync(m => m.Name == id);

            if (server == null)
            {
                return NotFound();
            }

            return Ok(server);
        }

        // PUT: api/Server/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer([FromRoute] string id, [FromBody] Server.API.Models.Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != server.Name)
            {
                return BadRequest();
            }

            _context.Entry(server).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Server
        [HttpPost]
        public async Task<IActionResult> PostServer([FromBody] Server.API.Models.Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServer", new { id = server.Name }, server);
        }

        // DELETE: api/Server/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = await _context.Servers.SingleOrDefaultAsync(m => m.Name == id);
            if (server == null)
            {
                return NotFound();
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();

            return Ok(server);
        }

        private bool ServerExists(string id)
        {
            return _context.Servers.Any(e => e.Name == id);
        }
    }
}