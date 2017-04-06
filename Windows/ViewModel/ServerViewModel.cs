using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;

namespace Windows
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class ServerViewModel : BaseViewModel
    {
        public ServerViewModel()
        {
            RefleshServerCommand = new RelayCommand(async () => await RefleshServer());
            ConnectVPNCommand = new RelayCommand(async () => await ConnectVPN());
            SelectCommand = new RelayParameterizedCommand(async (parameter) => await Select(parameter));
        }

        public ICommand RefleshServerCommand { get; set; }
        public ICommand ConnectVPNCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        public ServerListViewModel Servers { get; set; }

        public ServerListItemViewModel Server { get; set; }

        public bool IsFetching { get; set; }

        public bool IsTryConnecting { get; set; }

        public async Task RefleshServer()
        {
            await RunCommand(() => this.IsFetching, async () =>
            {
                this.Servers = null;
                TokenHelper tg = new TokenHelper();
                var token = tg.ReadTokenFromDisk();
                if (token == null)
                {
                    WindowViewModel.Instance.CurrentPage = ApplicationPage.Login;
                }
                ServerService server = new ServerService();
                try
                {
                    var servers = await server.GetServersAsync(token.access_token);
                    foreach (var s in servers)
                    {
                        s.ServerViewModel = this;
                    }

                    this.Servers = new ServerListViewModel { Items = servers.ToList() };
                }
                catch (Exception ex)
                {
                    WindowViewModel.Instance.CurrentPage = ApplicationPage.Login;
                }


            });
        }

        public async Task ConnectVPN()
        {
            await RunCommand(() => this.IsTryConnecting, async () =>
            {
                IVpnManagementAgent psvm = new PSVpnManager();
                await psvm.AddOrUpdateProfileAsync(Server.Domain);
                await psvm.ConnectProfileAsync("bosxixi", "xboxone");
            });
        }



        public async Task Select(object parameter)
        {
            await RunCommand(() => this.IsTryConnecting, async () =>
            {
                await Task.Delay(3000);
            });
        }
    }
}
