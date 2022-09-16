using MaterialDesignThemes.Wpf;
using System.Windows;

namespace BeeSSH.Interface.CustomMessageBox
{
    /// <summary>
    /// Interaction logic for BeeMessageBox.xaml
    /// </summary>
    public partial class BeeMessageBox : Window
    {
        public BeeMessageBox(string message, MessageType type, MessageButtons buttons)
        {
            ShowBeeMessageBox(message, type, buttons);
        }

        public void ShowBeeMessageBox(string message, MessageType type, MessageButtons buttons)
        {
            InitializeComponent();
            txtMessage.Text = message;
            switch (type)
            {
                case MessageType.Info:
                {
                    txtTitle.Text = "Info";
                    break;
                }
                case MessageType.Warning:
                {
                    txtTitle.Text = "Warning";
                    break;
                }
                case MessageType.Error:
                {
                    txtTitle.Text = "Error";
                    break;
                }
                case MessageType.API:
                {
                    txtTitle.Text = "API";
                    BtnClose.Visibility = Visibility.Hidden;
                    break;
                }
            }

            switch (buttons)
            {
                case MessageButtons.OkCancel:
                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.YesNo:
                    btnOk.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Ok:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
            }
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

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResultEnd(true);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResultEnd(false);
        }

        private void DialogResultEnd(bool val)
        {
            DialogResult = val;
            Close();
        }

        public enum MessageType
        {
            Info,
            Warning,
            Error,
            API
        }

        public enum MessageButtons
        {
            OkCancel,
            YesNo,
            Ok
        }
    }
}