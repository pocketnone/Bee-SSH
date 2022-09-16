using System.Windows;
using System.Windows.Controls;
using BeeSSH.Interface.CustomMessageBox;
using static BeeSSH.Core.API.Request;
using static BeeSSH.Core.API.Cache;

namespace BeeSSH.Interface.UserControlls
{
    public partial class totp_imput : UserControl
    {
        public totp_imput()
        {
            InitializeComponent();
        }

        private void longinFinal(object sender, RoutedEventArgs e)
        {
            var res = Login(_email, _password, totpText.Text); // get all servers
            if (res == "ok")
            {
                FetchShortCutsScripts(); // Fetch Scripts
                Core.GUILoader.GUIPandleLoader.OpenGUI();
                var parentWindow = Window.GetWindow(this);
                parentWindow.Close();
            }
            else
            {
                new BeeMessageBox(res, BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog();
            }
        }
    }
}