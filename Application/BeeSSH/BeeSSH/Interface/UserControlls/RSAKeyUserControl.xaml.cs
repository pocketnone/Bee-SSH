using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BeeSSH.Interface.CustomMessageBox;
using WK.Libraries.BetterFolderBrowserNS;
using UserControl = System.Windows.Controls.UserControl;
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
            if (string.IsNullOrEmpty(RSAKeyName.ToString()) && string.IsNullOrEmpty(RSAKeyByts.ToString()))
            {


                var FolderBrowser = new BetterFolderBrowser();
                FolderBrowser.Title = "Select Folder to save...";
                FolderBrowser.RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                FolderBrowser.Multiselect = false;
                if (FolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    new Thread(() =>
                    {
                        var keygen = new global::SshKeyGenerator.SshKeyGenerator(Convert.ToInt32(RSAKeyByts));
                        var privateKey = keygen.ToPrivateKey();
                        var publicSshKey = keygen.ToRfcPublicKey();
                        string folder = FolderBrowser.SelectedFolder;
                        string _privateKey = "private_" + RSAKeyName + ".key";
                        string _publicKey = "public_" + RSAKeyName + ".key";
                        _privateKey = Path.Combine(folder, _privateKey);
                        _publicKey = Path.Combine(folder, _publicKey);
                        File.WriteAllText(_publicKey, publicSshKey);
                        File.WriteAllText(_privateKey,privateKey);
                        new BeeMessageBox("Generated Key.", BeeMessageBox.MessageType.Info, BeeMessageBox.MessageButtons.Ok).ShowDialog();
                    }).Start();
                }
            }
            else
            {
                new BeeMessageBox("Please fill out all field", BeeMessageBox.MessageType.Error,
                    BeeMessageBox.MessageButtons.Ok).ShowDialog();
            }
        }

        private void onlynumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
