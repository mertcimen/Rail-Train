using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.Tools;
using FunGames.Tools.Core.Modules;

namespace FunGames.Monetization.InGameAds
{
    public abstract class FGIGAdAbstract<T> : FGModuleChild<T, FGIGAdsCallbacks>
        where T : FGModuleChild<T, FGIGAdsCallbacks>
    {
        protected override IFGModuleParent Parent => FGIGAdsManager.Instance;

        private const string EVENT_ID_AD_IMPRESSION = "InGameAdImpression";
        private const string EVENT_ID_AD_CLICKED = "InGameAdClicked";
        
        private const string EVENT_PARAM_MODULE = "module";
        
        protected override void InitializeCallbacks()
        {
            FGIGAdsManager.Instance.Callbacks.Initialization += Initialize;
            FGIGAdsManager.Instance.Callbacks.OnAdClicked += AdClicked;
        }

        protected void AdDisplayed()
        {
            Log("Ad displayed");
            Callbacks._adDisplayed?.Invoke();
        }

        protected void AdLoaded()
        {
            Log("Ad loaded");
            Callbacks._adLoaded?.Invoke();
        }

        protected void AdRequestFailed()
        {
            Log("Ad request failed");
            Callbacks._adRequestFailed?.Invoke();
        }

        protected void AdImpressionValidated()
        {
            Log("Ad impression validated");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_IMPRESSION,new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName }
            });
            Callbacks._adImpressionValidated?.Invoke();
        }

        protected void AdCompleted()
        {
            Log("Ad completed");
            Callbacks._adCompleted?.Invoke();
        }

        public void AdClicked()
        {
            Log("Ad clicked");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_CLICKED,new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName }
            });
            Callbacks._adClicked?.Invoke();
        }

        protected override void ClearInitialization()
        {
            FGIGAdsManager.Instance.Callbacks.Initialization -= Initialize;
            FGIGAdsManager.Instance.Callbacks.OnAdClicked -= AdClicked;
        }
        
        public override bool MustBeInitialized()
        {
            return base.MustBeInitialized() && !FunGamesSDK.IsNoAds;
        }
    }
}