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

        private void MessageBoxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (new BeeMessageBox("Example", BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog().Value)
            {
                var name = new BeeMessageBox("Example2", BeeMessageBox.MessageType.Warning, BeeMessageBox.MessageButtons.YesNo).ShowDialog().Value;
            }
        }
    }
}
