namespace FunGames.Monetization.IAP
{
    public class FGIAP
    {
        public static FGIAPCallbacks Callbacks => FGIAPManager.Instance.Callbacks;

        public static void BuyProduct(string id) => FGIAPManager.Instance.BuyProduct(id);

        public static FGIAPProduct GetProduct(string id)
        {
            foreach (var fgProduct in FGIAPSettings.settings.Products)
            {
                if (fgProduct.GooglePlayId.Equals(id) || fgProduct.AppleStoreId.Equals(id)) return fgProduct;
            }

            return null;
        }
        public static FGProductInfo GetProductInfo(string id) => FGIAPManager.Instance.GetProductInfo(id);

        public static void Restore() => FGIAPManager.Instance.Restore();
    }
}