using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private AccountService accountService = new AccountService();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Handle_Login(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LoginInformation loginInformation = new LoginInformation
            {
                email = txtEmail.Text,
                password = txtPassword.Password
            };
            SortedList<string, string> errorMessage = Validate_Account(loginInformation);
            msgPassword.Text = errorMessage.ContainsKey("passwordError") ? errorMessage["passwordError"] : "";
            msgEmail.Text = errorMessage.ContainsKey("emailError") ? errorMessage["emailError"] : "";
            if (errorMessage.Count == 0)
            {
                var result = await accountService.LoginAsync(loginInformation);
                ContentDialog contentDialog = new ContentDialog();

                if (result != null)
                {
                    Account account = await accountService.GetAccountInformation(result.access_token);
                    App.currentLoggedIn = account;
                    contentDialog.Title = "Action success.";
                    contentDialog.Content = "Login completed.";
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    contentDialog.Title = "Action failed.";
                    contentDialog.Content = "Wrong email or password. Please try again.";
                }
                contentDialog.CloseButtonText = "Ok";
                await contentDialog.ShowAsync();
            }
        }

        private SortedList<string, string> Validate_Account(LoginInformation loginInformation)
        {
            SortedList<string, string> errorMessage = new SortedList<string, string>();
            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            string emailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!Regex.IsMatch(loginInformation.password, passwordPattern))
            {
                errorMessage.Add("passwordError", "Invalid password.");
            }
            if (!Regex.IsMatch(loginInformation.email, emailPattern))
            {
                errorMessage.Add("emailError", "Invalid email address.");
            }
            return errorMessage;
        }

        private void Handle_Register(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Pages.RegisterPage));
        }
    }
}