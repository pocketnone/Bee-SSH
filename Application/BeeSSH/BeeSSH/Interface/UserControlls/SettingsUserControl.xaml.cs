using BeeSSH.Interface.CustomMessageBox;
using System.Windows;
using System.Windows.Controls;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;


namespace BeeSSH.Interface.UserControlls
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();
            Settings_MainView();
        }

        private void pwchangeBtnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnCheckForUpdate_OnClickBtnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}