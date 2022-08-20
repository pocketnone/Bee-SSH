using BeeSSH.Utils.DiscordRPC;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCConfig;

namespace BeeSSH
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private bool userIsLoggedIn = true; //bool temoprär
        DiscordRPCLoader loader = new DiscordRPCLoader(DiscordID);
        public Login()
        {
            InitializeComponent();

            loader.UpdateDetails("In Login");
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            loader.UpdateDetails("Pressed Button Login");
            if (loginBtn.Content.ToString() == "Final Step")
            {
                //make master password checks here
                Interface.ApplicationWindow b = new Interface.ApplicationWindow();
                b.Show();
                this.Hide();
            }
            else
            {
                //normal checks here
                if (userIsLoggedIn)
                    ShowMasterPasswordBox();
            }

        }

        private void ShowMasterPasswordBox()
        {
            loader.UpdateDetails("Look at Masterpassword Input");
            masterPasBoxCard.Visibility = Visibility.Visible;
            masterPasBox.Visibility = Visibility.Visible;
            loginBtn.Content = "Final Step";
            statusLbl.Content = "Enter your master password";


            regBtn.IsEnabled = false;
            runOfflineBtn.IsEnabled = false;
            emailBox.IsEnabled = false;
            faAuthBox.IsEnabled = false;
            passBox.IsEnabled = false;
            helpBtn.IsEnabled = false;
            runOfflineBtn.IsEnabled = false;
        }

        private void loginBtnOffline_Click(object sender, RoutedEventArgs e) => ShowMasterPasswordBox();


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba");
        }

        private void forgotPassword(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba/users/resetpassword");
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dragmove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void minBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;


        private void twoFAEnabled_Checked(object sender, RoutedEventArgs e) => faAuthBox.Visibility = Visibility.Visible;

        private void twoFAEnabled_Unchecked(object sender, RoutedEventArgs e) => faAuthBox.Visibility = Visibility.Collapsed;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetNormalDesign();
        }

        private void SetNormalDesign()
        {
            masterPasBoxCard.Visibility = Visibility.Collapsed;
            masterPasBox.Visibility = Visibility.Collapsed;
            loginBtn.Content = "Login";
            statusLbl.Content = "Log in to your Account or use it Offline";


            regBtn.IsEnabled = true;
            runOfflineBtn.IsEnabled = true;
            emailBox.IsEnabled = true;
            faAuthBox.IsEnabled = true;
            passBox.IsEnabled = true;
            helpBtn.IsEnabled = true;
            runOfflineBtn.IsEnabled = true;
        }

        // Only input Numbers
        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
