using App2.Data;
using App2.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class AddContest : ContentPage
    {
        private int ID_zakazchik;
        List<Zakaz> zakaz;
        List<Zakazchik> zakazchik;
        List<Konkurs> konkurs;

        public AddContest()
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

        private bool Numeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
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

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            int ID_zakazchik = Convert.ToInt32(Application.Current.Properties["ID_zakazchik"]);

            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text) || string.IsNullOrWhiteSpace(AmountEntry.Text) ||
                string.IsNullOrWhiteSpace(DurationEntry.Text) || string.IsNullOrWhiteSpace(Kategoria.Text))
            {
                await DisplayAlert("Error", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            async Task CheckDescription()
            {
                string description = DescriptionEditor.Text;

                if (description.Length > 0)
                {
                    if (!Regex.IsMatch(description, @"^(?:[\p{L}\d]+(?:[\s,.;:!?]+\b)?)+$"))
                    {
                        await DisplayAlert("Error", "В описании разрешены только буквы, цифры, пробелы и знаки препинания после слов, без нескольких пробелов подряд", "OK");
                        return;
                    }
                }
            }


            if (!Numeric(AmountEntry.Text))
            {
                await DisplayAlert("Error", "Цена должна содержать только цифры", "OK");
                return;
            }

            if (!Numeric(DurationEntry.Text))
            {
                await DisplayAlert("Error", "Время должно содержать только цифры", "OK");
                return;
            }

            if (Kategoria.Text.Length > 30)
            {
                if (!IsAlpha(Kategoria.Text))
                {
                    await DisplayAlert("Error", "Только буквы", "OK");
                    return;
                }
            }

            var newZakaz = new Konkurs
            {
                IdZakazchik = ID_zakazchik,
                Opisaniye = DescriptionEditor.Text,
                Price = int.Parse(AmountEntry.Text),
                Vremya = int.Parse(DurationEntry.Text),
                Kategoria = Kategoria.Text
            };

            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);
            await databaseService.Save(newZakaz);
            await DisplayAlert("Success", "Заказ успешно добавлен", "OK");
            await Navigation.PopAsync();
        }

    }
}
