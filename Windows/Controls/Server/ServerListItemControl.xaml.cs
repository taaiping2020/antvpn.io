using System.Windows;
using System.Windows.Controls;

namespace Windows
{
    /// <summary>
    /// Interaction logic for ServerListItemControl.xaml
    /// </summary>
    public partial class ServerListItemControl : UserControl
    {
        public ServerListItemControl()
        {
            InitializeComponent();
        }

        private void background_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sdjf = sender.GetType();
            //e.Source
            var item = (ServerListItemViewModel)((FrameworkElement)e.Source).DataContext;
            foreach (var server in item.ServerViewModel.Servers.Items)
            {
                server.IsSelected = false;
            }
            item.IsSelected = true;
            item.ServerViewModel.Server = item;
        }
    }
}
