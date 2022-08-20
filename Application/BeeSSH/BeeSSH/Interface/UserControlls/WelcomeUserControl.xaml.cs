using System.Diagnostics;
using System.Windows.Controls;
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
        }

        private void githubBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://github.com/sysfaker/Bee-SSH");
        }

        private void discordBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/A2pUGTPjru");
        }
    }
}
