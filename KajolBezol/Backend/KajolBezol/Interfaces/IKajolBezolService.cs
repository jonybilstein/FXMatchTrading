using KajolBezol.Model;

namespace KajolBezol.Interfaces
{
    public interface IKajolBezolService
    {
        Task<decimal> GetBlueNow();
        Task<decimal> GetBuyingPrice();
        Task<decimal> GetSellingPrice();
        Task<User> GetUser(UserLogin userToAuthenticate);
        Task<List<TradeRequest>> GetPendingTradeRequests(string filterUserId, bool viewAllFields = true);
        List<TradeRequest> InsertTradeRequest(int userId, decimal amount, TradeType tradeType);
        bool UserHasNoBids(int userId);
        bool IsTradingOpen();
        List<TradeRequest> GetRequests(int userNameAsInt);
        string GetUsePolicy();
        int GetAppVersion();

        Task SendMessageToUser(string token, string title, string body);
        Task SendEmailToUser(string emailAddr, string title, string body);
        string GetWhatsAppContact();
        string GetPrivacyPolicy();
    }
}
