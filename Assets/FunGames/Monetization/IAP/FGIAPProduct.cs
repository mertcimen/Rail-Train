using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Monetization.IAP
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES_FUNGAMES + "/IAP/Product", menuName = FGPath.FUNGAMES + "/IAP/FGProduct")]
    public class FGIAPProduct : ScriptableObject
    {
        public string GooglePlayId;
        public string AppleStoreId;

        public FGProductType ProductType;

        public string Id
        {
            get
            {
#if UNITY_IOS
                return AppleStoreId;
#else
                return GooglePlayId;
#endif
            }
        }
    }

    public class FGProductInfo
    {
        public string id;
        public string title;
        public string description;
        public decimal price;
        public string priceString;
        public string currencyCode;
    }

    public enum FGProductType
    {
        CONSUMABLE,
        SUBSCRIPTION,
        NON_CONSUMABLE
    }

    public class FGIAPPurchaseInfo
    {
        public string transactionID;
        public string receipt;
        public double price;
        public string currencyCode;

        public FGIAPPurchaseInfo(string transactionID, string receipt, double price, string currencyCode)
        {
            this.transactionID = transactionID;
            this.receipt = receipt;
            this.price = price;
            this.currencyCode = currencyCode;
        }
    }
}