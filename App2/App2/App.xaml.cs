using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App2.Data;
using App2.Models;
using System.IO;
using SQLite;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace App2
{
    public partial class App : Application
    {
        private string username;
        private string userId;
        private string password;
        public static SQLiteAsyncConnection Database;
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int ID_zakazchik;
        private int user;

        public App()
        {
            InitializeComponent();
            InitializeDatabase();
            MainPage = new NavigationPage(new LoginPage());

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                MainPage = new NavigationPage(new MainPage(username, password, isFreelancer, ID_zakazchik));
            }
            else
            {
                MainPage = new NavigationPage(new MainPage(username, password, isFreelancer, ID_zakazchik));
            }
        }

        private async void InitializeDatabase()
        {
            Database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database1.db"));
            await Database.CreateTableAsync<Ispolnitel>().ConfigureAwait(false);
            await Database.CreateTableAsync<Zakazchik>().ConfigureAwait(false);
            await Database.CreateTableAsync<Zakaz>().ConfigureAwait(false);
            await Database.CreateTableAsync<Konkurs>().ConfigureAwait(false);


        }

        protected override void OnStart()
        {
            RestoreLoginState(MainPage.Navigation);
        }

        protected override void OnSleep()
        {
            SaveLoginState();
        }

        protected override void OnResume()
        {
            RestoreLoginState(MainPage.Navigation);
        }
        private async void RestoreLoginState(INavigation navigation)
        {
            if (Application.Current.Properties.ContainsKey("IsLoggedIn") && (bool)Application.Current.Properties["IsLoggedIn"])
            {
                string username = Application.Current.Properties["Username"].ToString();
                string password = Application.Current.Properties["Password"].ToString();
                bool isFreelancer = (bool)Application.Current.Properties["IsFreelancer"];
                int userId = 0;
                int user = 0;

                if (Application.Current.Properties.ContainsKey("ID_zakazchik"))
                {
                    userId = (int)Application.Current.Properties["ID_zakazchik"];
                }
                if (Application.Current.Properties.ContainsKey("ID_Ispolnitel"))
                {
                    user = (int)Application.Current.Properties["ID_Ispolnitel"];
                }

                AuthManager.IsAuthenticated = true;
                await navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, userId, user));
            }
            else
            {
                await navigation.PushAsync(new MainPage(username, password, isFreelancer, ID_zakazchik));
            }
        }


        private void SaveLoginState()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "login_state.txt");
            string loginData = $"{username},{password},{isFreelancer},{userId},{user}";
            File.WriteAllText(filePath, loginData);
        }




    }
}
