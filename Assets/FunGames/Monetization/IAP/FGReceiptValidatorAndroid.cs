namespace FunGames.Monetization.IAP
{
    public class FGReceiptValidatorAndroid : IFGReceiptValidator
    {
        // public string OrderUniqueId => _orderUniqueId;
        // public string PurchaseToken => _purchaseToken;
        // public string Payload => _payload;
        
        public readonly string OrderUniqueId;
        public readonly string PurchaseToken;
        public readonly string Payload;

        public FGReceiptValidatorAndroid(string orderUniqueId, string purchaseToken, string payload)
        {
            OrderUniqueId = orderUniqueId;
            PurchaseToken = purchaseToken;
            Payload = payload;
        }
    }
}