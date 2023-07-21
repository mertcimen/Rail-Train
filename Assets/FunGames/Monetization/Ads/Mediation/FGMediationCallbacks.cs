using System;

namespace FunGames.Mediation
{
    public class FGMediationCallbacks : FGModuleCallbacks
    {
        internal Action _InitializeAds;

        internal Action _ShowBanner;
        internal Action<string> _ShowSpecificBanner;
        internal Action _HideBanner;
        internal Action<FGAdInfo> _OnBannerAdLoaded;
        internal Action<FGAdInfo> _OnBannerAdClicked;
        internal Action<string, FGAdInfo> _OnBannerAdImpression;
        internal Action _OnBannerAdFailedToLoad;

        internal Action<string> _ShowInter;
        internal Action<string, string> _ShowSpecificInter;
        internal Action<FGAdInfo> _OnInterstitialAdLoaded;
        internal Action<FGAdInfo> _OnInterstitialAdDisplayed;
        internal Action<FGAdInfo> _OnInterstitialAdDismissed;
        internal Action<FGAdInfo> _OnInterstitialAdClicked;
        internal Action<string, FGAdInfo> _OnInterstitialAdImpression;
        internal Action _OnInterstitialAdFailedToLoad;
        internal Action<FGAdInfo> _OnInterstitialAdFailedToDisplay;

        internal Action<Action<bool>, string> _ShowRV;
        internal Action<Action<bool>, string, string> _ShowSpecificRV;
        internal Action<FGAdInfo> _OnRewardedLoaded;
        internal Action<FGAdInfo> _OnRewardedDisplayed;
        internal Action<FGAdInfo> _OnRewardedDismissed;
        internal Action<FGAdInfo> _OnRewardedClicked;
        internal Action<string, FGAdInfo> _OnRewardedAdImpression;
        internal Action<FGAdInfo> _OnRewardReceived;
        internal Action _OnRewardedFailedToLoad;
        internal Action<FGAdInfo> _OnRewardedFailedToDisplay;

        internal Action _ShowAppOpen;
        internal Action<FGAdInfo> _OnAppOpenAdLoaded;
        internal Action<FGAdInfo> _OnAppOpenAdDisplayed;
        internal Action<FGAdInfo> _OnAppOpenAdDismissed;
        internal Action<FGAdInfo> _OnAppOpenAdClicked;
        internal Action<string, FGAdInfo> _OnAppOpenAdImpression;
        internal Action _OnAppOpenAdFailedToLoad;
        internal Action<FGAdInfo> _OnAppOpenAdFailedToDisplay;

        internal Action<string, FGAdInfo> _OnMrecAdImpression;
        internal Action<string, FGAdInfo> _OnCrosspromoAdImpression;

        public event Action InitializeAds
        {
            add => _InitializeAds = value;
            remove => _InitializeAds -= value;
        }

        #region Banners

        public event Action ShowBanner
        {
            add => _ShowBanner += value;
            remove => _ShowBanner -= value;
        }

        public event Action<string> ShowSpecificBanner
        {
            add => _ShowSpecificBanner += value;
            remove => _ShowSpecificBanner -= value;
        }

        public event Action HideBanner
        {
            add => _HideBanner += value;
            remove => _HideBanner -= value;
        }

        public event Action<FGAdInfo> OnBannerAdLoaded
        {
            add => _OnBannerAdLoaded += value;
            remove => _OnBannerAdLoaded -= value;
        }

        public event Action<FGAdInfo> OnBannerAdDisplayed
        {
            add => _OnBannerAdLoaded += value;
            remove => _OnBannerAdLoaded -= value;
        }

        public event Action<FGAdInfo> OnBannerAdClicked
        {
            add => _OnBannerAdClicked += value;
            remove => _OnBannerAdClicked -= value;
        }

        public event Action OnBannerAdFailedToLoad
        {
            add => _OnBannerAdFailedToLoad += value;
            remove => _OnBannerAdFailedToLoad -= value;
        }

        public event Action<string, FGAdInfo> OnBannerAdImpression
        {
            add => _OnBannerAdImpression += value;
            remove => _OnBannerAdImpression -= value;
        }

        #endregion

        #region Interstitials

        public event Action<string> ShowInterstitial
        {
            add => _ShowInter += value;
            remove => _ShowInter -= value;
        }

        public event Action<string, string> ShowSpecificInterstitial
        {
            add => _ShowSpecificInter += value;
            remove => _ShowSpecificInter -= value;
        }

        public event Action<FGAdInfo> OnInterstitialAdLoaded
        {
            add => _OnInterstitialAdLoaded += value;
            remove => _OnInterstitialAdLoaded -= value;
        }

        public event Action<FGAdInfo> OnInterstitialAdDisplayed
        {
            add => _OnInterstitialAdDisplayed += value;
            remove => _OnInterstitialAdDisplayed -= value;
        }

        public event Action<FGAdInfo> OnInterstitialClosed
        {
            add => _OnInterstitialAdDismissed += value;
            remove => _OnInterstitialAdDismissed -= value;
        }

        public event Action<FGAdInfo> OnInterstitialAdClicked
        {
            add => _OnInterstitialAdClicked += value;
            remove => _OnInterstitialAdClicked -= value;
        }

        public event Action<string, FGAdInfo> OnInterstitialAdImpression
        {
            add => _OnInterstitialAdImpression += value;
            remove => _OnInterstitialAdImpression -= value;
        }

        public event Action OnInterstitialAdFailedToLoad
        {
            add => _OnInterstitialAdFailedToLoad += value;
            remove => _OnInterstitialAdFailedToLoad -= value;
        }

