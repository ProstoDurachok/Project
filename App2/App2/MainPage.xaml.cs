using App2.Data;
using App2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        private string username;
        private string password;
        List<Ispolnitel> ispolnitels;
        List<Zakazchik> zakazchiks;
        private bool isFreelancer;
        private int IdZakazchik;
        private int user;
        private int IdIspolnitel;

        public MainPage(string username, string password,bool isFreelancer, int idZakazchik)
        {
            this.username = username;
            this.password = password;
            this.isFreelancer = isFreelancer;
            InitializeComponent();
            CheckAuthentication();
            LoadProjects();
            IdIspolnitel = IdIspolnitel;
            IdZakazchik = idZakazchik;
        }
        private async void Prof(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile( ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
        }
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage()); 
        }

        private async void RegistrationButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorizationPage());
        }


        private async void WorkButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Work(username,password, isFreelancer, IdZakazchik));
        }

        private async void FreelancersButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Workers(username,password, isFreelancer, IdZakazchik));
        }

        private async void ContestsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Contests(username,password, isFreelancer, IdZakazchik));
        }

        private async Task LoadProjects()
        {
            string dbPath = "/data/user/0/com.companyname.app2/files/.local/share/database1.db";
            DatabaseService databaseService = new DatabaseService(dbPath);

            List<Zakaz> projects;

            projects = await databaseService.GetProjectsFromDatabase();

            if (projects != null && projects.Count > 0)
            {
                Dictionary<int, int> ispolnitelCounts = new Dictionary<int, int>();

                foreach (Zakaz projectItem in projects)
                {
                    if (projectItem.IdIspolnitel == 0)
                    {
                        continue;
                    }

                    if (ispolnitelCounts.ContainsKey(projectItem.IdIspolnitel))
                    {
                        ispolnitelCounts[projectItem.IdIspolnitel]++;
                    }
                    else
                    {
                        ispolnitelCounts.Add(projectItem.IdIspolnitel, 1);
                    }
                }

                var top3Ispolnitels = ispolnitelCounts.OrderByDescending(pair => pair.Value)
                                                        .Take(3);

                StackLayout dataContainer = new StackLayout
                {
                    Margin = new Thickness(10),
                    Spacing = 5
                };

                foreach (var ispolnitelPair in top3Ispolnitels)
                {
                    Ispolnitel ispolnitel = await databaseService.GetItemAsync(ispolnitelPair.Key);

                    if (ispolnitel != null)
                    {
                        string ispolnitelName = ispolnitel.FirstName + " " + ispolnitel.SecondName + " " + ispolnitel.Patronymic;
                        int projectCount = ispolnitelPair.Value;

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
                            Text = ispolnitelName,
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



        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckAuthentication();
        }

        private void CheckAuthentication()
        {
            if (AuthManager.IsAuthenticated)
            {
                ProfileButton.IsVisible = true;
                Login.IsVisible = false;
                Registration.IsVisible = false;
            }
            else
            {
                ProfileButton.IsVisible = false;
                Login.IsVisible = true;
                Registration.IsVisible = true;
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var loginPage = new LoginPage();
            loginPage.AuthenticationSuccess += LoginPage_AuthenticationSuccess;
            await Navigation.PushAsync(loginPage); 
        }

        private void LoginPage_AuthenticationSuccess(object sender, EventArgs e)
        {
            ProfileButton.IsVisible = true;
            Login.IsVisible = false;
            Registration.IsVisible = false;
        }

        private async void OnProfile(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile(ispolnitels, username, password, isFreelancer, zakazchiks, IdZakazchik, user));
        }
        }
    }
