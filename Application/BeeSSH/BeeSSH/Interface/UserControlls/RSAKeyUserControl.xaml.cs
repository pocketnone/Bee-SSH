using System.Windows;
using System.Windows.Controls;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for RSAKeyUserControl.xaml
    /// </summary>
    public partial class RSAKeyUserControl : UserControl
    {
        public RSAKeyUserControl()
        {
            InitializeComponent();
            RSAKE_MainView();
        }

        private void GenerateRSA(object sender, RoutedEventArgs e)
        {

        }
    }
}
