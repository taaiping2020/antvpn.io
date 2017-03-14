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
using Extensions.Windows;

namespace Login.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        static readonly string ModuleFileNameOrigin = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "UserManager.psm1");
        static readonly string ModuleFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "UserManagerLocalUser.psm1");
        static readonly PowerShell ps;
        static readonly Runspace runspace;
        static readonly InitialSessionState initial;
        static UsersController()
        {
            var text = System.IO.File.ReadAllText(ModuleFileNameOrigin);
            System.IO.File.WriteAllText(ModuleFileName, text);

            initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { ModuleFileName });
            runspace = RunspaceFactory.CreateRunspace(initial);
            ps = PowerShell.Create(initial);
            ps.Runspace = runspace;
            runspace.Open();
        }
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
            ps.Commands.Clear();
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
            try
            {
                ps.Commands.Clear();
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
            catch (Exception ex)
            {
                return Ok(ex);
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

            ps.Commands.Clear();
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

            ps.Commands.Clear();
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

            ps.Commands.Clear();
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

            return Ok();
        }
    }
}
