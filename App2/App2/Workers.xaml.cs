using App2.Data;
using App2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class Workers : ContentPage
    {
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int IdZakazchik;
        private int user;
        private string username;
        private string password;

        public Workers(string username, string password, bool isFreelancer, int idZakazchik)
        {
            this.isFreelancer = isFreelancer;
            this.username = username;
            this.password = password;
            InitializeComponent();
            CheckAuthentication();
            LoadProjects();
            IdZakazchik = idZakazchik;
        }

        private async void Prof(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
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

        private async void WorkButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Work(username, password, isFreelancer, IdZakazchik));
        }

        private async void RegistrationButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorizationPage());
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void ContestsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Contests(username, password, isFreelancer, IdZakazchik));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckAuthentication();
        }

        private async Task LoadProjects()
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            List<Zakaz> projects;

            projects = await databaseService.GetProjectsFromDatabase();

            if (projects != null && projects.Count > 0)
            {
                Dictionary<int, int> zakazchikCounts = new Dictionary<int, int>();

                foreach (Zakaz projectItem in projects)
                {


                    if (zakazchikCounts.ContainsKey(projectItem.IdZakazchik))
                    {
                        zakazchikCounts[projectItem.IdZakazchik]++;
                    }
                    else
                    {
                        zakazchikCounts.Add(projectItem.IdZakazchik, 1);
                    }
                }

                var top3Zakazchiks = zakazchikCounts.OrderByDescending(pair => pair.Value)
                                                     .Take(3);

                StackLayout dataContainer = new StackLayout
                {
                    Margin = new Thickness(10),
                    Spacing = 5
                };

                foreach (var zakazchikPair in top3Zakazchiks)
                {
                    Zakazchik customer = await databaseService.GetItemmsAsync(zakazchikPair.Key);

                    if (customer != null)
                    {
                        string customerName = customer.FirstName + " " + customer.SecondName + " " + customer.Patronymic;
                        int projectCount = zakazchikPair.Value;

                        StackLayout dataRow = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Margin = new Thickness(0, 5, 0, 5)
                        };

                        Frame frame = new Frame
                        {
                            CornerRadius = 10,
                            Padding = new Thickness(10),
                            Margin = new Thickness(20),
                            Content = dataRow,
                            BorderColor = Color.LightGray,
                            HasShadow = true,
                            BackgroundColor = Color.White
                        };

                        Image profileImage = new Image
                        {
                            Source = "Prof.png",
                            HorizontalOptions = LayoutOptions.Start,
                            WidthRequest = 70,
                            HeightRequest = 70
                        };

                        Label nameLabel = new Label
                        {
                            Text = customerName,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            TextColor = Color.Black,
                            VerticalOptions = LayoutOptions.Center
                        };

                        Label projectCountLabel = new Label
                        {
                            Text = "Количество проектов: " + projectCount,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                            TextColor = Color.Gray,
                            VerticalOptions = LayoutOptions.Center
                        };

                        dataRow.Children.Add(profileImage);
                        dataRow.Children.Add(nameLabel);
                        dataRow.Children.Add(projectCountLabel);

                        dataContainer.Children.Add(frame);
                    }
                }

                YourStackLayout.Children.Clear();

                YourStackLayout.Children.Add(dataContainer);
            }
            else
            {
                Label noDataLabel = new Label
                {
                    Text = "Пока нет лучших данных",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                YourStackLayout.Children.Clear();

                YourStackLayout.Children.Add(noDataLabel);
            }
        }






        private async void OnProfile(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
        }
    }
}
