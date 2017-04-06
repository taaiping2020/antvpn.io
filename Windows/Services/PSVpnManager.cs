using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Windows
{
    class PSVpnManager : IVpnManagementAgent
    {
        public PSVpnManager()
        {
            this.rasDialFileName = Path.Combine(WinDir, "rasdial.exe");
        }
        public const string ConnectionName = "antvpn.io";
        private static string WinDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
        private string rasDialFileName;

        private string RasDialFileName
        {
            get { return rasDialFileName; }
            set
            {
                if (File.Exists(value))
                {
                    rasDialFileName = value;
                }

                throw new FileNotFoundException();
            }
        }

        static PowerShell ps = PowerShell.Create();
        private bool IsActive
        {
            get
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.RedirectStandardInput = true;
                    myProcess.StartInfo.RedirectStandardOutput = true;
                    myProcess.StartInfo.FileName = "cmd.exe";
                    myProcess.Start();
                    myProcess.StandardInput.WriteLine("ipconfig");
                    myProcess.StandardInput.WriteLine("exit");
                    myProcess.WaitForExit();

                    string content = myProcess.StandardOutput.ReadToEnd();
                    if (content.Contains("0.0.0.0"))
                    {
                        return true;
                    }

                    return false;
                }

            }
        }

        private bool WaitUntilActive(int timeOut = 10)
        {
            for (int i = 0; i < timeOut; i++)
            {
                if (!this.IsActive)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
        private bool WaitUntilInActive(int timeOut = 10)
        {
            for (int i = 0; i < timeOut; i++)
            {
                if (this.IsActive)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
        public async Task AddOrUpdateProfileAsync(string serverAddress)
        {
            if (String.IsNullOrEmpty(serverAddress))
            {
                throw new ArgumentNullException(nameof(serverAddress));
            }
            await Task.Factory.StartNew(() =>
            {
                ps.Commands.Clear();
                ps.AddCommand("Add-VpnConnection");
                ps.AddParameter("Name", ConnectionName);
                ps.AddParameter("ServerAddress", serverAddress);
                try
                {
                    ps.Invoke();
                }
                catch (CmdletInvocationException ex)
                {
                    if (ex.ErrorRecord.FullyQualifiedErrorId == "WIN32 183,Add-VpnConnection")
                    {
                        // try update vpn connection
                        ps.Commands.Clear();
                        ps.AddCommand("Set-VpnConnection");
                        ps.AddParameter("Name", ConnectionName);
                        ps.AddParameter("ServerAddress", serverAddress);
                        ps.Invoke();
                    }
                };
            });
        }

        public async Task<VpnManagementErrorStatus> ConnectProfileAsync(string username, string password)
        {
            await Task.Factory.StartNew(() =>
            {
                if (String.IsNullOrEmpty(username))
                {
                    throw new ArgumentNullException(nameof(username));
                }
                if (String.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException(nameof(password));
                }

                string args = $"{PSVpnManager.ConnectionName} {username} {password}";
                ProcessStartInfo myProcess = new ProcessStartInfo(rasDialFileName, args)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(myProcess);
                WaitUntilActive();
            });

            if (IsActive)
            {
                return VpnManagementErrorStatus.Ok;
            }

            return VpnManagementErrorStatus.Other;
        }

        public async Task<VpnManagementErrorStatus> DisconnectProfileAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                string args = $"{PSVpnManager.ConnectionName} /disconnect";
                ProcessStartInfo myProcess = new ProcessStartInfo(rasDialFileName, args)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(myProcess);
                WaitUntilInActive();
            });

            if (!IsActive)
            {
                return VpnManagementErrorStatus.Ok;
            }

            return VpnManagementErrorStatus.Other;
        }

        public async Task<VpnManagementErrorStatus> DeleteProfileAsync()
        {
            if (String.IsNullOrEmpty(PSVpnManager.ConnectionName))
            {
                throw new ArgumentNullException(nameof(PSVpnManager.ConnectionName));
            }
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    ps.Commands.Clear();
                    ps.AddCommand("Remove-VpnConnection");
                    ps.AddParameter("Name", PSVpnManager.ConnectionName);
                    ps.AddParameter("Confirm", false);

                    ps.Invoke();
                });
            }
            catch (CmdletInvocationException)
            {
                return VpnManagementErrorStatus.CannotFindProfile;
            };
            return VpnManagementErrorStatus.Ok;
        }
    }
}
