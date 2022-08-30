using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BeeSSH.Core.API;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for ConnectionsUserControl.xaml
    /// </summary>
    public partial class ConnectionsUserControl : UserControl
    {
        public ConnectionsUserControl()
        {
            InitializeComponent();
            Connections();
        }
        private MaterialDesignThemes.Wpf.Card CreateServerItem(string serverTitle)
        {
            var newIcon = new MaterialDesignThemes.Wpf.PackIcon()
            {
                Kind = MaterialDesignThemes.Wpf.PackIconKind.Server,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),

            };
            var newTitle = new Label()
            {
                Content = serverTitle,
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            var newBtn = new RadioButton()
            {
                Content = "Connect",
                Style = FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            ;
            
            var newStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            newStackPanel.Children.Add(newIcon);
            newStackPanel.Children.Add(newTitle);
            newStackPanel.Children.Add(newBtn);

            return new MaterialDesignThemes.Wpf.Card()
            {
                Content = newStackPanel
            };
        }

        private void ConnectToServer(string _Servername)
        {
            var b = Cache.ServerList.Find(x => x.ServerName.Contains(_Servername));
            // @TODO: Open a Terminal with SSH.
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var _server in Cache.ServerList)
            {
                ServerList.Items.Add(CreateServerItem(_server.ServerName));   
            }
        }
    }
}

