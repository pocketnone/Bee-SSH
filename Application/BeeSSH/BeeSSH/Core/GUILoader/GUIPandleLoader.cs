using System;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BeeSSH.Core.API;
using Renci.SshNet;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;


namespace BeeSSH.Core.GUILoader
{
    public static class GUIPandleLoader
    {
        private static Interface.ApplicationWindow b = new Interface.ApplicationWindow();
        private static FrameworkElement _frameworkElement = new FrameworkElement();

        internal static void OpenGUI()
        {
            b.Show();
        }

        internal static void AddNewCMD(string ServerUID, string serverTitle)
        {
            b.TerminalBtnList.Items.Add(CreateServerItem(serverTitle, ServerUID));
        }
        
        private static MaterialDesignThemes.Wpf.Card CreateServerItem(string serverTitle, string serverUID)
        {
            var newIcon = new MaterialDesignThemes.Wpf.PackIcon()
            {
                Kind = MaterialDesignThemes.Wpf.PackIconKind.Server,
                Name = serverUID + "_ico",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),

            };
            
            var newBtn = new RadioButton()
            {
                Content = "Terminal",
                Name = serverUID,
                Style = _frameworkElement.FindResource("MaterialDesignFlatAccentButton") as Style,
                Foreground = Brushes.White,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness(5),
            };
            newBtn.Click += ConnectionInfo;

            var newStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            newStackPanel.Children.Add(newIcon);
            newStackPanel.Children.Add(newBtn);
            newStackPanel.Name = serverUID + "_span";

            return new MaterialDesignThemes.Wpf.Card()
            {
                Content = newStackPanel
            };
        }


        internal static void ConnectionInfo(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _ServerUID = button.Name.ToString();
            b.ContentFrame.Navigate(new Uri("Interface/UserControlls/TerminalUsercControl.xaml", UriKind.Relative));
        }
    }
}