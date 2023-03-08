using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for WelcomeUserControl.xaml
    /// </summary>
    public partial class WelcomeUserControl : UserControl
    {
        public WelcomeUserControl()
        {
            InitializeComponent();
            MainView();
            WelcomeBack.Text = "Welcome back " + _Username + "!";
        }

        private void githubBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/sysfaker/Bee-SSH");
        }

        private void discordBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/A2pUGTPjru");
        }

        private void twitterBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://twitter.com/SshBeessh");
        }

        private void Webpagebtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://res.yt/");
        }
    }
}