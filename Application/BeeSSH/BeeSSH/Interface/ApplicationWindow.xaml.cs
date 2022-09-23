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

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/AddServerUserControl.xaml", UriKind.Relative));
        }

        private void ConnectionsBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/ConnectionsUserControl.xaml", UriKind.Relative));
        }

        private void GenerateRSABtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/RSAKeyUserControl.xaml", UriKind.Relative));
        }

        private void OpenMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuBtn.Visibility = Visibility.Collapsed;
            CloseMenuBtn.Visibility = Visibility.Visible;
            ContentFrame.Width = 1042;
        }

        private void CloseMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuBtn.Visibility = Visibility.Visible;
            CloseMenuBtn.Visibility = Visibility.Collapsed;
            ContentFrame.Width = 1190;
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/SettingsUserControl.xaml", UriKind.Relative));
        }

        private void RemoteBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Interface/UserControlls/WelcomeUserControl.xaml", UriKind.Relative));
        }
    }
}