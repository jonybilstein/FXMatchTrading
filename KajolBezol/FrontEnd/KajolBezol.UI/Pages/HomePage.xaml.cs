using KajolBezol.UI.ViewModels;

namespace KajolBezol.UI.Pages;

public partial class HomePage : TabbedPage
{
    

    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

       
    }


}