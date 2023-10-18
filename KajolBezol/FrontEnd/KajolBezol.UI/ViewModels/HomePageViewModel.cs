using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KajolBezol.UI.Model;
using KajolBezol.UI.Pages;
using KajolBezol.UI.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
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

    public partial class HomePageViewModel : ObservableObject
    {

        KajolBezolServiceUI _kajolBezolUIService;
        public HomePageViewModel(KajolBezolServiceUI kajolBezolUIService)
        {
            this._kajolBezolUIService = kajolBezolUIService;
        
        }

        
        
        

    }

}
