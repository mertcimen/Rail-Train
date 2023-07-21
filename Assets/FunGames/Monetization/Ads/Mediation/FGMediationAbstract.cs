using System;
using FunGames.AuthorizationTracking;
using FunGames.Tools;

namespace FunGames.Mediation
{
    public abstract class FGMediationAbstract<T> : FGModuleChild<T, FGMediationCallbacks>
        where T : FGModuleChild<T, FGMediationCallbacks>
    {
        protected override IFGModuleParent Parent => FGMediationManager.Instance;

        internal FGAdInfo _loadedBannerAd;

        private Action<bool> _adInitialization;

        protected abstract void LoadAds();
        protected abstract void ShowBannerAd();
        protected abstract void ShowBannerAd(string unitId);
        protected abstract void HideBannerAd();

        protected abstract void ShowInterstitialAd(string interstitalPlacementName);

        protected abstract void ShowInterstitialAd(string interstitalPlacementName, string unitId);

        protected abstract void ShowRewardedAd(Action<bool> rewardedCallBack, string rewardedPlacementName);

        protected abstract void ShowRewardedAd(Action<bool> rewardedCallBack, string rewardedPlacementName,
            string unitId);

        protected abstract void ShowAppOpen();

        protected override void InitializeCallbacks()
        {
            _adInitialization = delegate { InitializeAds(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _adInitialization;
            // FGMediationManager.Instance.Callbacks.OnInitialized += _adInitialization;
            FGMediationManager.Instance.Callbacks.Initialization += Initialize;
            FGMediationManager.Instance.Callbacks.InitializeAds += LoadAds;
            FGMediationManager.Instance.Callbacks.ShowBanner += ShowBannerAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificBanner += ShowBannerAd;
            FGMediationManager.Instance.Callbacks.HideBanner += HideBannerAd;
            FGMediationManager.Instance.Callbacks.ShowInterstitial += ShowInterstitialAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificInterstitial += ShowInterstitialAd;
            FGMediationManager.Instance.Callbacks.ShowRewarded += ShowRewardedAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificRewarded += ShowRewardedAd;
            FGMediationManager.Instance.Callbacks.ShowAppOpen += ShowAppOpen;
        }

        protected void InitializeAds()
        {
            if (!IsInitialized())
            {
                FGMediationManager.Instance.Callbacks.OnInitialized += _adInitialization;
                return;
            }

            Callbacks._InitializeAds?.Invoke();
        }

        protected void MrecImpressionValidated(string arg1, FGAdInfo arg2)
        {
            Log("MRec Impression validated");
            Callbacks._OnMrecAdImpression?.Invoke(arg1, arg2);
            FGMediationManager.Instance.Callbacks._OnMrecAdImpression?.Invoke(arg1, arg2);
        }

        protected void CrosspromoImpressionValidated(string arg1, FGAdInfo arg2)
        {
            Log("Crosspromo Impression validated");
            Callbacks._OnCrosspromoAdImpression?.Invoke(arg1, arg2);
            FGMediationManager.Instance.Callbacks._OnCrosspromoAdImpression?.Invoke(arg1, arg2);
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _adInitialization;
            FGMediationManager.Instance.Callbacks.OnInitialized -= _adInitialization;
            FGMediationManager.Instance.Callbacks.Initialization -= Initialize;
            FGMediationManager.Instance.Callbacks.InitializeAds -= LoadAds;
            FGMediationManager.Instance.Callbacks.ShowBanner -= ShowBannerAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificBanner -= ShowBannerAd;
            FGMediationManager.Instance.Callbacks.HideBanner -= HideBannerAd;
            FGMediationManager.Instance.Callbacks.ShowInterstitial -= ShowInterstitialAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificInterstitial -= ShowInterstitialAd;
            FGMediationManager.Instance.Callbacks.ShowRewarded -= ShowRewardedAd;
            FGMediationManager.Instance.Callbacks.ShowSpecificRewarded -= ShowRewardedAd;
            FGMediationManager.Instance.Callbacks.ShowAppOpen -= ShowAppOpen;
        }
    }
}