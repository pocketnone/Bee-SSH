using System.Runtime.CompilerServices;
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
        private MaterialDesignThemes.Wpf.Card CreateServerItem(string serverTitle, string serverUID)
        {
            var newIcon = new MaterialDesignThemes.Wpf.PackIcon()
            {
                Kind = MaterialDesignThemes.Wpf.PackIconKind.Server,
                Name = serverUID,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),

            };
            var newTitle = new Label()
            {
                Content = serverTitle,
                Name = serverUID,
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            var newBtn = new RadioButton()
            {
                Content = "Connect",
                Name = serverUID,
                Style = FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            var DelBtn = new RadioButton()
            {
                Content = "Delete",
                Name = serverUID,
                Style = FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            newBtn.Click += Connect_Click;
            DelBtn.Click += Delete_Click;
            
            var newStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            newStackPanel.Children.Add(newIcon);
            newStackPanel.Children.Add(newTitle);
            newStackPanel.Children.Add(newBtn);
            newStackPanel.Children.Add(DelBtn);
            newStackPanel.Name = serverUID;

            return new MaterialDesignThemes.Wpf.Card()
            {
                Content = newStackPanel
            };
        }

        
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string content = button.Name.ToString();
            ConnectToServer(content);
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string serverUID = button.Name.ToString();
            var DeleteStackpandel = (StackPanel)this.FindName(serverUID);
            Request.DeleteServer(serverUID);
            DeleteStackpandel.Children.Clear();
            foreach (var oldServer in Cache.ServerList)
            {
                if (oldServer.ServerUID == serverUID)
                    Cache.ServerList.Remove(oldServer);
            }
        }
        
        private void ConnectToServer(string _ServerUID)
        {
            var b = Cache.ServerList.Find(x => x.ServerUID.Contains(_ServerUID));
            // @TODO: Open a Terminal with SSH.
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var _server in Cache.ServerList)
            {
                ServerList.Items.Add(CreateServerItem(_server.ServerName, _server.ServerUID));   
            }
        }
    }
}

