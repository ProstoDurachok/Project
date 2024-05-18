using App2.Data;
using App2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit;
using System.ComponentModel;

namespace App2
{
    public partial class Contests : ContentPage
    {
        public int ID_Konkurschik;
        private string username;
        private string password;
        private List<Ispolnitel> ispolnitels;
        private List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int IdZakazchik;
        private string selectedCategory;
        private int user;

        public Contests(string username, string password, bool isFreelancer, int idZakazchik)
        {
            this.username = username;
            this.password = password;
            this.isFreelancer = isFreelancer;
            InitializeComponent();
            this.ispolnitels = new List<Ispolnitel>(); 
            this.zakazchiks = new List<Zakazchik>();
            this.Appearing += OnPageAppearing;
            CheckAuthentication();
            this.IdZakazchik = idZakazchik;
            AddButton.IsVisible = AuthManager.IsAuthenticated && !isFreelancer;
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
            await Navigation.PushAsync(new MainPage(username, password, isFreelancer, IdZakazchik));
        }

        private async void RegistrationButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorizationPage());
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void FreelancersButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Workers(username, password, isFreelancer, IdZakazchik));
        }

        private async void WorkButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Work(username, password, isFreelancer, IdZakazchik));
        }

        private async void HandleCategorySelection(string selectedCategory)
        {
            this.selectedCategory = selectedCategory; 
            MyStackLayout.Children.Clear();
            await LoadProjects(selectedCategory);
            await DisplayAlert("Выбранная категория", selectedCategory, "OK");
        }

        private async Task LoadCategories()
        {
            MyStack.Children.Clear();
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);
            List<Konkurs> projects = await databaseService.GetProjectsAsync();
            HashSet<string> categories = new HashSet<string>();

            foreach (Konkurs project in projects)
            {
                if (project.IdIspolnitel != 0)
                {
                    continue; 
                }
                categories.Add(project.Kategoria.ToString());
            }

            if (categories.Count > 0)
            {
                foreach (string category in categories)
                {
                    Label categoryLabel = new Label
                    {
                        Text = category,
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(0, 10),
                        TextColor = Color.Black,
                        BackgroundColor = Color.White,
                        Padding = new Thickness(10),
                        GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            HandleCategorySelection(category);
                        })
                    }
                }
                    };

                    MyStack.Children.Add(categoryLabel);
                }
            }
            else
            {
                Label noCategoriesLabel = new Label
                {
                    Text = "Категорий пока нет",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                MyStack.Children.Add(noCategoriesLabel);
            }
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            MyStackLayout.Children.Clear();
            await LoadProjects(selectedCategory);
        }

        private async void OnShowProjectsClicked(object sender, EventArgs e)
        {
            MyGrid.IsVisible = !MyGrid.IsVisible;

            if (MyGrid.IsVisible)
            {
                MyStackLayout.IsVisible = true;

                await LoadCategories();
            }
            else
            {
                MyStackLayout.IsVisible = true;

                await LoadProjects(selectedCategory);
            }
        }

        private async void acceptOrderButton_Clicked(object sender, EventArgs e, Konkurs projectItem)
        {
            if (projectItem != null)
            {
                if (projectItem.IdIspolnitel == 0)
                {
                    string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
                    DatabaseService databaseService = new DatabaseService(dbPath);

                    if (Application.Current.Properties.TryGetValue("ID_Ispolnitel", out object idIspolnitelObj) &&
                        idIspolnitelObj is int ID_Ispolnitel)
                    {
                        projectItem.IdIspolnitel = ID_Ispolnitel;

                        await databaseService.Savee(projectItem);

                        if (sender is Button button)
                        {
                            button.Text = "Отказаться от заказа";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "ID исполнителя не найден", "OK");
                    }
                }
                else
                {
                    bool result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите отказаться от заказа?", "Да", "Отмена");
                    if (result)
                    {
                        projectItem.IdIspolnitel = 0;
                        string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
                        DatabaseService databaseService = new DatabaseService(dbPath);
                        await databaseService.Savee(projectItem);

                        if (sender is Button button)
                        {
                            button.Text = "Принять заказ";
                        }
                    }
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Проект не найден", "OK");
            }
        }


        private async Task LoadProjects(string selectedCategory)
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);
            List<Konkurs> projects;

            if (selectedCategory != null)
            {
                projects = await databaseService.GetProjects(selectedCategory);
            }
            else
            {
                projects = await databaseService.GetProjectsAsync();
            }

            MyStackLayout.Children.Clear();

            bool projectsDisplayed = false;

            if (projects != null && projects.Count > 0)
            {
                foreach (Konkurs projectItem in projects)
                {
                    if (projectItem.IdIspolnitel != 0)
                    {
                        continue;
                    }

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
                        Text = "Принять заказ",
                        Margin = new Thickness(0, 10, 0, 0),
                        BackgroundColor = Color.FromHex("#007bff"),
                        TextColor = Color.White, 
                        CornerRadius = 5,
                        IsVisible = isFreelancer 

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
                        CornerRadius = 10,
                        Padding = new Thickness(10),
                        Margin = new Thickness(20),
                        Content = projectContainer,
                        BorderColor = Color.Black,
                        HasShadow = true, 
                        BackgroundColor = Color.White
                    };

                    MyStackLayout.Children.Add(frame);

                    projectsDisplayed = true;
                }

                MyStackLayout.IsVisible = true;
            }

            if (!projectsDisplayed)
            {
                Label noProjectsLabel = new Label
                {
                    Text = "Проектов пока нет",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                MyStackLayout.Children.Add(noProjectsLabel);

                MyStackLayout.IsVisible = true;
            }
        }


        private async void Prof(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
        }

        private async void Add(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddContest());
        }
    }
}
