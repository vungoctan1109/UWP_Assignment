using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class RegisterPage : Page
    {
        private AccountService accountService = new AccountService();

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void Handle_Submit(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Account account = new Account
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                password = txtPassword.Password,
                address = txtAddress.Text,
                phone = txtPhone.Text,
                avatar = txtAvatar.Text,
                email = txtEmail.Text,
                birthday = txtBirthday.Date.DateTime.ToString("yyyy-MM-dd")
            };

            foreach (var child in txtGender.Children)
            {
                if ((child as RadioButton).IsChecked == true)
                {
                    account.gender = Convert.ToInt32((child as RadioButton).Tag);
                    Debug.WriteLine(account.gender);
                }
            }

            SortedList<string, string> errorMessage = Validate_Account(account);
            msgFirstName.Text = errorMessage.ContainsKey("firstNameError") ? errorMessage["firstNameError"] : "";
            msgLastName.Text = errorMessage.ContainsKey("lastNameError") ? errorMessage["lastNameError"] : "";
            msgPassword.Text = errorMessage.ContainsKey("passwordError") ? errorMessage["passwordError"] : "";
            msgAddress.Text = errorMessage.ContainsKey("addressError") ? errorMessage["addressError"] : "";
            msgPhone.Text = errorMessage.ContainsKey("phoneError") ? errorMessage["phoneError"] : "";
            msgAvatar.Text = errorMessage.ContainsKey("avatarError") ? errorMessage["avatarError"] : "";
            msgEmail.Text = errorMessage.ContainsKey("emailError") ? errorMessage["emailError"] : "";
            msgGender.Text = errorMessage.ContainsKey("genderError") ? errorMessage["genderError"] : "";
            msgBirthday.Text = errorMessage.ContainsKey("birthdayError") ? errorMessage["birthdayError"] : "";
            if (errorMessage.Count == 0)
            {
                var result = await accountService.RegisterAsync(account);
                ContentDialog contentDialog = new ContentDialog();
                if (result)
                {
                    contentDialog.Title = "Action success.";
                    contentDialog.Content = "Your account has been created.";
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(Pages.LoginPage));
                }
                else
                {
                    contentDialog.Title = "Action failed.";
                    contentDialog.Content = "Please try again.";
                }
                contentDialog.CloseButtonText = "Ok";
                await contentDialog.ShowAsync();
            }
        }

        private SortedList<string, string> Validate_Account(Account account)
        {
            SortedList<string, string> errorMessage = new SortedList<string, string>();
            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            string phonePattern = @"^(84|0[3|5|7|8|9])+([0-9]{8})$";
            string avatarPattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
            string emailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            if (string.IsNullOrEmpty(account.firstName))
            {
                errorMessage.Add("firstNameError", "First name is required.");
            }
            if (string.IsNullOrEmpty(account.lastName))
            {
                errorMessage.Add("lastNameError", "Last name is required.");
            }
            if (!Regex.IsMatch(account.password, passwordPattern))
            {
                errorMessage.Add("passwordError", "Password must be contain at least 8 characters, least 1 number and 1 letter.");
            }
            if (string.IsNullOrEmpty(account.address))
            {
                errorMessage.Add("addressError", "Address is required.");
            }
            if (!Regex.IsMatch(account.phone, phonePattern))
            {
                errorMessage.Add("phoneError", "Invalid phone number.");
            }
            if (!Regex.IsMatch(account.avatar, avatarPattern))
            {
                errorMessage.Add("avatarError", "Invalid avatar.");
            }
            if (!Regex.IsMatch(account.email, emailPattern))
            {
                errorMessage.Add("emailError", "Invalid email address.");
            }
            if (Convert.ToString(account.gender) == "0")
            {
                errorMessage.Add("genderError", "Gender is required.");
            }
            if (DateTime.Compare(Convert.ToDateTime(account.birthday), DateTime.Now) > 0 || account.birthday.Equals("1601-01-01") || account.birthday.Equals((DateTime.Now).ToString("yyyy-MM-dd")))
            {
                errorMessage.Add("birthdayError", "Invalid birthday.");
            }
            return errorMessage;
        }

        private void Handle_Reset(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPassword.Password = "";
            txtEmail.Text = "";
            txtIntroduction.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtAvatar.Text = "";
        }

        private void Handle_Login(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Pages.LoginPage));
        }
    }
}