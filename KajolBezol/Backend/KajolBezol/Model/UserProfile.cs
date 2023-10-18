using KajolBezol.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.ComponentModel;

namespace KajolBezol.Model
{
    public class UserProfile
    {
        public User User { get; set; }
        public List<TradeRequest> OpenTradeRequests { get; set; }
        public List<TradeRequest> MyTradeRequests { get; set; }
        private IKajolBezolService KajolBezolService { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string Token { get; set; }

        public string UsePolicy { get; set; }
        public bool IsTradingOpen { get; set; }
        public string WhatsAppContact { get; set; }




        public UserProfile(IKajolBezolService kajolBezol, User user, string token = "")
        {
            this.KajolBezolService = kajolBezol;
            this.User = user;
            Task.Run(async () => await this.Refresh()).Wait();
            this.Token = token;


        }
        public async Task Refresh()
        {

            this.OpenTradeRequests = await KajolBezolService.GetPendingTradeRequests(this.User.UserName, this.User.Role == "Admin" ? true : false);
            this.MyTradeRequests = KajolBezolService.GetRequests(User.WhatsAppNumber);
            this.BuyPrice = await KajolBezolService.GetBuyingPrice();
            this.SellPrice = await KajolBezolService.GetSellingPrice();
            this.UsePolicy = KajolBezolService.GetUsePolicy();
            this.IsTradingOpen = KajolBezolService.IsTradingOpen();
            this.WhatsAppContact = KajolBezolService.GetWhatsAppContact();
        }

    }
}
