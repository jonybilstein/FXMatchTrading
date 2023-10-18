using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase;
using KajolBezol.UI.Localization;
using KajolBezol.UI.Model;
using KajolBezol.UI.Pages;
using KajolBezol.UI.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.Firebase.CloudMessaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KajolBezol.UI.ViewModels
{

    public partial class LoginPageViewModel : ObservableObject
    {

        KajolBezolServiceUI _kajolBezolUIService;
        UserProfile _userProfile;
        IServiceProvider _serviceProvider;
        public LoginPageViewModel(KajolBezolServiceUI kajolBezolUIService, IServiceProvider serviceProvider)
        {
            this._kajolBezolUIService = kajolBezolUIService;
            this._serviceProvider = serviceProvider;

        }

        [ObservableProperty]
        public bool isRunning;

        [ObservableProperty]
        string user;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string errorMessage;


        [RelayCommand]
        async Task Login()
        {
            IsRunning = true;

            try
            {

                var versionFromAPI = await _kajolBezolUIService.GetAppVersion();

                if (versionFromAPI > _kajolBezolUIService._appSettings.AppVersion)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.NewVersionTitle, AppResources.NewVersionBody, "OK");
                    IsRunning = false;
                    return;
                }

                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();



                UserLogin user = new UserLogin()
                {
                    UserName = User,
                    PassCode = Password,
                    DeviceKey = token
                };

                _userProfile = await _kajolBezolUIService.PopulateProfile(user);

                if (_userProfile != null)
                {


                    var homepage = _serviceProvider.GetService<HomePage>();
                    var matchPage = _serviceProvider.GetService<MatchPageViewModel>();
                    matchPage._timer.Start();

                    var tradePage = _serviceProvider.GetService<TradingPageViewModel>();
                    tradePage._timer.Start();

                    await Application.Current.MainPage.Navigation.PushModalAsync(homepage);
                }
                else
                {
                    ErrorMessage = Localization.AppResources.InvalidPassword;
                }


            }

            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, AppResources.ErrorDescription, "OK");
            }

            IsRunning = false;



        }

    }



}