        public event Action<FGAdInfo> OnInterstitialAdFailedToDisplay
        {
            add => _OnInterstitialAdFailedToDisplay += value;
            remove => _OnInterstitialAdFailedToDisplay -= value;
        }

        #endregion

        #region Rewarded

        public event Action<Action<bool>, string> ShowRewarded
        {
            add => _ShowRV += value;
            remove => _ShowRV -= value;
        }

        public event Action<Action<bool>, string, string> ShowSpecificRewarded
        {
            add => _ShowSpecificRV += value;
            remove => _ShowSpecificRV -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdLoaded
        {
            add => _OnRewardedLoaded += value;
            remove => _OnRewardedLoaded -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdDisplayed
        {
            add => _OnRewardedDisplayed += value;
            remove => _OnRewardedDisplayed -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdClosed
        {
            add => _OnRewardedDismissed += value;
            remove => _OnRewardedDismissed -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdClicked
        {
            add => _OnRewardedClicked += value;
            remove => _OnRewardedClicked -= value;
        }

        public event Action<string, FGAdInfo> OnRewardedAdImpression
        {
            add => _OnRewardedAdImpression += value;
            remove => _OnRewardedAdImpression -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdRewardReceived
        {
            add => _OnRewardReceived += value;
            remove => _OnRewardReceived -= value;
        }

        public event Action OnRewardedAdFailedToLoad
        {
            add => _OnRewardedFailedToLoad += value;
            remove => _OnRewardedFailedToLoad -= value;
        }

        public event Action<FGAdInfo> OnRewardedAdFailedToDisplay
        {
            add => _OnRewardedFailedToDisplay += value;
            remove => _OnRewardedFailedToDisplay -= value;
        }

        #endregion

        #region AppOpen

        public event Action ShowAppOpen
        {
            add => _ShowAppOpen += value;
            remove => _ShowAppOpen -= value;
        }

        public event Action<FGAdInfo> OnAppOpenAdLoaded
        {
            add => _OnAppOpenAdLoaded += value;
            remove => _OnAppOpenAdLoaded -= value;
        }

        public event Action<FGAdInfo> OnAppOpenDisplayed
        {
            add => _OnAppOpenAdDisplayed += value;
            remove => _OnAppOpenAdDisplayed -= value;
        }

        public event Action<FGAdInfo> OnAppOpenClosed
        {
            add => _OnAppOpenAdDismissed += value;
            remove => _OnAppOpenAdDismissed -= value;
        }

        public event Action<FGAdInfo> OnAppOpenAdClicked
        {
            add => _OnAppOpenAdClicked += value;
            remove => _OnAppOpenAdClicked -= value;
        }

        public event Action<FGAdInfo> OnAppOpenAdClosed
        {
            add => _OnAppOpenAdDismissed += value;
            remove => _OnAppOpenAdDismissed -= value;
        }

        public event Action<string, FGAdInfo> OnAppOpenAdImpression
        {
            add => _OnAppOpenAdImpression += value;
            remove => _OnAppOpenAdImpression -= value;
        }

        public event Action OnAppOpenAdFailedToLoad
        {
            add => _OnAppOpenAdFailedToLoad += value;
            remove => _OnAppOpenAdFailedToLoad -= value;
        }

        public event Action<FGAdInfo> OnAppOpenAdFailedToDisplay
        {
            add => _OnAppOpenAdFailedToDisplay += value;
            remove => _OnAppOpenAdFailedToDisplay -= value;
        }

        #endregion

        public event Action<string, FGAdInfo> OnCrosspromoAdImpression
        {
            add => _OnCrosspromoAdImpression += value;
            remove => _OnCrosspromoAdImpression -= value;
        }

        public event Action<string, FGAdInfo> OnMrecAdImpression
        {
            add => _OnMrecAdImpression += value;
            remove => _OnMrecAdImpression -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _InitializeAds = null;

            _ShowBanner = null;
            _ShowSpecificBanner = null;
            _HideBanner = null;
            _OnBannerAdLoaded = null;
            _OnBannerAdClicked = null;
            _OnBannerAdImpression = null;
            _OnBannerAdFailedToLoad = null;

            _ShowInter = null;
            _ShowSpecificInter = null;
            _OnInterstitialAdLoaded = null;
            _OnInterstitialAdDisplayed = null;
            _OnInterstitialAdDismissed = null;
            _OnInterstitialAdClicked = null;
            _OnInterstitialAdImpression = null;
            _OnInterstitialAdFailedToLoad = null;
            _OnInterstitialAdFailedToDisplay = null;

            _ShowRV = null;
            _ShowSpecificRV = null;
            _OnRewardedLoaded = null;
            _OnRewardedDisplayed = null;
            _OnRewardedDismissed = null;
            _OnRewardedClicked = null;
            _OnRewardedAdImpression = null;
            _OnRewardReceived = null;
            _OnRewardedFailedToLoad = null;
            _OnRewardedFailedToDisplay = null;

            _ShowAppOpen = null;
            _OnAppOpenAdLoaded = null;
            _OnAppOpenAdDisplayed = null;
            _OnAppOpenAdDismissed = null;
            _OnAppOpenAdClicked = null;
            _OnAppOpenAdImpression = null;
            _OnAppOpenAdFailedToLoad = null;
            _OnAppOpenAdFailedToDisplay = null;

            _OnMrecAdImpression = null;
            _OnCrosspromoAdImpression = null;
        }
    }
}