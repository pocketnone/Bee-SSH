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
        string rsakey_buff = null; 
        public AddServerUserControl()
        {
            InitializeComponent();
            AddServer_MainView();
        }

        private void AddRSABtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "BEESSH | Select RSA File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = ofd.FileName;
                rsakey_buff = File.ReadAllText(filePath);
            }
            ofd.Dispose();
        }

        private void AddServerBtn(object sender, RoutedEventArgs e)
        {
            string response = "Not Requested";
            ServerList.Add(new ServerListModel
            {
                PassPharse = ServerPassPharse.Text,
                RSAKEY = rsakey_buff,
                ServerIP = ServerIP.Text,
                ServerName = ServerName.Text,
                ServerPassword = ServerPassword.Password,
                ServerPort = ServerPort.Text,
                ServerUserName = Serverusername.Text,
                FingerPrint = "null"
            });
            if (rsakey_buff == null && string.IsNullOrEmpty(ServerPassPharse.Text))
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass), Encrypt(ServerPort.Text, EncryptionMasterPass),
               "null", Encrypt(ServerIP.Text, EncryptionMasterPass), Encrypt(ServerPassword.Password, EncryptionMasterPass), 
               Encrypt(Serverusername.Text, EncryptionMasterPass),
               Encrypt(ServerPassPharse.Text, EncryptionMasterPass));
            }
            else if (rsakey_buff == null && !string.IsNullOrEmpty(ServerPassPharse.Text))
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass), Encrypt(ServerPort.Text, EncryptionMasterPass),
               "null", Encrypt(ServerIP.Text, EncryptionMasterPass), Encrypt(ServerPassword.Password, EncryptionMasterPass),
               Encrypt(ServerPassPharse.Text, EncryptionMasterPass), Encrypt(Serverusername.Text, EncryptionMasterPass));
            }
            else
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass), Encrypt(ServerPort.Text, EncryptionMasterPass),
                    Encrypt(rsakey_buff, EncryptionMasterPass), Encrypt(ServerIP.Text, EncryptionMasterPass), Encrypt(rsakey_buff, EncryptionMasterPass),
                    Encrypt(ServerPassPharse.Text, EncryptionMasterPass), Encrypt(Serverusername.Text, EncryptionMasterPass));
            }
            new BeeMessageBox(response, BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog();
        }
    }
}
