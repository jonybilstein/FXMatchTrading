namespace KajolBezol.Model
{
    public class TradeRequest
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public decimal amount { get; set; }
        public DateTime? RequestTime { get; set; }
        public bool Cancelled { get; set; }
        public bool Fulfilled { get; set; }
        public bool Commited { get; set; }
        public int? TransactionId { get; set; }
        public TradeType Type { get; set; }



        private string UserTransactionType
        {
            get
            {
                var enumType = this.Type.ToString();
                switch (enumType)
                {
                    case "Buy":
                        return "sell";
                    case "Sell":
                        return "buy";
                    default:
                        return "";
                }
            }
        }

        public string Title
        {
            get
            {
                if (this.TransactionId == null || (int)this.TransactionId == 0)
                {
                    return $"Your bid to {this.UserTransactionType}";
                }
                else
                {
                    return $"Your bid to {this.UserTransactionType}";
                }
            }
        }

        public string Body
        {
            get
            {
                if (this.TransactionId == null || (int)this.TransactionId == 0)
                {
                    return $"Your bid to {this.UserTransactionType} US${this.amount} is awaiting. We'll let you know once it gets matched";
                }
                else
                {
                    return $"Your bid to {this.UserTransactionType} US${this.amount} has been matched under code {this.TransactionId}. Contact the admin to finish this transaction.";
                }

            }

        }

        public string ShortBody
        {
            get
            {
                if (this.TransactionId == null || (int)this.TransactionId == 0)
                {
                    return $"Your bid to {this.UserTransactionType} US${this.amount} is awaiting";
                }
                else
                {
                    return $"Your bid to {this.UserTransactionType} US${this.amount} got matched. App -> Billboard for +info";
                }

            }

        }


    }
}



public enum TradeType
{
    Buy, Sell
}


