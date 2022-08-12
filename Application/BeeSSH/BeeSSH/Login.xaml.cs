using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace BeeSSH
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loginBtnOffline_Click(object sender, RoutedEventArgs e)
        {
            Interface.ApplicationWindow b = new Interface.ApplicationWindow();
            b.Show();
            this.Hide();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba");
        }

        private void forgotPassword(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba/user/resetpassword");
        }//TEST

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
    }
}
