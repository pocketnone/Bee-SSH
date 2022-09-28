using System.Windows;
using System.Windows.Controls;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;
using static BeeSSH.Core.API.Request;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Core.Crypter.String;
using BeeSSH.Interface.CustomMessageBox;
using WK.Libraries.BetterFolderBrowserNS;
using System.IO;
using System.Windows.Forms;
using BeeSSH.Core.API;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for AddServerUserControl.xaml
    /// </summary>
    public partial class AddServerUserControl : UserControl
    {
        private string rsakey_buff = null;

        public AddServerUserControl()
        {
            InitializeComponent();
            AddServer_MainView();
            ServerPassPharse.Visibility = Visibility.Hidden;
        }

        private void AddRSABtn(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "BEESSH | Select RSA File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = ofd.FileName;
                rsakey_buff = File.ReadAllText(filePath);
                ServerPassPharse.Visibility = Visibility.Visible;
            }

            ofd.Dispose();
        }

        private void AddServerBtn(object sender, RoutedEventArgs e)
        {
            var response = new AddServerResponse();
            if (string.IsNullOrEmpty(rsakey_buff))
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass),
                    Encrypt(ServerPort.Text, EncryptionMasterPass), false,
                    Encrypt("null", EncryptionMasterPass), Encrypt(ServerIP.Text, EncryptionMasterPass),
                    Encrypt(ServerPassword.Password, EncryptionMasterPass),
                    Encrypt(Serverusername.Text, EncryptionMasterPass),
                    Encrypt(ServerPassPharse.Text, EncryptionMasterPass));
                ServerList.Add(new ServerListModel
                {
                    PassPharse = ServerPassPharse.Text,
                    RSAKEY = false,
                    RSAKeyText = "null",
                    ServerIP = ServerIP.Text,
                    ServerName = ServerName.Text,
                    ServerPassword = ServerPassword.Password,
                    ServerPort = ServerPort.Text,
                    ServerUserName = Serverusername.Text,
                    FingerPrint = "null",
                    ServerUID = response.ServerUID
                });
               
            }
            else
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass),
                    Encrypt(ServerPort.Text, EncryptionMasterPass), true,
                    Encrypt(rsakey_buff, EncryptionMasterPass), Encrypt(ServerIP.Text, EncryptionMasterPass),
                    Encrypt(ServerPassword.Password, EncryptionMasterPass),
                    Encrypt(Serverusername.Text, EncryptionMasterPass),
                    Encrypt(ServerPassPharse.Text, EncryptionMasterPass));
                ServerList.Add(new ServerListModel
                {
                    PassPharse = ServerPassPharse.Text,
                    RSAKEY = true,
                    RSAKeyText = rsakey_buff,
                    ServerIP = ServerIP.Text,
                    ServerName = ServerName.Text,
                    ServerPassword = ServerPassword.Password,
                    ServerPort = ServerPort.Text,
                    ServerUserName = Serverusername.Text,
                    FingerPrint = "null",
                    ServerUID = response.ServerUID
                }); 
                
            }

            new BeeMessageBox(response.DataRes, BeeMessageBox.MessageType.Info, BeeMessageBox.MessageButtons.Ok).ShowDialog();
            
            ServerPassPharse.Visibility = Visibility.Hidden;
            ServerName.Text = "";
            ServerPort.Text = "";
            ServerName.Text = "";
            ServerIP.Text  = "";
            ServerPassword.Password = "";
            ServerPassPharse.Text = "";
            Serverusername.Text = "";
        }
    }
}