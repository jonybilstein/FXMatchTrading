using KajolBezol.UI.ViewModels;

namespace KajolBezol.UI.Pages;

public partial class TradingPage : ContentPage
{

    public TradingPage()
    {

        InitializeComponent();
        this.BindingContext = Application.Current.Handler.MauiContext.Services.GetService<TradingPageViewModel>();

    }



    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var vm = this.BindingContext as TradingPageViewModel;
        vm.Clean();
        
    }

    protected async override void OnDisappearing()
    {
        base.OnDisappearing();
        var vm = this.BindingContext as TradingPageViewModel;
        vm.Clean();

    }


}