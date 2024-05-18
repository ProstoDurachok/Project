using App2.Data;
using App2.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace App2
{
    public partial class ProfileEditorPage : ContentPage
    {
        private string username;
        private string password;
        private bool isFreelancer;
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private int IdZakazchik;
        private int user;


        public ProfileEditorPage(List<Ispolnitel> ispolnitels, List<Zakazchik> zakazchiks, string username, string password, bool isFreelancer, int IdZakazchik)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.isFreelancer = isFreelancer;
            this.IdZakazchik = IdZakazchik;
            LoadUserProfileAsync();
            Opis.IsVisible = AuthManager.IsAuthenticated && isFreelancer;
            Skils.IsVisible = AuthManager.IsAuthenticated && isFreelancer;

        }

        private async void LoadUserProfileAsync()
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            if (isFreelancer)
            {
                var currentUser = await databaseService.GetLoggedInUserAsync(username, password);
                if (currentUser != null)
                {
                    FirstNameEntry.Text = currentUser.FirstName;
                    SecondNameEntry.Text = currentUser.SecondName;
                    PatronymicEntry.Text = currentUser.Patronymic;
                    Opis.Text = currentUser.Opis;
                    Skils.Text = currentUser.Skils;
                }
            }
            else
            {
                var currentUser = await databaseService.GetLogedInUserAsync(username, password);
                if (currentUser != null)
                {
                    FirstNameEntry.Text = currentUser.FirstName;
                    SecondNameEntry.Text = currentUser.SecondName;
                    PatronymicEntry.Text = currentUser.Patronymic;
                }
            }
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

        private bool IsAlpha(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }
        bool IsValidDescription(string text)
        {
            if (text.Length > 0)
            {
                if (!Regex.IsMatch(text, @"^(?:[\p{L}\d]+(?:[\s,.;:!?]+\b)?)+$"))
                {
                    return false;
                }
            }
            return true;

        }


        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
           


            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            if (isFreelancer)
            {
                var currentUser = await databaseService.GetLoggedInUserAsync(username, password);
                if (currentUser != null)
                {

                    if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
                       string.IsNullOrWhiteSpace(SecondNameEntry.Text) ||
                       string.IsNullOrWhiteSpace(PatronymicEntry.Text) ||
                       string.IsNullOrWhiteSpace(Opis.Text) ||
                       string.IsNullOrWhiteSpace(Skils.Text))
                    {
                        await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                        return;
                    }

                    if (FirstNameEntry.Text.Length > 30 || SecondNameEntry.Text.Length > 30 || PatronymicEntry.Text.Length > 30 ||
                        Opis.Text.Length > 100 || Skils.Text.Length > 100)
                    {
                        await DisplayAlert("Ошибка", "Длина полей должна быть не более 30 символов (имя, фамилия, отчество) и 100 символов (описание, скилы)", "OK");
                        return;
                    }

                    if (!IsAlpha(FirstNameEntry.Text) ||
                        !IsAlpha(SecondNameEntry.Text) ||
                        !IsAlpha(PatronymicEntry.Text) ||
                        !IsValidDescription(Opis.Text) ||
                        !IsValidDescription(Skils.Text))
                    {
                        await DisplayAlert("Ошибка", "Поля Имя, Фамилия, Отчество могут содержать только буквы Описание и Скиллы могут содержать только буквы и цифры, пробел только один после сло как и запятые", "OK");
                        return;
                    }

                    currentUser.FirstName = FirstNameEntry.Text;
                    currentUser.SecondName = SecondNameEntry.Text;
                    currentUser.Patronymic = PatronymicEntry.Text;
                    currentUser.Opis = Opis.Text;
                    currentUser.Skils = Skils.Text;
                    await databaseService.UpdateUserProfile(currentUser);
                }
            }
            else
            {
                var currentUser = await databaseService.GetLogedInUserAsync(username, password);
                if (currentUser != null)
                {
                    if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
                        string.IsNullOrWhiteSpace(SecondNameEntry.Text) ||
                        string.IsNullOrWhiteSpace(PatronymicEntry.Text))
                    {
                        await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                        return;
                    }

                    if (FirstNameEntry.Text.Length > 30 || SecondNameEntry.Text.Length > 30 || PatronymicEntry.Text.Length > 30)
                    {
                        await DisplayAlert("Ошибка", "Длина полей должна быть не более 30 символов (имя, фамилия, отчество) и 100 символов (описание, скилы)", "OK");
                        return;
                    }

                    if (!IsAlpha(FirstNameEntry.Text) ||
                        !IsAlpha(SecondNameEntry.Text) ||
                        !IsAlpha(PatronymicEntry.Text))
                    {
                        await DisplayAlert("Ошибка", "Поля могут содержать только буквы и цифры", "OK");
                        return;
                    }
                    currentUser.FirstName = FirstNameEntry.Text;
                    currentUser.SecondName = SecondNameEntry.Text;
                    currentUser.Patronymic = PatronymicEntry.Text;
                    await databaseService.UpateUserProfile(currentUser);
                }
            }

            await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
        }
    }
}
