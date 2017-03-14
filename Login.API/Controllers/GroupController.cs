using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using Extensions.Windows;
using System.Management.Automation.Runspaces;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Login.API.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Login.API.Controllers
{
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        static readonly string ModuleFileNameOrigin = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "UserManager.psm1");
        static readonly string ModuleFileName = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "UserManagerLocalGroup.psm1");
        static readonly PowerShell ps;
        static readonly Runspace runspace;
        static readonly InitialSessionState initial;
        static GroupController()
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
        /// get adusers by groupname
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [HttpGet("Members/{groupName}")]
        [ProducesResponseType(typeof(ADUser[]), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Get(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            ps.Commands.Clear();
            ps.AddScript($@"Get-ADGroup -Identity ""{groupName}"" -Properties Members | Select-Object -ExpandProperty Members | Get-ADUser -Properties *");
            var psos = ps.Invoke();
            if (psos.IsNullOrCountEqualsZero())
            {
                return NotFound();
            }

            var uis = psos.Select(c => ADUserFactory.Create(c));
            return Ok(uis);
        }

        /// <summary>
        /// create new group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [HttpPost("{groupName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Post(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            ps.Commands.Clear();
            ps.AddCommand("New-ADGroup");
            ps.AddParameter("Name", groupName);
            ps.AddParameter("GroupScope", "Global");
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
        /// remove group member
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("Remove")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Remove(string groupName, string username)
        {
            if (String.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            ps.Commands.Clear();
            ps.AddCommand("Remove-ADGroupMember");
            ps.AddParameter("Identity", groupName);
            ps.AddParameter("Member", username);
            ps.AddParameter("Confirm", false);
            ps.Invoke();

            return Ok();
        }

        /// <summary>
        /// add group member
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("Add")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Add(string groupName, string username)
        {
            if (String.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            ps.Commands.Clear();
            ps.AddCommand("Add-ADGroupMember");
            ps.AddParameter("Identity", groupName);
            ps.AddParameter("Member", username);
            ps.AddParameter("Confirm", false);
            ps.Invoke();

            return Ok();
        }

        /// <summary>
        /// delete adgroup
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpDelete("{groupname}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult Delete(string groupname)
        {
            if (String.IsNullOrEmpty(groupname))
            {
                return BadRequest();
            }

            ps.Commands.Clear();
            ps.AddCommand("Remove-ADGroup");
            ps.AddParameter("Identity", groupname);
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

