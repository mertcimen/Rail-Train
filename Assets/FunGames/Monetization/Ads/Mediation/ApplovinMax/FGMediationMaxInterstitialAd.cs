using System;
using System.Collections;
using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.Tools.Core.Modules;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGMediationMaxInterstitialAd : FGMediationAdAbstract<FGMediationMaxInterstitialAd>
    {
        private int _interstitialRetryAttempt;
        private float _timeToLoad = 0;
        private float _loadIteration = 0;
        public override void InitializeCallbacks()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterAdImpressionEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedToLoadEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
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
                MaxSdk.LoadInterstitial(AdId);
            }
            catch
            {
                FGMax.Instance.Log("Failed Load Interstitials : Please Check Ad Unit");
            }
        }

        public override bool IsReady()
        {
            if (FunGamesSDK.IsNoAds) return false;
            return MaxSdk.IsInterstitialReady(AdId);
        }

        public void Show(string interstitialPlacementName)
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not show : Ads removed.");
                return;
            }
            
            FGMax.Instance.Log("Show Interstitial Ad");
            if (!MaxSdk.IsInterstitialReady(AdId)) return;
            try
            {
                MaxSdk.ShowInterstitial(AdId, interstitialPlacementName);
            }
            catch (Exception e)
            {
                FGAnalytics.NewDesignEvent("Error:UserQuitBeforeEndingAd");
                FGMax.Instance.LogError(e.StackTrace);
                throw;
            }
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            
            _interstitialRetryAttempt = 0;
            _timeToLoad = Time.time - _timeToLoad;
            InterstitialLoaded(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            
            FGMax.Instance._timeSinceLastInterstitial = 0f;
            InterstitialDisplayed(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            
            Load();
            FGAnalytics.NewDesignEvent("Interstitial:Dismissed");
            InterstitialClosed(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            InterstitialClicked(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnInterAdImpressionEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId))
            {
                FGMax.Instance.LogWarning("Not the same ids : " + adUnitId + " - " + AdId);
                return;
            }
            InterstitialImpressionValidated(adUnitId, FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnInterstitialFailedToLoadEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            _interstitialRetryAttempt++;
            var retryDelay = Math.Pow(2, _interstitialRetryAttempt);
            StartCoroutine(WaitToLoadInter((float)retryDelay));
            InterstitialFailedToLoad();
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            InterstitialFailedToDisplay(FGMax.Instance.FGAdInfo(adInfo));
            Load();
        }

        IEnumerator WaitToLoadInter(float time)
        {
            yield return new WaitForSeconds(time);
            Load();
        }
        
        

        protected void InterstitialLoaded(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Interstitial Ad Loaded");
            FGAnalytics.NewAdEvent(AdAction.Loaded, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdLoaded?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdLoaded?.Invoke(adInfo);
            FGAnalytics.NewDesignEvent(EVENT_LOADING_TIME, new Dictionary<string, object>
            {
                {EVENT_PARAM_TYPE, "Interstitial"},
                {EVENT_PARAM_UNIT_ID, adInfo.AdUnitIdentifier},
                {EVENT_PARAM_NETWORK, adInfo.NetworkName},
                {EVENT_PARAM_PLACEMENT, adInfo.Placement},
                {EVENT_PARAM_TIME, _timeToLoad},
                {EVENT_PARAM_LOAD_ITERATION, _loadIteration},
            });
            //Log("Interstitial Ready = " + FGMediation.IsInterstitialReady);
        }

        protected void InterstitialDisplayed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Interstitial Ad Displayed");
            FGAnalytics.NewAdEvent(AdAction.Show, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdDisplayed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdDisplayed?.Invoke(adInfo);
        }

        protected void InterstitialClosed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Interstitial Ad Dismissed");
            FGAnalytics.NewAdEvent(AdAction.Dismissed, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdDismissed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdDismissed?.Invoke(adInfo);
        }

        protected void InterstitialClicked(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Interstitial Ad Clicked");
            FGAnalytics.NewAdEvent(AdAction.Clicked, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdClicked?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdClicked?.Invoke(adInfo);
        }

        protected void InterstitialImpressionValidated(string arg1, FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Interstitial Impression validated");
            FGAnalytics.NewAdEvent(AdAction.Impression, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdImpression?.Invoke(arg1, adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdImpression?.Invoke(arg1, adInfo);
        }

        protected void InterstitialFailedToLoad()
        {
            FGMax.Instance.LogWarning("Interstitial failed to Load");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.Interstitial, "Max", "default");
            FGMax.Instance.Callbacks._OnInterstitialAdFailedToLoad?.Invoke();
            FGMediationManager.Instance.Callbacks._OnInterstitialAdFailedToLoad?.Invoke();
            //Log("Interstitial Ready = " + FGMediation.IsInterstitialReady);
        }

        protected void InterstitialFailedToDisplay(FGAdInfo adInfo)
        {
            FGMax.Instance.LogWarning("Interstitial failed to Display");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.Interstitial, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnInterstitialAdFailedToDisplay?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnInterstitialAdFailedToDisplay?.Invoke(adInfo);
        }
    }
}