using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BeeSSH.Core.API;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Core.GUILoader.GUIPandleLoader;
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
                Name = serverUID + "_ico",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            var newTitle = new Label()
            {
                Content = serverTitle,
                Name = serverUID + "_tit",
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            var newBtn = new RadioButton()
            {
                Content = "Connect",
                Name = serverUID,
                Style = FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            var DelBtn = new RadioButton()
            {
                Content = "Delete",
                Name = serverUID + "_del",
                Style = FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            newBtn.Click += Connect_Click;
            DelBtn.Click += Delete_Click;

            var newStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            newStackPanel.Children.Add(newIcon);
            newStackPanel.Children.Add(newTitle);
            newStackPanel.Children.Add(newBtn);
            newStackPanel.Children.Add(DelBtn);
            newStackPanel.Name = serverUID + "_stackp";

            return new MaterialDesignThemes.Wpf.Card()
            {
                Content = newStackPanel
            };
        }


        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            var content = button.Name.ToString();
            var b = Cache.ServerList.Find(x => x.ServerUID.Contains(content));
            ConnectToServer(content, b.ServerName);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            var serverUID = button.Name.ToString().Replace("_del", "");
           
            
            Request.DeleteServer(serverUID);

            try
            {
                StackPanel st = this.FindName(serverUID + "_stackp") as StackPanel;
                st.Children.Clear();
            } catch{}
            

            foreach (var oldServer in Cache.ServerList)
                if (oldServer.ServerUID == serverUID)
                {
                    Cache.ServerList.Remove(oldServer);
                    break;
                }
        }

        private void ConnectToServer(string _ServerUID, string name)
        {
            AddNewCMD(_ServerUID, name);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var _server in Cache.ServerList)
                ServerList.Items.Add(CreateServerItem(_server.ServerName, _server.ServerUID));
        }
    }
}