using KajolBezol.UI.Pages;
using KajolBezol.UI.Services;
using KajolBezol.UI.ViewModels;

namespace KajolBezol.UI
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var loginPage = serviceProvider.GetService(typeof(LoginPage)) as LoginPage;
            
            MainPage = loginPage;
        }
    }
}