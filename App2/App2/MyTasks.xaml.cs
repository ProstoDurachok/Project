using App2.Data;
using App2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class MyTasks : ContentPage
    {
        private int ID_Ispolnitel;
        private List<Zakaz> ispolnitels;
        private List<Konkurs> zakazchiks;

        public MyTasks()
        {
            InitializeComponent();
            this.ispolnitels = new List<Zakaz>();
            this.zakazchiks = new List<Konkurs>();
            ID_Ispolnitel = Convert.ToInt32(Application.Current.Properties["ID_zakazchik"]);
            LoadProjects(ID_Ispolnitel);
            LoadProject(ID_Ispolnitel);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MyStackLayout.Children.Clear();
            await LoadProjects(ID_Ispolnitel);
            await LoadProject(ID_Ispolnitel);
        }

        private async void acceptOrderButton_Clicked(object sender, EventArgs e, Zakaz projectItemCopy)
        {
            if (projectItemCopy != null)
            {
                string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
                DatabaseService databaseService = new DatabaseService(dbPath);
                await databaseService.DeleteAsync(projectItemCopy);
                MyStackLayout.Children.Clear();

                await LoadProject(ID_Ispolnitel);
                await databaseService.SaveAsync(projectItemCopy);
            }
        }


        private async void acceptOrderButton_Clicke(object sender, EventArgs e, Konkurs projecttem)
        {
            if (projecttem != null)
            {
                string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
                DatabaseService databaseService = new DatabaseService(dbPath);
                await databaseService.Delete(projecttem);
                MyStackLayout.Children.Clear();

                await LoadProject(ID_Ispolnitel);
                await databaseService.Savee(projecttem);
            }
        }


        private async Task LoadProjects(int ID_Ispolnitel)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);
            List<Konkurs> projects = await databaseService.Gesync(ID_Ispolnitel);
            MyStack.Children.Clear();
            bool projectsDisplayed = false;

            if (projects != null && projects.Count > 0)
            {
                Label konkursHeader = new Label
                {
                    Text = "Конкурсы",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                    Margin = new Thickness(10, 10, 10, 0)
                };

                MyStack.Children.Add(konkursHeader);

                foreach (Konkurs projectItem in projects)
                {
                    StackLayout projectLayout = new StackLayout
                    {
                        Spacing = 8,
                    };

                    Zakazchik customer = await databaseService.GetItemmsAsync(projectItem.IdZakazchik);

                    Label nameLabel = new Label
                    {
                        Text = "Имя заказчика: " + customer.FirstName + " " + customer.SecondName + " " + customer.Patronymic,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label descriptionLabel = new Label
                    {
                        Text = "Описание: " + projectItem.Opisaniye,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label vremyaLabel = new Label
                    {
                        Text = "Время: " + projectItem.Vremya,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label priceLabel = new Label
                    {
                        Text = "Цена: " + projectItem.Price,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label kategoriaLabel = new Label
                    {
                        Text = "Категория: " + projectItem.Kategoria,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Button acceptOrderButto = new Button
                    {
                        Text = "Отказаться от закааз",
                        Margin = new Thickness(0, 10, 0, 0),
                        BackgroundColor = Color.FromHex("#007bff"),
                        TextColor = Color.White,
                        CornerRadius = 5,

                    };

                    var projecttem = projectItem;
                    acceptOrderButto.Clicked += (sender, e) => acceptOrderButton_Clicke(sender, e, projecttem);

                    projectLayout.Children.Add(nameLabel);
                    projectLayout.Children.Add(descriptionLabel);
                    projectLayout.Children.Add(vremyaLabel);
                    projectLayout.Children.Add(priceLabel);
                    projectLayout.Children.Add(kategoriaLabel);
                    projectLayout.Children.Add(acceptOrderButto);


                    StackLayout projectContainer = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(10, 0, 0, 0),
                        Spacing = 5
                    };

                    projectContainer.Children.Add(projectLayout);

                    Frame frame = new Frame
                    {
                        BackgroundColor = Color.White,
                        CornerRadius = 10,
                        Padding = new Thickness(10),
                        Margin = new Thickness(20),
                        Content = projectContainer
                    };

                    MyStack.Children.Add(frame);

                    projectsDisplayed = true;
                }

                MyStackLayout.IsVisible = true;
            }

            if (!projectsDisplayed)
            {
                Label noProjectsLabel = new Label
                {
                    Text = "Конкурсов пока нет",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                MyStack.Children.Add(noProjectsLabel);
            }
        }


        private async Task LoadProject(int ID_Ispolnitel)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);
            List<Zakaz> projects = await databaseService.Ge(ID_Ispolnitel);
            MyStackLayout.Children.Clear();

            if (projects != null && projects.Count > 0)
            {
                Label zakazHeader = new Label
                {
                    Text = "Заказы",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                    Margin = new Thickness(10, 10, 10, 0)
                };

                MyStackLayout.Children.Add(zakazHeader);

                foreach (Zakaz projectItem in projects)
                {
                    StackLayout projectLayout = new StackLayout
                    {
                        Spacing = 8,
                    };

                    Zakazchik customer = await databaseService.GetItemmsAsync(projectItem.IdZakazchik);

                    Label nameLabel = new Label
                    {
                        Text = "Имя заказчика: " + customer.FirstName + " " + customer.SecondName + " " + customer.Patronymic,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label descriptionLabel = new Label
                    {
                        Text = "Описание: " + projectItem.Opisaniye,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label vremyaLabel = new Label
                    {
                        Text = "Время: " + projectItem.Vremya,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label priceLabel = new Label
                    {
                        Text = "Цена: " + projectItem.Price,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Label kategoriaLabel = new Label
                    {
                        Text = "Категория: " + projectItem.Kategoria,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        TextColor = Color.Black,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Button acceptOrderButton = new Button
                    {
                        Text = "ОТказаться от заказа",
                        Margin = new Thickness(0, 10, 0, 0),
                        BackgroundColor = Color.FromHex("#007bff"),
                        TextColor = Color.White,
                        CornerRadius = 5,

                    };

                    var projectItemCopy = projectItem;
                    acceptOrderButton.Clicked += (sender, e) => acceptOrderButton_Clicked(sender, e, projectItemCopy);

                    projectLayout.Children.Add(nameLabel);
                    projectLayout.Children.Add(descriptionLabel);
                    projectLayout.Children.Add(vremyaLabel);
                    projectLayout.Children.Add(priceLabel);
                    projectLayout.Children.Add(kategoriaLabel);
                    projectLayout.Children.Add(acceptOrderButton);


                    StackLayout projectContainer = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(10, 0, 0, 0),
                        Spacing = 5
                    };

                    projectContainer.Children.Add(projectLayout);

                    Frame frame = new Frame
                    {
                        BackgroundColor = Color.White,
                        CornerRadius = 10,
                        Padding = new Thickness(10),
                        Margin = new Thickness(20),
                        Content = projectContainer,
                        BorderColor = Color.Black
                    };

                    MyStackLayout.Children.Add(frame);
                }

                MyStackLayout.IsVisible = true;
            }
            else
            {
                Label noProjectsLabel = new Label
                {
                    Text = "Заказов пока нет",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                MyStackLayout.Children.Add(noProjectsLabel);
            }
        }
    }
}
