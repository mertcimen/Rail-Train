namespace FunGames.Monetization.IAP
{
    public class FGReceiptValidatorIOS : IFGReceiptValidator
    {
        public readonly string ProductId;
        public readonly string TransactionId;
        public readonly string Payload;
        
        public FGReceiptValidatorIOS(string productId, string transactionId, string payload)
        {
            ProductId = productId;
            TransactionId = transactionId;
            Payload = payload;
        }
    }
}