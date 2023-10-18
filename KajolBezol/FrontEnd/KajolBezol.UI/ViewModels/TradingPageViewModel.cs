using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KajolBezol.UI.Localization;
using KajolBezol.UI.Model;
using KajolBezol.UI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace KajolBezol.UI.ViewModels
{
    public partial class TradingPageViewModel : ObservableObject
    {
        private KajolBezolServiceUI _kajolBezolUIService;

        public IDispatcherTimer _timer;


        public TradingPageViewModel(KajolBezolServiceUI kajolBezolUIService)
        {
            this._kajolBezolUIService = kajolBezolUIService;

            this.Clean();

            this._timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
                {
                    DollarAmount = DollarAmount;
                };
            _timer.Start();

        }

        [ObservableProperty]
        public string transactionDisplay;

        public string isTradeButtonVisible;

        [ObservableProperty]
        public string operacionEnCurso;


        [ObservableProperty]
        public bool isSell;

        [ObservableProperty]
        public bool isRunning;


        public bool IsTradeButtonVisible
        {
            get
            {

                if (this._kajolBezolUIService.UserProfile.IsTradingOpen)
                {
                    var hasPendingRequests = this._kajolBezolUIService.UserProfile.HasPendingTradeRequests();

                    if (hasPendingRequests)
                    {
                        this.OperacionEnCurso = AppResources.CannotTradePrevOperation;
                        this.ShowWarning = true;

                    }
                    else
                    {
                        this.OperacionEnCurso = string.Empty;
                        this.ShowWarning = false;
                    }

                    return !hasPendingRequests;
                }
                else
                {
                    this.OperacionEnCurso = AppResources.ClosedTxt;
                    this.ShowWarning = true;
                    return false;
                }

            }



        }


        public int dollarAmount;

        public int DollarAmount
        {
            get
            {
                return dollarAmount;
            }


            set
            {
                dollarAmount = value;

                var roundedAmount = this.GetRoundedValue(dollarAmount);

                var textToDisplay = $"U${roundedAmount.ToString()}";


                this.TransactionDisplay = textToDisplay;
                OnPropertyChanged(nameof(DollarAmount));
                OnPropertyChanged(nameof(TransactionDisplay));
                OnPropertyChanged(nameof(IsTradeButtonVisible));
                OnPropertyChanged(nameof(OperationTypeSpanishTxt));





            }

        }



        public bool IsRounded(int amount)
        {
            if (amount % 100 == 0)
            {
                return true;
            }
            return false;
        }


        public string OperationTypeSpanishTxt
        {
            get
            {
                var matched = LookForAMatch(this.GetRoundedValue(this.DollarAmount)).Result;
                if (matched)
                {
                    if (this.OperationType == "Sell")
                    {
                        return AppResources.BuyRightNow;

                    }
                    else
                    {
                        return AppResources.SellRightNow;
                    }
                }
                else
                {
                    if (this.OperationType == "Sell")
                    {
                        return AppResources.BidToBuy;
                    }
                    else
                    {
                        return AppResources.BidToSell;

                    }
                }


            }
        }



        private int GetRoundedValue(double value)
        {
            var valueReceived = Math.Round(value) / 100;
            var begin = (int)Math.Floor(valueReceived);
            var end = (int)Math.Ceiling(valueReceived);

            int newValue;

            if (valueReceived - begin < end - valueReceived)
            {
                newValue = begin * 100;
            }
            else
            {
                newValue = end * 100;
            }

            return newValue;

        }



        public string _operationType;
        public string OperationType
        {
            get
            {
                return _operationType;
            }
            set
            {
                this._operationType = value;
            }
        }


        [ObservableProperty]
        public bool showWarning;

        [RelayCommand]
        async Task SendRequest()
        {

            try
            {
                IsRunning = true;

                if (await _kajolBezolUIService.IsTradingOpen())
                {
                    var transType = "";
                    string confirmMessage = string.Empty;


                    if (this.OperationType == "Sell")
                    {
                        transType = "InsertSellRequest";
                    }

                    if (this.OperationType == "Buy")
                    {
                        transType = "InsertBuyRequest";
                    }

                    var tradeRequests = await this._kajolBezolUIService.InsertTradeTransaction(transType, this.GetRoundedValue(this.DollarAmount).ToString());

                    if (tradeRequests.Count == 1)
                    {

                        if (this.OperationType == "Buy")
                        {
                            confirmMessage = String.Format(AppResources.BidConfirmSell, this.GetRoundedValue(this.DollarAmount).ToString());
                        }

                        if (this.OperationType == "Sell")
                        {
                            confirmMessage = String.Format(AppResources.BidConfirmBuy, this.GetRoundedValue(this.DollarAmount).ToString());

                        }
                        await App.Current.MainPage.DisplayAlert(confirmMessage, AppResources.CheckBillboardPeriodically, "OK");

                    }

                    else if (tradeRequests.Count > 1)
                    {

                        if (this.OperationType == "Buy")
                        {
                            confirmMessage = String.Format(AppResources.TransactionConfirmSell, this.GetRoundedValue(this.DollarAmount).ToString());
                        }

                        if (this.OperationType == "Sell")
                        {
                            confirmMessage = String.Format(AppResources.TransactionConfirmBuy, this.GetRoundedValue(this.DollarAmount).ToString());

                        }
                        await App.Current.MainPage.DisplayAlert(confirmMessage, AppResources.CheckBillboardForDetails, "OK");


                    }

                    await this._kajolBezolUIService.PopulateProfile(this._kajolBezolUIService.UserProfile.UserLogin);
                    this.Clean();

                }

                IsRunning = false;
            }
            catch
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, AppResources.ErrorDescription, "OK");

            }

        }

        [RelayCommand]
        public void ComprarBtn()
        {
            this.OperationType = "Sell";
            this.BuyColor = Color.FromArgb("#ffffff");
            this.SellColor = Color.FromArgb("#0000ff");

        }

        [RelayCommand]
        public void VenderBtn()
        {
            this.OperationType = "Buy";
            this.BuyColor = Color.FromArgb("#0000ff");
            this.SellColor = Color.FromArgb("#ffffff");

        }

        [ObservableProperty]
        public Microsoft.Maui.Graphics.Color sellColor;

        [ObservableProperty]
        public Microsoft.Maui.Graphics.Color buyColor;


        async Task<bool> LookForAMatch(int dollarAmount)
        {
            var matchingtradeType = this.OperationType == "Buy" ? TradeType.Sell : TradeType.Buy;

            var thisUser = this._kajolBezolUIService.UserProfile.User.WhatsAppNumber;
            var immediateList = this._kajolBezolUIService.UserProfile.OpenTradeRequests.Where(x =>
            {
                return (x.UserId != thisUser && x.Amount == dollarAmount && x.Type == matchingtradeType);

            }).ToList();

            if (immediateList.Count > 0)
            {
                return true;
            }

            return false;




        }

        public void Clean()
        {
            this.DollarAmount = 500;
            this.OperationType = "Sell";
            this.BuyColor = Color.FromArgb("#ffffff");
            this.SellColor = Color.FromArgb("#0000ff");


        }


    }
}
