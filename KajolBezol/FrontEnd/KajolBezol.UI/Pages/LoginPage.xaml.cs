namespace KajolBezol.UI.Pages;
using KajolBezol.UI.Permissions;
using KajolBezol.UI.ViewModels;
using Microsoft.Maui.ApplicationModel;


public partial class LoginPage : ContentPage
{



    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override  async void OnAppearing()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<PostNotifications>();
        if (status == PermissionStatus.Unknown || status == PermissionStatus.Denied)
        {
            await Permissions.RequestAsync<PostNotifications>();
        }


    }


}