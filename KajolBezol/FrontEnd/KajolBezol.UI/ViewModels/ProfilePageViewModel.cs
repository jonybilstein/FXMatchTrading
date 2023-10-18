using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KajolBezol.UI.Localization;
using KajolBezol.UI.Pages;
using KajolBezol.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KajolBezol.UI.ViewModels
{
    public partial class ProfilePageViewModel : ObservableObject
    {

        IServiceProvider _serviceProvider;

        public ProfilePageViewModel(KajolBezolServiceUI service, IServiceProvider serviceProvider)
        {
            UsePolicy = HttpUtility.HtmlDecode(service.UserProfile.UsePolicy);
            UserName = $"{AppResources.WelcomeTxt} <br /> <font color=\"black\">{service.UserProfile.User.FullName}</font>";

            this._serviceProvider = serviceProvider;
        }


        [ObservableProperty]
        string usePolicy;


        [ObservableProperty]
        string userName;


        [RelayCommand]
        async Task TapExit()
        {
            var matchPage = _serviceProvider.GetService<MatchPageViewModel>();
            matchPage._timer.Stop();

            var tradingPage = _serviceProvider.GetService<TradingPageViewModel>();
            tradingPage._timer.Stop();

            var login = _serviceProvider.GetService<LoginPage>();
            await Application.Current.MainPage.Navigation.PushModalAsync(login);
        }




    }
}
