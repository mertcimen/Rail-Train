using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.Tools.Core.Modules;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGMediationMaxBannerAd : FGMediationAdAbstract<FGMediationMaxBannerAd>
    {
        private bool _isBannerLoaded;
        private int _bannerRetryAttempt;
        private bool _showBannerAsked;
        private bool _isBannerShowing;
        
        private float _timeToLoad = 0;
        private float _loadIteration = 0;
        
        public override void InitializeCallbacks()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoaded;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent +=OnBannerAdImpressionEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedToLoadEvent;
        }

        public override void Load()
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not load : Ads removed.");
                return;
            }
            
            try
            {
                _loadIteration++;
                _timeToLoad = Time.time;
                MaxSdk.CreateBanner(AdId, MaxSdkBase.BannerPosition.BottomCenter);
                MaxSdk.SetBannerBackgroundColor(AdId, Color.black);
            }
            catch
            {
                FGMax.Instance.Log("Failed Create Banner : Please Check Ad Unit");
            }
        }

        public override bool IsReady()
        {
            if (FunGamesSDK.IsNoAds) return false;
            return _isBannerLoaded;
        }

        public void Show()
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not show : Ads removed.");
                return;
            }
            
            if (_isBannerShowing || _showBannerAsked) return;

            MaxSdk.ShowBanner(AdId);
            _isBannerShowing = _isBannerLoaded;
            _showBannerAsked = true;

            if (_isBannerLoaded) ShowBannerEvent();
        }

        public void Hide()
        {
            if (_isBannerShowing) HideBannerEvent();

            MaxSdk.HideBanner(AdId);
            _isBannerShowing = false;
            _showBannerAsked = false;
        }


        private void OnBannerAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            _isBannerLoaded = true;
            _timeToLoad = Time.time - _timeToLoad;
            BannerLoaded(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnBannerAdImpressionEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            BannerImpressionValidated(adUnitId, FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnBannerAdFailedToLoadEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            BannerFailedToLoad();
            FGMax.Instance.LogWarning(errorInfo.Message);
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            BannerClicked(FGMax.Instance.FGAdInfo(adInfo));
        }

        protected void ShowBannerEvent()
        {
            FGMax.Instance.Log("Banner Ad Displayed");
            FGAnalytics.NewAdEvent(AdAction.Show, AdType.Banner, FGMax.Instance._loadedBannerAd?.NetworkName,
                FGMax.Instance._loadedBannerAd?.Placement);
        }
        
        protected void HideBannerEvent()
        {
            FGMax.Instance.Log("Banner Ad Closed");
            FGAnalytics.NewAdEvent(AdAction.Dismissed, AdType.Banner, FGMax.Instance._loadedBannerAd?.NetworkName,
                FGMax.Instance._loadedBannerAd?.Placement);
        }
        
        protected void BannerLoaded(FGAdInfo adInfo)
        {
            FGMax.Instance._loadedBannerAd = adInfo;
            FGMax.Instance.Log("Banner Ad Loaded");
            FGAnalytics.NewAdEvent(AdAction.Loaded, AdType.Banner, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnBannerAdLoaded?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnBannerAdLoaded?.Invoke(adInfo);
            FGAnalytics.NewDesignEvent(EVENT_LOADING_TIME, new Dictionary<string, object>
            {
                {EVENT_PARAM_TYPE, "Banner"},
                {EVENT_PARAM_UNIT_ID, adInfo.AdUnitIdentifier},
                {EVENT_PARAM_NETWORK, adInfo.NetworkName},
                {EVENT_PARAM_PLACEMENT, adInfo.Placement},
                {EVENT_PARAM_TIME, _timeToLoad},
                {EVENT_PARAM_LOAD_ITERATION, _loadIteration},
            });
            //Log("Banner Ready = " + FGMediation.IsBannerReady);
        }

        protected void BannerClicked(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Banner Ad Closed");
            FGAnalytics.NewAdEvent(AdAction.Clicked, AdType.Banner, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnBannerAdClicked?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnBannerAdClicked?.Invoke(adInfo);
        }

        protected void BannerFailedToLoad()
        {
            FGMax.Instance.LogWarning("Banner Ad Failed to Load");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.Banner, "Max", "default");
            FGMax.Instance.Callbacks._OnBannerAdFailedToLoad?.Invoke();
            FGMediationManager.Instance.Callbacks._OnBannerAdFailedToLoad?.Invoke();
            //Log("Banner Ready = " + FGMediation.IsBannerReady);
        }

        protected void BannerImpressionValidated(string arg1, FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Banner Impression validated");
            FGAnalytics.NewAdEvent(AdAction.Impression, AdType.Banner, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnBannerAdImpression?.Invoke(arg1, adInfo);
            FGMediationManager.Instance.Callbacks._OnBannerAdImpression?.Invoke(arg1, adInfo);
        }
    }
}