using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Xml;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace Login.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        readonly string ModuleFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "UserManager.psm1");
        /// <summary>
        /// Get single user.
        /// </summary>
        /// <remarks>
        /// Note that the name must supply.
        /// </remarks>
        /// <param name="name">user's name</param>
        /// <returns>ADUser</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If can't find the user.</response>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ADUserInfo), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Get(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"Get-ADUser -Identity {name} -Properties *");
                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    return NotFound();
                }

                PSObject pso = psos.First();
                var ui = new ADUserInfo(pso);
                return Ok(ui);
            }
        }

        /// <summary>
        /// get adusers by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("Filter/{filter}")]
        [ProducesResponseType(typeof(ADUserInfo[]), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult GetByFilter(string filter)
        {
            if (String.IsNullOrEmpty(filter))
            {
                return BadRequest();
            }

            using (var ps = PowerShell.Create())
            {
                ps.AddCommand($@"Get-ADUser");
                ps.AddParameter("Filter", filter);
                ps.AddParameter("Properties", "*");
                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    return NotFound();
                }

                var uis = psos.Select(c => new ADUserInfo(c));
                return Ok(uis);
            }
        }

        /// <summary>
        /// create aduser
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Post([FromBody]ADUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { ModuleFileName });
            using (var runspace = RunspaceFactory.CreateRunspace(initial))
            using (var ps = PowerShell.Create())
            {
                ps.Runspace = runspace;
                runspace.Open();

                ps.AddCommand("New-User");
                ps.AddParameters(new Dictionary<string, object>
                {
                    ["username"] = model.Name,
                    ["password"] = model.Password,
                    ["allowDialIn"] = model.AllowDialIn ?? false,
                    ["enabled"] = model.Enabled ?? true,
                });
                if (!String.IsNullOrEmpty(model.GroupName))
                {
                    ps.AddParameter("groupname", model.GroupName);
                }
                try
                {
                    ps.Invoke();
                }
                catch (CmdletInvocationException ex)
                {
                    return BadRequest(ex.Message);
                }

                runspace.Close();
            }
            return Ok();
        }

        /// <summary>
        /// set aduser properties
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Put([FromBody]ADUser model)
        {
            ModelState.Remove(nameof(model.Password));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { ModuleFileName });
            using (var runspace = RunspaceFactory.CreateRunspace(initial))
            using (var ps = PowerShell.Create())
            {
                ps.Runspace = runspace;
                runspace.Open();

                ps.AddCommand("Set-User");
                ps.AddParameter("username", model.Name);
                if (!String.IsNullOrEmpty(model.GroupName))
                {
                    ps.AddParameter("groupname", model.GroupName);
                }
                if (!String.IsNullOrEmpty(model.Password))
                {
                    ps.AddParameter("password", model.Password);
                }
                if (model.Enabled != null)
                {
                    ps.AddParameter("enabled", model.Enabled);
                }
                if (model.AllowDialIn != null)
                {
                    ps.AddParameter("allowDialIn", model.AllowDialIn);
                }
                try
                {
                    ps.Invoke();
                }
                catch (CmdletInvocationException ex)
                {
                    return BadRequest(ex.Message);
                }

                runspace.Close();
            }
            return Ok();
        }

        /// <summary>
        /// remove aduser
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult Delete(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Remove-ADUser");
                ps.AddParameter("Identity", name);
                ps.AddParameter("Confirm", false);
                try
                {
                    ps.Invoke();
                }
                catch (CmdletInvocationException)
                {
                    return NotFound();
                }
            }
            return Ok();
        }
    }
}
