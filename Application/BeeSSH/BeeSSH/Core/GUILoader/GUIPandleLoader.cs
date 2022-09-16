using System;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BeeSSH.Core.API;
using DiscordRPC;
using Renci.SshNet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
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

        private static RadioButton CreateServerItem(string serverTitle, string serverUID)
        {
            b.TerminalBtnList.Visibility = Visibility.Visible;
            var newBtn = new RadioButton()
            {
                Content = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new MaterialDesignThemes.Wpf.PackIcon()
                        {
                            Kind = MaterialDesignThemes.Wpf.PackIconKind.TerminalLine,
                            Name = serverUID + "_ico",
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0,0,5,0),
                            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        },
                        new Label()
                        {
                            Content = serverTitle,
                            FontSize = 16,
                            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        }
                    }
                },
                Name = serverUID,
                Foreground = Brushes.White,
                Style = _frameworkElement.FindResource("MaterialDesignFlatAccentButton") as Style
            };
            newBtn.Click += ConnectionInfo;

            //newStackPanel.Name = serverUID + "_span";

            return newBtn;
        }


        internal static void ConnectionInfo(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            _ServerUID = button.Name.ToString();
            b.ContentFrame.Navigate(new Uri("Interface/UserControlls/TerminalUsercControl.xaml", UriKind.Relative));
        }
    }
}