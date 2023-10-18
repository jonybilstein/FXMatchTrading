using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KajolBezol.UI.Localization;
using KajolBezol.UI.Model;
using KajolBezol.UI.Services;
using KajolBezol.UI.Settings;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KajolBezol.UI.ViewModels
{
    public partial class MatchPageViewModel : ObservableObject
    {




        public MatchPageViewModel(KajolBezolServiceUI service, IConfiguration config)
        {
            this.Service = service;

            var settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
            _whatsAppContact = this.Service.UserProfile.WhatsAppContact;

            this.Clean();

            this._timer = new System.Timers.Timer();
            _timer.Interval = 4000;
            _timer.Elapsed += (s, e) =>
            {
                RefreshUserProfile();
            };
            _timer.Start();


        }

        public string whatsAppText = string.Empty;

        public System.Timers.Timer _timer;

        public string _whatsAppContact;
        public KajolBezolServiceUI Service { get; }

        [ObservableProperty]
        string matchMessage;

        [ObservableProperty]
        bool whatsAppVisible;


        [ObservableProperty]
        string buyPrice;

        [ObservableProperty]
        string sellPrice;


        [ObservableProperty]
        List<TradeRequest> availableRequests;


        [ObservableProperty]
        decimal _amount;

        [ObservableProperty]
        string tradeTypeStr;

        [ObservableProperty]
        string transactionStateIcon;

        public void RefreshUserProfile()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await this.Service.PopulateProfile(new UserLogin
                {
                    UserName = this.Service.UserProfile.UserLogin.UserName,
                    PassCode = this.Service.UserProfile.UserLogin.PassCode,
                    DeviceKey = this.Service.UserProfile.UserLogin.DeviceKey

                });

                this.PopulateAvailableRequests();

                DateTime now = DateTime.Now;

                this.BuyPrice = $"{this.Service.UserProfile.BuyPrice.ToString()}";
                this.SellPrice = $"{this.Service.UserProfile.SellPrice.ToString()}";

                var tradeRequest = this.Service.UserProfile.MyTradeRequests.Where(x => !x.Fulfilled && !x.Cancelled).SingleOrDefault();

                if (tradeRequest != null)
                {
                    if (tradeRequest.TransactionId != null && tradeRequest.Commited)
                    {
                        if (tradeRequest.Type == TradeType.Buy)
                        {
                            this.MatchMessage = string.Format(AppResources.SellMatched,tradeRequest.Amount,tradeRequest.TransactionId);
                        }
                        else if (tradeRequest.Type == TradeType.Sell)
                        {
                            this.MatchMessage = string.Format(AppResources.BuyMatched, tradeRequest.Amount, tradeRequest.TransactionId);

                        }

                        TransactionStateIcon = "matched.svg";
                        WhatsAppVisible = true;
                        whatsAppText = $"";
                    }
                    else if (tradeRequest.TransactionId == null && !tradeRequest.Commited)
                    {
                        if (tradeRequest.Type == TradeType.Buy)
                        {
                            this.MatchMessage = string.Format(AppResources.SellStillNoMatch, tradeRequest.Amount);

                        }
                        else if (tradeRequest.Type == TradeType.Sell)
                        {
                            this.MatchMessage = string.Format(AppResources.BuyStillNoMatch,tradeRequest.Amount);
                        }

                        TransactionStateIcon = "waiting.svg";
                        WhatsAppVisible = false;
                        whatsAppText = string.Empty;
                    }
                    else
                    {
                        this.MatchMessage = AppResources.NoBids;
                        TransactionStateIcon = "createrequest.svg";
                        WhatsAppVisible = false;
                        whatsAppText = string.Empty;

                    }
                }
                else
                {
                    this.MatchMessage = AppResources.NoBids;
                    TransactionStateIcon = "createrequest.svg";
                    WhatsAppVisible = false;
                    whatsAppText = string.Empty;

                }


            });


        }

        
        void PopulateAvailableRequests()
        {
            this.AvailableRequests = this.Service.UserProfile.OpenTradeRequests.Where(x =>
            {
                return x.UserId != this.Service.UserProfile.User.WhatsAppNumber;
            }).OrderBy(x => { return x.Type; }).ToList();

        }

        [RelayCommand]
        public async Task WhatsAppContact()
        {

            var ws_URI = $"{this._whatsAppContact}?text={UrlEncoder.Default.Encode(this.whatsAppText)}";
            Uri uri = new Uri(ws_URI);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }


        public void Clean()
        {
            this.WhatsAppVisible = false;
            this.MatchMessage = string.Empty;
            this.BuyPrice = string.Empty;
            this.SellPrice = string.Empty;
            this.whatsAppText = string.Empty;
        }


    }
}
