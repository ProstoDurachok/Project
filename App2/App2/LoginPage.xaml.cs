using App2.Models;
using System;
using System.Threading.Tasks;
using App2.Data;
using Xamarin.Forms;
using System.Collections.Generic;

namespace App2
{
    public partial class LoginPage : ContentPage
    {
        public event EventHandler AuthenticationSuccess;
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int ID_zakazchik;
        private int ID;


        public LoginPage()
        {
            InitializeComponent();

        }
        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radioButton = (RadioButton)sender;

            if (radioButton == freelancerRadioButton && e.Value)
            {
                employerRadioButton.IsChecked = false;
            }
            else if (radioButton == employerRadioButton && e.Value)
            {
                freelancerRadioButton.IsChecked = false;
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorizationPage());
        }

        private async void OnForgotTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Forgot());
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

        private async void Login(object sender, EventArgs e)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            string username = Log.Text;
            string password = Password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            if (!IsAlphaNumeric(username) || !IsAlphaNumeric(password))
            {
                await DisplayAlert("Ошибка", "Имя пользователя и пароль должны содержать только буквы и цифры", "OK");
                return;
            }

            Ispolnitel user = null;
            Zakazchik user1 = null;

            bool isFreelancer = false;

            if (freelancerRadioButton.IsChecked)
            {
                user = await databaseService.GetUserByLoginAsync(Log.Text);
                isFreelancer = true;
                AuthManager.IsFreelancer = true;
            }
            else if (employerRadioButton.IsChecked)
            {
                user1 = await databaseService.GetUserUsernameAsync(Log.Text);
                isFreelancer = false;
                AuthManager.IsFreelancer = true;
            }

            if ((user1 != null && user1.Passwordd == Password.Text) || (user != null && user.Passwordd == Password.Text))
            {
                AuthManager.IsAuthenticated = true;

                Application.Current.Properties["IsLoggedIn"] = true;
                Application.Current.Properties["Username"] = username;
                Application.Current.Properties["Password"] = password;
                Application.Current.Properties["IsFreelancer"] = isFreelancer;

                if (!isFreelancer)
                {
                    Application.Current.Properties["ID_zakazchik"] = user1.ID_zakazchik;
                }
                else

                {
                    Application.Current.Properties["ID_Ispolnitel"] = user.ID_Ispolnitel;

                }

                await Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, ID_zakazchik, ID));
            }
            else
            {
                await DisplayAlert("Вход неудачен", "Неправильный логин или пароль", "OK");
            }
        }








    }
}
