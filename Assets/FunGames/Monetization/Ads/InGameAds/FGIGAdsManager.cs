using System;
using FunGames.AuthorizationTracking;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Monetization.InGameAds
{
    public class FGIGAdsManager : FGModuleParent<FGIGAdsManager, FGIGAdsCallbacks>
    {
        public override string ModuleName => "InGameAds";
        protected override string RemoteConfigKey => "FGInGameAds";
        public override Color LogColor => FGDebugSettings.settings.InGameAds;

        public const string RC_INGAMEADS = "InGameAds";

        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
            FGRemoteConfig.AddDefaultValue(RC_INGAMEADS, 0);
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}