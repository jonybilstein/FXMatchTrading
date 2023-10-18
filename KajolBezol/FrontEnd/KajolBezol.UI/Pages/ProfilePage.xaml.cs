using KajolBezol.UI.ViewModels;

namespace KajolBezol.UI.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        this.BindingContext = Application.Current.Handler.MauiContext.Services.GetService<ProfilePageViewModel>();

    }
    
}