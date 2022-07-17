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

namespace BeeSSH.UserFrames
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        // Open Webapp
        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba/");
        }

        private void Offline_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
