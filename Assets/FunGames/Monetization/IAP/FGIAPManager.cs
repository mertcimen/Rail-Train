using System;
using System.Collections.Generic;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Monetization.IAP
{
    public class FGIAPManager : FGModuleParent<FGIAPManager, FGIAPCallbacks>
    {
        public override string ModuleName => "IAP";
        protected override string RemoteConfigKey => "FGIAP";
        public override Color LogColor => FGDebugSettings.settings.IAP;

        private Action<bool> _initialization;

        private Dictionary<string, FGProductInfo> _productInfos = new Dictionary<string, FGProductInfo>();

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
        }

        public void BuyProduct(string id)
        {
            Callbacks._OnPurchaseRequested?.Invoke(FGIAP.GetProduct(id));
        }
        
        public void Restore()
        {
            Callbacks._OnRestoreRequested?.Invoke();
        }
        
        public void AddProductInfo(FGProductInfo productInfo)
        {
            if (_productInfos.ContainsKey(productInfo.id))
            {
                Log("Product info already exists for product " + productInfo.id);
                return;
            }
            _productInfos.Add(productInfo.id, productInfo);
        }
        
        public FGProductInfo GetProductInfo(string id)
        {
            if (!_productInfos.ContainsKey(id))
            {
                Log("Product info not found for product " + id);
                return null;
            }
            return _productInfos[id];
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}