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

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void openSettings(object sender, RoutedEventArgs e)
        {

        }

        private void dragmove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void appminimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
