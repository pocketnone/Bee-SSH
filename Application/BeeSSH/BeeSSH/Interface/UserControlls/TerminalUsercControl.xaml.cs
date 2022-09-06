using System;
using System.Windows.Controls;
using BeeSSH.Core.API;
using BeeSSH.Interface.CustomMessageBox;
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

        private void ConnectSSHWithOutRSA()
        {
            var b = Cache.ServerList.Find(x => x.ServerUID.Contains(_Servername));
            byte[] expectedFingerPrint = new byte[] { };
            bool fingerprint = false;
            if (b.FingerPrint != "null")
            {
                expectedFingerPrint = Convert.FromBase64String(b.FingerPrint);
                fingerprint = true;
            }

            var client = new SshClient(b.ServerIP, Int32.Parse(b.ServerPort), b.ServerUserName);
            client.HostKeyReceived += (sender, e) =>
            {
                if (fingerprint)
                {
                    if (expectedFingerPrint.Length == e.FingerPrint.Length)
                    {
                        for (var i = 0; i < expectedFingerPrint.Length; i++)
                        {
                           
                            if (expectedFingerPrint[i] != e.FingerPrint[i])
                            {
                                bool trust = Convert.ToBoolean(new BeeFingerprint($"The Server {b.ServerName} has a differnet fingerprint"
                                    , b.ServerName).ShowDialog());

                               if(!trust) {
                                    e.CanTrust = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool trust = Convert.ToBoolean(new BeeFingerprint($"The Server {b.ServerName} has a differnet fingerprint"
                            , b.ServerName).ShowDialog());
                        if(!trust)
                        {
                            e.CanTrust = false;
                        }
                    }
                }
            };
            client.Connect();
            
        }

        private void ConnectSSHwithRSA()
        {
            
        }
    }
}
