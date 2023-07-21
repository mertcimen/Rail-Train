using System;

namespace FunGames.Monetization.IAP
{
    public class FGIAPCallbacks : FGModuleCallbacks
    {
        internal Action<FGIAPProduct> _OnPurchaseRequested;
        internal Action _OnRestoreRequested;
        internal Action<FGIAPProduct, FGIAPPurchaseInfo, IFGReceiptValidator[]> _OnReceiptValidated;
        internal Action<FGIAPProduct, FGIAPPurchaseInfo> _OnPurchaseSucceeded;
        internal Action<FGIAPProduct, string> _OnPurchaseFailed;
        internal Action<FGIAPProduct, FGIAPPurchaseInfo> _OnRestorationSucceeded;
        internal Action<FGIAPProduct, string> _OnRestorationFailed;

        public event Action<FGIAPProduct> OnPurchaseRequested
        {
            add => _OnPurchaseRequested += value;
            remove => _OnPurchaseRequested -= value;
        }
        public event Action OnRestoreRequested
        {
            add => _OnRestoreRequested += value;
            remove => _OnRestoreRequested -= value;
        }
        public event Action<FGIAPProduct, FGIAPPurchaseInfo, IFGReceiptValidator[]> OnReceiptValidated
        {
            add => _OnReceiptValidated += value;
            remove => _OnReceiptValidated -= value;
        }

        /// <summary>
        /// An event containing the purchased product details and a string
        /// </summary>
        public event Action<FGIAPProduct, FGIAPPurchaseInfo> OnPurchaseSucceeded
        {
            add => _OnPurchaseSucceeded += value;
            remove => _OnPurchaseSucceeded -= value;
        }

        public event Action<FGIAPProduct, string> OnPurchaseFailed
        {
            add => _OnPurchaseFailed += value;
            remove => _OnPurchaseFailed -= value;
        }

        public event Action<FGIAPProduct, FGIAPPurchaseInfo> OnRestorationSucceeded
        {
            add => _OnRestorationSucceeded += value;
            remove => _OnRestorationSucceeded -= value;
        }

        public event Action<FGIAPProduct, string> OnRestorationFailed
        {
            add => _OnRestorationFailed += value;
            remove => _OnRestorationFailed -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _OnPurchaseRequested = null;
            _OnReceiptValidated = null;
            _OnPurchaseSucceeded = null;
            _OnPurchaseFailed = null;
            _OnRestorationSucceeded = null;
            _OnRestorationFailed = null;
        }
    }
}