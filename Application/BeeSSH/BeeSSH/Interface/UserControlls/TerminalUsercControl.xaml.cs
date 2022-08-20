using System.Windows.Controls;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for TerminalUsercControl.xaml
    /// </summary>
    public partial class TerminalUsercControl : UserControl
    {
        public TerminalUsercControl()
        {
            InitializeComponent();
            Terminal_MainView();
        }
    }
}
