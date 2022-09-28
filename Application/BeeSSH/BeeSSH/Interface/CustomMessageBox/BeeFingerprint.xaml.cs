using System;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace BeeSSH.Interface.CustomMessageBox
{
    /// <summary>
    /// Interaction logic for BeeMessageBox.xaml
    /// </summary>
    public partial class BeeFingerprint : Window
    {
        public BeeFingerprint(string message, string servername)
        {
            BeeFingerprintBox(message, servername);
        }
        public void BeeFingerprintBox(string message, string servername)
        {
            InitializeComponent();
            txtTitle.Text = "New Fingerprint from " + servername;
            txtMessage.Text = message;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //PaletteHelper für later design change stuff.
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            paletteHelper.SetTheme(theme);
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResultEnd(false);
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResultEnd(true);
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResultEnd(false);
        }

        private void DialogResultEnd(bool val)
        {
            DialogResult = val;
            Close();
        }
    }
}