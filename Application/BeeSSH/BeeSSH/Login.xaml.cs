using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void dragmove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
