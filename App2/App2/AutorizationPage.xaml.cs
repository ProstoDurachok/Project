using System;
using Xamarin.Forms;
using App2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using System.Text;
using App2.Data;


namespace App2
{
    public partial class AutorizationPage : ContentPage
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private bool IsAlphaNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text.Trim();
            string email = EmailEntry.Text.Trim();
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            if (!IsAlphaNumeric(username))
            {
                await DisplayAlert("Ошибка", "Имя пользователя должно содержать только буквы и цифры", "OK");
                return;
            }
            if (!IsValidEmail(email))
            {
                await DisplayAlert("Ошибка", "Некорректный адрес электронной почты", "OK");
                return;
            }

            if (!IsAlphaNumeric(password))
            {
                await DisplayAlert("Ошибка", "Пароль должен содержать только буквы и цифры", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Ошибка", "Пароль и подтверждение пароля не совпадают", "OK");
                return;
            }

            bool userExists = await CheckUserExists(username);
            if (userExists)
            {
                await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "OK");
                return;
            }

            bool user = await CheckUser(username);
            if (user)
            {
                await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "OK");
                return;
            }

            bool isFreelancer = FreelancerRadioButton.IsChecked;
            if (isFreelancer)
            {
                Ispolnitel newUser = new Ispolnitel
                {
                    Loginad = username,
                    Email = email,
                    Passwordd = password,
                };
                await App.Database.InsertAsync(newUser);
            }
            else
            {
                Zakazchik newUser = new Zakazchik
                {
                    Loginad = username,
                    Email = email,
                    Passwordd = password,
                };
                await App.Database.InsertAsync(newUser);
            }

            UsernameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;
            FreelancerRadioButton.IsChecked = false;
            EmployerRadioButton.IsChecked = false;

            await DisplayAlert("Успех", "Пользователь успешно зарегистрирован", "OK");
        }
        private async Task<bool> CheckUserExists(string username)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            bool userExists = await databaseService.UserExists(username);
            return userExists;
        }

        private async Task<bool> CheckUser(string username)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            bool user = await databaseService.User(username);
            return user;
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool isFreelancer;

        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radioButton = (RadioButton)sender;

            if (radioButton == FreelancerRadioButton && e.Value)
            {
                isFreelancer = true;
                EmployerRadioButton.IsChecked = false;
            }
            else if (radioButton == EmployerRadioButton && e.Value)
            {
                isFreelancer = false;
                FreelancerRadioButton.IsChecked = false;
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
