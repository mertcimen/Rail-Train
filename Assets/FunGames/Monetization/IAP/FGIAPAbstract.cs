using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.Tools;
using UnityEngine;

namespace FunGames.Monetization.IAP
{
    public abstract class FGIAPAbstract<T> : FGModuleChild<T, FGIAPCallbacks>
        where T : FGModuleChild<T, FGIAPCallbacks>
    {
        protected override IFGModuleParent Parent => FGIAPManager.Instance;

        protected abstract void RequestPurchase(FGIAPProduct product);
        
        protected abstract void RequestRestore();

        protected override void InitializeCallbacks()
        {
            // Callbacks.OnInitialized += delegate { InitializeProductInfos(); };
            FGIAPManager.Instance.Callbacks.Initialization += Initialize;
            FGIAPManager.Instance.Callbacks.OnPurchaseRequested += RequestPurchase;
            FGIAPManager.Instance.Callbacks.OnRestoreRequested += RequestRestore;
        }

        protected void PurchaseRequestedEvent(FGIAPProduct product)
        {
            Log("Purchase requested for : " + product.Id);
            Callbacks._OnPurchaseRequested?.Invoke(product);
            FGAnalytics.NewDesignEvent("PurchaseRequested", new Dictionary<string, object>()
            {
                { "ProductId", product.Id }
            });
        }

        protected void ReceiptValidatedEvent(FGIAPProduct product, FGIAPPurchaseInfo purchaseInfo,
            IFGReceiptValidator[] validatedReceipts)
        {
            foreach (IFGReceiptValidator productReceipt in validatedReceipts)
            {
                if (productReceipt is FGReceiptValidatorAndroid androidReceipt)
                {
                    Log("Android Receipt validated :");
                    Log("Order Unique ID = " + androidReceipt.OrderUniqueId);
                    Log("Purchase Token = " + androidReceipt.PurchaseToken);
                }

                if (productReceipt is FGReceiptValidatorIOS iosReceipt)
                {
                    Log("IOS Receipt validated :");
                    Log("Product ID = " + iosReceipt.ProductId);
                    Log("Transaction ID = " + iosReceipt.TransactionId);
                }
            }

            Callbacks._OnReceiptValidated?.Invoke(product, purchaseInfo, validatedReceipts);
            FGIAPManager.Instance.Callbacks._OnReceiptValidated?.Invoke(product, purchaseInfo, validatedReceipts);
        }

        protected void PurchaseSucceededEvent(FGIAPProduct product, FGIAPPurchaseInfo purchaseInfo)
        {
            if (!FGProductType.CONSUMABLE.Equals(product.ProductType) && PlayerPrefs.HasKey(product.Id))
            {
                Log("Purchase event called but purchase already processed for " + product.Id);
                return;
            }

            Log("Purchasing product : " + product.Id + " Succeeded !");
            Log("Price : " + purchaseInfo.price);
            Log("Currency : " + purchaseInfo.currencyCode);
            Callbacks._OnPurchaseSucceeded?.Invoke(product, purchaseInfo);
            FGIAPManager.Instance.Callbacks._OnPurchaseSucceeded?.Invoke(product, purchaseInfo);
            PlayerPrefs.SetInt(product.Id, 1);
            FGAnalytics.NewDesignEvent("PurchaseSucceeded", new Dictionary<string, object>()
            {
                { "ProductId", product.Id },
                { "Price", purchaseInfo.price },
                { "Currency", purchaseInfo.currencyCode }
            });
        }

        protected void PurchaseFailedEvent(FGIAPProduct product, string message)
        {
            LogWarning("Purchasing product : " + product.Id + " Failed due to " + message);
            Callbacks._OnPurchaseFailed?.Invoke(product, message);
            FGIAPManager.Instance.Callbacks._OnPurchaseFailed?.Invoke(product, message);
            FGAnalytics.NewDesignEvent("PurchaseFailed", new Dictionary<string, object>()
            {
                { "ProductId", product.Id },
                { "Message", message }
            });
        }

        protected void RestorationSucceededEvent(FGIAPProduct product, FGIAPPurchaseInfo purchaseInfo)
        {
            if (!FGProductType.CONSUMABLE.Equals(product.ProductType) && PlayerPrefs.HasKey(product.Id))
            {
                Log("Restore event called but purchase already processed for " + product.Id);
                return;
            }

            Log("Restoring product : " + product.Id + " Succeeded !");
            Log("Price : " + purchaseInfo.price);
            Log("Currency : " + purchaseInfo.currencyCode);
            Callbacks._OnRestorationSucceeded?.Invoke(product, purchaseInfo);
            FGIAPManager.Instance.Callbacks._OnRestorationSucceeded?.Invoke(product, purchaseInfo);
            PlayerPrefs.SetInt(product.Id, 1);
            FGAnalytics.NewDesignEvent("RestorationSucceeded", new Dictionary<string, object>()
            {
                { "ProductId", product.Id },
                { "Price", purchaseInfo.price },
                { "Currency", purchaseInfo.currencyCode }
            });
        }

        protected void RestorationFailedEvent(FGIAPProduct product, string message)
        {
            LogWarning("Restoration Failed");
            Callbacks._OnRestorationFailed?.Invoke(product, message);
            FGIAPManager.Instance.Callbacks._OnRestorationFailed?.Invoke(product, message);
            FGAnalytics.NewDesignEvent("RestorationFailed", new Dictionary<string, object>()
            {
                { "ProductId", product.Id },
                { "Message", message }
            });
        }

        protected override void ClearInitialization()
        {
            FGIAPManager.Instance.Callbacks.Initialization -= Initialize;
            FGIAPManager.Instance.Callbacks.OnPurchaseRequested -= RequestPurchase;
        }
    }
}