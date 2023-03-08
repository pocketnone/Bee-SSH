using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
       
        
        ConsoleContent dc = new ConsoleContent();

        public TerminalUsercControl()
        {
            InitializeComponent();
            var ServerUID = _ServerUID;
            PandleServer(ServerUID);
            Terminal_MainView();
            dc.SetServer(ServerUID);
            DataContext = dc;
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InputBlock.KeyDown += InputBlock_KeyDown;
            InputBlock.Focus();
        }
        
        void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dc.ConsoleInput = InputBlock.Text;
                dc.RunCommand();
                InputBlock.Focus();
                Scroller.ScrollToBottom();
            }
        }
        
    }
    
    public class ConsoleContent : INotifyPropertyChanged
    {
        private SshClient client;
        private ShellStream _shellStream;
        string consoleInput = string.Empty;

        public void SetServer(string servername)
        {
            this._Servername = servername;
            ConnectSSHWithOutRSA();
        }
        private string _Servername { get; set; }
        
        
        ObservableCollection<string> consoleOutput = new ObservableCollection<string>() { "BEESSH" };

        public string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            set
            {
                consoleInput = value;
                OnPropertyChanged("ConsoleInput");
            }
        }

        [STAThread]
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
                                bool? trust = null;
                                new Thread(() =>
                                {
                                    trust = Convert.ToBoolean(new BeeFingerprint(
                                        $"The Server {b.ServerName} has a differnet fingerprint"
                                        , b.ServerName).ShowDialog());
                                });
                                while (trust == null)
                                { Thread.Sleep(100); }

                                if (!Convert.ToBoolean(trust))
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
            
            
            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                new BeeMessageBox(ex.Message, BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog();
            }
            
        }
        
        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }

        public void RunCommand()
        {
            ConsoleOutput.Add(ConsoleInput);
            var cmd = client.CreateCommand(ConsoleInput);
            var res = cmd.Execute();
            ConsoleOutput.Add(res);
            
            
            ConsoleInput = String.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}