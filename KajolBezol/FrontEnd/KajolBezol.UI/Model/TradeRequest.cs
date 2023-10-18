namespace KajolBezol.UI.Model
{
    
    public class TradeRequest
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? RequestTime { get; set; }
        public bool Cancelled { get; set; }
        public bool Fulfilled { get; set; }
        public bool Commited { get; set; }
        public int? TransactionId { get; set; }
        public TradeType Type { get; set; }

        public string TradeTypeStr
        {
            get
            {
                return this.Type.ToString();
            }
            set
            {
                
            }
        }
    }

    public enum TradeType
    {
        Buy, Sell
    }

    

}
