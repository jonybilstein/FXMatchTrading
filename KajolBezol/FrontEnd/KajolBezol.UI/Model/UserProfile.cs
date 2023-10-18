using KajolBezol.UI.Model;
using System.ComponentModel;

namespace KajolBezol.UI.Model
{
    public class UserProfile
    {
        public User User { get; set; }
        public UserLogin UserLogin { get; set; }
        public string UsePolicy { get; set; }
        public List<TradeRequest> OpenTradeRequests { get; set; }
        public List<TradeRequest> MyTradeRequests { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string Token { get; set; }
        public bool IsTradingOpen { get; set; }
        public string WhatsAppContact { get; set; }


        public bool HasPendingTradeRequests()
        {
            return this.MyTradeRequests.Where(x => !x.Cancelled && !(x.Commited && x.Fulfilled)).Any();
        }

        public UserProfile()
        {


        }

        public void Clean()
        {
            User = null;
            UserLogin = null;
            OpenTradeRequests = null;
            MyTradeRequests = null;
            BuyPrice = 0;
            SellPrice = 0;
            Token = null;
        }







    }
}
