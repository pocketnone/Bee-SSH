using System;
using System.Windows;
using System.Windows.Input;

namespace BeeSSH.Interface
{
    /// <summary>
    /// Interaktionslogik für ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/WelcomeUserControl.xaml", UriKind.Relative));
        }

        private void minBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/AddServerUserControl.xaml", UriKind.Relative));
        }

        private void ConnectionsBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/WelcomeUserControl.xaml", UriKind.Relative));
        }

        private void TerminalBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/TerminalUsercControl.xaml", UriKind.Relative));
        }

        private void GenerateRSABtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/RSAKeyUserControl.xaml", UriKind.Relative));
        }
    }
}
