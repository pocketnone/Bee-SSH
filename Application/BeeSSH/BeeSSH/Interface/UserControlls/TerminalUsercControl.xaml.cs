using System.Windows.Controls;
using BeeSSH.Core.API;
using Renci.SshNet;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for TerminalUsercControl.xaml
    /// </summary>
    public partial class TerminalUsercControl : UserControl
    {
        private string _Servername { get; set; }
        public TerminalUsercControl(string Servername)
        {
            InitializeComponent();
            PandleServer(Servername);
            _Servername = Servername;
            Terminal_MainView();
        }

        private void ConnectSSH()
        {
            var b = Cache.ServerList.Find(x => x.ServerUID.Contains(_Servername));
            
        }
    }
}
