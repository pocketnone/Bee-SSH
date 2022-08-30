﻿using System;
using BeeSSH.Utils.DiscordRPC;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BeeSSH.Interface.CustomMessageBox;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCManager;
using static BeeSSH.Core.API.Request;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Core.Autosave.AutoLogin;

namespace BeeSSH
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private bool userIsLoggedIn = true; //bool temoprär
        public Login()
        {
            InitializeComponent();
            LoginView();
            bool _b = GetAutoLogin();
            if (_b)
            {
                string[] b = GiveLoginData();
                EncryptionMasterPass = b[2];
                _email = b[0];
                _password = b[1];
                ContentFrame.Navigate(new Uri("Interface/UserControlls/totp_imput.xaml", UriKind.Relative));
                // @TODO: Open a "totp" imput Field filled over the Screen.
                // @TODO: Request Example at line 49
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loginBtn.Content.ToString() == "Final Step")
            {
                //make master password checks here
                EncryptionMasterPass = masterPasBox.Password;
                _email = emailBox.Text;
                _password = passBox.Password;
                string otp = faAuthBox.Text;

                if (!string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_password))
                {
                    string res = Login(_email, _password, otp);           // get all servers
                    if (res == "ok")
                    {
                        if (autologin.IsChecked == true)
                        {
                            CreateAutologin(_email, _password, EncryptionMasterPass, true);
                        }
                        FetchShortCutsScripts(); // Fetch Scripts
                        Interface.ApplicationWindow b = new Interface.ApplicationWindow();
                        b.Show();
                        this.Close(); 
                    } else
                    {
                        new BeeMessageBox(res, BeeMessageBox.MessageType.Error, BeeMessageBox.MessageButtons.Ok).ShowDialog();
                    }
                }
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

        private void loginBtnOffline_Click(object sender, RoutedEventArgs e)
        {
            Interface.ApplicationWindow b = new Interface.ApplicationWindow();
            b.Show();
            this.Close();
        }


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

        private void helpBtnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://as.mba");
        }
        
    }
}
