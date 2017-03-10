using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using System.Security;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Login.API.Controllers
{
    [Route("api/[controller]")]
    public class RouterController : Controller
    {
        /// <summary>
        /// get online connections
        /// </summary>
        /// <returns></returns>
        [HttpGet("Connections")]
        [ProducesResponseType(typeof(RemoteAccessConnection[]), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult Get()
        {
            try
            {
                InitialSessionState initial = InitialSessionState.CreateDefault();
                initial.ImportPSModule(new string[] { "RemoteAccess",  });
                using (var runspace = RunspaceFactory.CreateRunspace(initial))
                using (var ps = PowerShell.Create())
                {
                    runspace.Open();
                    ps.Runspace = runspace;
                    ps.Commands.AddScript("$secpasswd = ConvertTo-SecureString 'xboxone' -AsPlainText -Force;" + Environment.NewLine
                                        + "$mycreds = New-Object System.Management.Automation.PSCredential ('bosxixi', $secpasswd);" + Environment.NewLine
                                        + "$j = New-PSSession -ComputerName 'localhost' -Credential $mycreds;" + Environment.NewLine
                                        + "Invoke-Command -Session $j -ScriptBlock { Get-RemoteAccessConnectionStatistics };" + Environment.NewLine
                                        + "Remove-PSSession -Session $j" + Environment.NewLine);
                    var psos = ps.Invoke();
                    if (psos.IsNullOrCountEqualsZero())
                    {
                        return NotFound("no connection on this server.");
                    }

                    var racs = psos.Select(c => new RemoteAccessConnection(c));
                    return Ok(racs);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        /// <summary>
        /// disconnect aduser
        /// </summary>
        /// <param username="username"></param>
        /// <returns></returns>
        [HttpDelete("Disconnect/{username}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Disconnect(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            try
            {
                using (var ps = PowerShell.Create())
                {
                    //ps.AddScript($@"Get-RemoteAccessConnectionStatistics | where {{ $_.UserName -like ""*\{username}"" -or $_UserName -like ""{username}"" }} | Select-Object UserName | Disconnect-VpnUser");
                    ps.Commands.AddScript("$secpasswd = ConvertTo-SecureString 'xboxone' -AsPlainText -Force;" + Environment.NewLine
                                      + "$mycreds = New-Object System.Management.Automation.PSCredential ('bosxixi', $secpasswd);" + Environment.NewLine
                                      + "$j = New-PSSession -ComputerName 'localhost' -Credential $mycreds;" + Environment.NewLine
                                      + $"Invoke-Command -Session $j -ScriptBlock {{ Get-RemoteAccessConnectionStatistics | where {{ $_.UserName -like '*\\{username}' -or $_UserName -like '{username}' }} | Select-Object UserName | Disconnect-VpnUser }};" + Environment.NewLine
                                      + "Remove-PSSession -Session $j" + Environment.NewLine);
                    ps.Invoke();
                }
                using (var ps = PowerShell.Create())
                {
                    //ps.AddCommand($@"Disconnect-VpnUser");
                    //ps.AddParameter("UserName", username);
                    ps.Commands.AddScript("$secpasswd = ConvertTo-SecureString 'xboxone' -AsPlainText -Force;" + Environment.NewLine
                                + "$mycreds = New-Object System.Management.Automation.PSCredential ('bosxixi', $secpasswd);" + Environment.NewLine
                                + "$j = New-PSSession -ComputerName 'localhost' -Credential $mycreds;" + Environment.NewLine
                                + $"Invoke-Command -Session $j -ScriptBlock {{ Disconnect-VpnUser -UserName {username} }} | Select-Object UserName | Disconnect-VpnUser }};" + Environment.NewLine
                                + "Remove-PSSession -Session $j" + Environment.NewLine);
                    try
                    {
                        ps.Invoke();
                    }
                    catch (Exception)
                    {
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet("Test/{cmdlet}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult Test(string cmdlet)
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { "RemoteAccess" });
            using (var runspace = RunspaceFactory.CreateRunspace(initial))
            using (var ps = PowerShell.Create())
            {
                runspace.Open();
                ps.Runspace = runspace;
                ps.AddScript($@"{cmdlet}");
                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    return BadRequest(psos);
                }

                return Ok(psos);
            }
        }

        /// <summary>
        /// get online connections
        /// </summary>
        /// <returns></returns>
        [HttpGet("Info")]
        [ProducesResponseType(typeof(RemoteAccessConnection[]), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetInfo()
        {
            using (var ps = PowerShell.Create())
            {
                //ps.
                ps.AddCommand("Get-RemoteAccessConnectionStatistics");
                var psos = ps.Invoke();
                if (psos.IsNullOrCountEqualsZero())
                {
                    return NotFound("no connection on this server.");
                }

                var racs = psos.Select(c => new RemoteAccessConnection(c));
                return Ok(racs);
            }
        }
    }
}
