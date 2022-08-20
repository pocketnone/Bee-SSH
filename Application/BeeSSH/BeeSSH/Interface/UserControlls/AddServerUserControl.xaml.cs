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
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for AddServerUserControl.xaml
    /// </summary>
    public partial class AddServerUserControl : UserControl
    {
        byte[] rsakey_buff = null; 
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
                rsakey_buff = File.ReadAllBytes(filePath);
            }
            ofd.Dispose();
        }

        private void AddServerBtn(object sender, RoutedEventArgs e)
        {
            string response = "Not Requested";
            if (rsakey_buff == null && string.IsNullOrEmpty(ServerPassPharse.Text))
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass), Encrypt(ServerPort.Text, EncryptionMasterPass),
               false, Encrypt(ServerIP.Text, EncryptionMasterPass), Encrypt(ServerPassword.Password, EncryptionMasterPass), 
               Encrypt(Serverusername.Text, EncryptionMasterPass),
               Encrypt(ServerPassPharse.Text, EncryptionMasterPass));
            }
            else if (rsakey_buff == null)
            {
                response = AddServer(Encrypt(ServerName.Text, EncryptionMasterPass), Encrypt(ServerPort.Text, EncryptionMasterPass),
               false, Encrypt(ServerIP.Text, EncryptionMasterPass), Encrypt(ServerPassword.Password, EncryptionMasterPass),
               Encrypt(ServerPassPharse.Text, EncryptionMasterPass), Encrypt(Serverusername.Text, EncryptionMasterPass));
            }
            else
            {

            }
            new BeeMessageBox(response, BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog();
        }
    }
}
