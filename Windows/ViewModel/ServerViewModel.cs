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
            RefleshServerCommand.Execute(this);
        }

        public ICommand RefleshServerCommand { get; set; }

        public ServerListDesignModel Servers { get; set; } = ServerListDesignModel.Instance;

        public bool LoginIsRunning { get; set; }

        public async Task RefleshServer()
        {
            await RunCommand(() => this.LoginIsRunning, async () =>
            {
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
                    var s = servers.First();
                    s.IsSelected = true;
                    this.Servers = new ServerListDesignModel { Items = servers.ToList() };
                }
                catch (Exception)
                {
                    WindowViewModel.Instance.CurrentPage = ApplicationPage.Login;
                }

 
            });
        }
    }
}
