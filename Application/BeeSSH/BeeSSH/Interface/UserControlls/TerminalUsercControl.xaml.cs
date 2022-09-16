using System;
using System.IO;
using System.Windows.Controls;
using BeeSSH.Core.API;
using BeeSSH.Interface.CustomMessageBox;
using Renci.SshNet;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Core.API.Request;
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
            var ServerUID = _ServerUID;
            PandleServer(ServerUID);
            _Servername = ServerUID;
            Terminal_MainView();
        }

        private void ConnectSSHWithOutRSA()
        {
            var b = ServerList.Find(x => x.ServerUID.Contains(_Servername));
            var expectedFingerPrint = new byte[] { };
            var fingerprint = false;
            if (b.FingerPrint != "null")
            {
                expectedFingerPrint = Convert.FromBase64String(b.FingerPrint);
                fingerprint = true;
            }

            if (b.RSAKEY)
            {
                var path = Path.GetTempPath() + b.ServerUID + ".key";
                File.WriteAllText(path, b.RSAKeyText);
                client = new SshClient(b.ServerIP, int.Parse(b.ServerPort), b.ServerUserName, new PrivateKeyFile(path));
                File.Delete(path);
            }
            else
            {
                client = new SshClient(b.ServerIP, int.Parse(b.ServerPort), b.ServerUserName, b.ServerPassword);
            }


            client.HostKeyReceived += (sender, e) =>
            {
                if (fingerprint)
                {
                    if (expectedFingerPrint.Length == e.FingerPrint.Length)
                    {
                        for (var i = 0; i < expectedFingerPrint.Length; i++)
                            if (expectedFingerPrint[i] != e.FingerPrint[i])
                            {
                                var trust = Convert.ToBoolean(new BeeFingerprint(
                                    $"The Server {b.ServerName} has a differnet fingerprint"
                                    , b.ServerName).ShowDialog());

                                if (!trust)
                                {
                                    e.CanTrust = false;
                                    break;
                                }
                                else
                                {
                                    AddFingerprint(Convert.ToBase64String(e.FingerPrint), b.ServerUID);
                                }
                            }
                    }
                    else
                    {
                        var trust = Convert.ToBoolean(new BeeFingerprint(
                            $"The Server {b.ServerName} has a differnet fingerprint"
                            , b.ServerName).ShowDialog());
                        if (!trust) e.CanTrust = false;
                    }
                }
                else
                {
                    var trust = Convert.ToBoolean(new BeeFingerprint(
                        $"Add new Fingerprint for {b.ServerName}", b.ServerName).ShowDialog());
                    if (!trust)
                    {
                        e.CanTrust = false;
                    }
                    else
                    {
                        AddFingerprint(Convert.ToBase64String(e.FingerPrint), b.ServerUID);
                        e.CanTrust = true;
                    }
                }
                
            };
            client.Connect();
            SendCommand("cd ~");
        }

        public void SendDataToGUI(string data)
        {
            var lab = new Label()
            {
                Content = data
            };
            UI.Children.Add(lab);
        }

        private void SendCommand(string command)
        {
            var cmd = client.CreateCommand(command);
            var res = cmd.Execute();
            SendDataToGUI(res);
        }
    }
}