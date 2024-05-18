using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App2.Models;
using System;
using App2.Data;
using System.Collections.Generic;
using System.Linq;

namespace App2
{
    public partial class Profile : ContentPage
    {
        private string username;
        private string password;
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int ID_zakazchik;
        private int user;

        public Profile(List<Ispolnitel> ispolnitels, string username, string password, bool isFreelancer, List<Zakazchik> zakazchiks, int IdZakazchik, int user)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.ispolnitels = ispolnitels;
            this.zakazchiks = zakazchiks;
            this.isFreelancer = isFreelancer;
            this.ID_zakazchik = IdZakazchik;
            this.user = user;
            a.IsVisible = AuthManager.IsAuthenticated && isFreelancer;
            aa.IsVisible = AuthManager.IsAuthenticated && !isFreelancer;
            Skil.IsVisible = AuthManager.IsAuthenticated && isFreelancer;
            Opi.IsVisible = AuthManager.IsAuthenticated && isFreelancer;
            CheckAuthentication();
            LoadUserProfileAsync();
        }

        private async void LoadUserProfileAsync()
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            if (isFreelancer)
            {
                var currentUser = await databaseService.GetLoggedInUserAsync(username, password);

                if (currentUser.FirstName != null || currentUser.SecondName != null || currentUser.Patronymic != null)
                {
                    FirstNameLabel.Text = currentUser.FirstName;
                    LastNameLabel.Text = currentUser.SecondName;
                    PatronymicLabel.Text = currentUser.Patronymic;
                    Opis.Text = currentUser.Opis;
                    Skils.Text = currentUser.Skils;
                }
                else
                {
                    bool answer = await DisplayAlert("Профиль", "Пожалуйста, заполните информацию в профиле", "Заполнить", "Отмена");
                    if (answer)
                    {
                        await Navigation.PushAsync(new ProfileEditorPage(ispolnitels, zakazchiks, username, password, isFreelancer, ID_zakazchik));
                    }
                }
            }
            else
            {
                var currentUser = await databaseService.GetLogedInUserAsync(username, password);

                if (currentUser.FirstName != null || currentUser.SecondName != null || currentUser.Patronymic != null)
                {
                    FirstNameLabel.Text = currentUser.FirstName;
                    LastNameLabel.Text = currentUser.SecondName;
                    PatronymicLabel.Text = currentUser.Patronymic;
                }
                else
                {
                    bool answer = await DisplayAlert("Профиль", "Пожалуйста, заполните информацию в профиле", "Заполнить", "Отмена");
                    if (answer)
                    {
                        await Navigation.PushAsync(new ProfileEditorPage(ispolnitels, zakazchiks, username, password, isFreelancer, ID_zakazchik));
                    }
                }
            }
        }

        private void CheckAuthentication()
        {
            if (AuthManager.IsAuthenticated)
            {
                ProfileButton.IsVisible = true;
                Registration.IsVisible = false;
                Login.IsVisible = false;
            }
            else
            {
                ProfileButton.IsVisible = false;
                Registration.IsVisible = true;
                Login.IsVisible = true;
            }
        }

        private async void Image_Tapped(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new MainPage(username, password, isFreelancer, ID_zakazchik));
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void WorkButton_Clicked(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new Work(username, password, isFreelancer, ID_zakazchik));
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void RegistrationButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorizationPage());
        }

        private async void ViewTasksButton_Clicked(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new MyTasksPage());
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void ViewTasksButton(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new MyTasks());
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["IsLoggedIn"] = false;
            Application.Current.Properties.Remove("Username");
            Application.Current.Properties.Remove("Password");
            Application.Current.Properties.Remove("IsFreelancer");
            Application.Current.Properties.Remove("ID_zakazchik");
            Application.Current.Properties.Remove("ID_Ispolnitel");

            await Application.Current.SavePropertiesAsync();

            if (App.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.Navigation.InsertPageBefore(new LoginPage(), navigationPage.Navigation.NavigationStack.First());
                await navigationPage.PopToRootAsync();
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        private async void FreelancersButton_Clicked(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new Workers(username, password, isFreelancer, ID_zakazchik));
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void ContestsButton_Clicked(object sender, EventArgs e)
        {
            if (ProfileIsFilled())
            {
                await Navigation.PushAsync(new Contests(username, password, isFreelancer, ID_zakazchik));
            }
            else
            {
                await DisplayAlert("Профиль", "Пожалуйста, сначала заполните информацию в профиле", "ОК");
            }
        }

        private async void Update(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfileEditorPage(ispolnitels, zakazchiks, username, password, isFreelancer, ID_zakazchik));
        }

        private bool ProfileIsFilled()
        {
            return FirstNameLabel.Text != null && LastNameLabel.Text != null && PatronymicLabel.Text != null;
        }
    }
}
