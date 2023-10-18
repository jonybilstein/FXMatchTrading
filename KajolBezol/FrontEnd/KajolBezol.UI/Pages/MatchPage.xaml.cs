
using KajolBezol.UI.Services;
using KajolBezol.UI.ViewModels;

namespace KajolBezol.UI.Pages;
public partial class MatchPage : ContentPage
{


    public MatchPage()
    {

        InitializeComponent();
        this.BindingContext = Application.Current.Handler.MauiContext.Services.GetService<MatchPageViewModel>();
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = this.BindingContext as MatchPageViewModel;
        vm.RefreshUserProfile();

    }

    


}