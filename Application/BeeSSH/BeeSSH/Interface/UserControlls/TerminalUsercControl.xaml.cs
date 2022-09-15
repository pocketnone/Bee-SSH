using System;
using System.IO;
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

        private SshClient client;
        private ShellStream _shellStream;
        public TerminalUsercControl()
        {
            InitializeComponent();
            string ServerUID = Cache._ServerUID;
            PandleServer(ServerUID);
            _Servername = ServerUID;
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

            if (!string.IsNullOrEmpty(b.RSAKEY) && !string.IsNullOrEmpty(b.ServerPassword))
            {
                string path = Path.GetTempPath() + b.ServerUID + ".key";
                File.WriteAllText(path, b.RSAKEY);
                client = new SshClient(b.ServerIP, Int32.Parse(b.ServerPort), b.ServerUserName, new PrivateKeyFile(path));
                File.Delete(path);
            }
            else
                client = new SshClient(b.ServerIP, Int32.Parse(b.ServerPort), b.ServerUserName, b.ServerPassword);
            
            
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
            SendCommand("cd ~");
        }

        public void SendDataToGUI(string data)
        {
            UI.Inlines.Add(new Label(){
                Content = data
            });
        }
        
        private void SendCommand(string command)
        {
            var cmd = client.CreateCommand(command);
            var res = cmd.Execute();
            SendDataToGUI(res);
        }
    }
}
