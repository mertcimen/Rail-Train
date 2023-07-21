using System;
using System.Collections;
using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.RemoteConfig;
using FunGames.Tools.Core.Modules;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGMediationMaxAppOpenAd : FGMediationAdAbstract<FGMediationMaxAppOpenAd>
    {
        private const string PP_APPOPEN_FREQ = "AppOpenFrequency";
        private const string PP_APPOPEN_CD = "AppOpenCoolDown";
        
        public const string RC_APPOPEN_FREQ = "AppOpenFrequency";
        public const string RC_APPOPEN_CD = "AppOpenCoolDown";

        private int AppOpenCoolDown = 0;
        private int AppOpenFrequency = 0;
        private int _lastAdIteration = 0;

        private int _appOpenRetryAttempt;
        
        private float _timeToLoad = 0;
        private float _loadIteration = 0;

        public override void InitializeCallbacks()
        {
            FGRemoteConfig.AddDefaultValue(RC_APPOPEN_FREQ,0);
            FGRemoteConfig.AddDefaultValue(RC_APPOPEN_CD,10);
            
            FGMediation.AddAppOpenCondition(AppOpenFrequencyCondition);
            FGMediation.AddAppOpenCondition(AppOpenFirstDayCondition);
            FGMediation.AddAppOpenCondition(AppOpenCoolDownCondition);

            InitializePlayerPref();

            FGRemoteConfig.Callbacks.OnInitialized += delegate { InitializePlayerPref(); };

            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAppOpenDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnAppOpenClickedEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAppOpenAdImpressionEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenFailedToLoadEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenFailedToDisplayEvent;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;

            FGMax.Instance.Callbacks.OnInitialized += delegate { Load(); };
            FGMediation.Callbacks.OnAppOpenAdLoaded += ShowFirstTime;
        }

        public override void Load()
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not load : Ads removed.");
                return;
            }
            
            _loadIteration++;
            _timeToLoad = Time.time;
            MaxSdk.LoadAppOpenAd(AdId);
        }

        public override bool IsReady()
        {
            if (FunGamesSDK.IsNoAds) return false;
            return MaxSdk.IsAppOpenAdReady(AdId);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                ShowAdIfReady();
            }
        }

        public void ShowAdIfReady()
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not show : Ads removed.");
                return;
            }
            
            FGMax.Instance.Log("Show AppOpen Requested");

            if (IsReady())
            {
                // FGMax.Instance.Log("Last Ad Iteration = " + _lastAdIteration);
                // FGMax.Instance.Log("Ad Frequency = " + AppOpenFrequency);
                if (!FGMediationManager.Instance.MatchAppOpenCondition()) return;

                FGMax.Instance.Log("Show AppOpen Ad");
                MaxSdk.ShowAppOpenAd(AdId);
            }
            else
            {
                Load();
            }
        }

        private void ShowFirstTime(FGAdInfo adInfo)
        {
            ShowAdIfReady();
            FGMediation.Callbacks.OnAppOpenAdLoaded -= ShowFirstTime;
        }

        private void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            _appOpenRetryAttempt = 0;
            StopCoroutine(WaitToLoadAd());
            _timeToLoad = Time.time - _timeToLoad;
            AppOpenLoaded(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnAppOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            FGMax.Instance._timeSinceLastInterstitial = 0f;
            AppOpenDisplayed(FGMax.Instance.FGAdInfo(adInfo));
        }

        public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            Load();
            AppOpenClosed(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnAppOpenClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            AppOpenClicked(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnAppOpenAdImpressionEvent(string adType, MaxSdkBase.AdInfo adinfo)
        {
            if (!adinfo.AdUnitIdentifier.Equals(AdId)) return;
            AppOpenImpressionValidated(adType, FGMax.Instance.FGAdInfo(adinfo));
        }

        private void OnAppOpenFailedToLoadEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            StartCoroutine(WaitToLoadAd());
            AppOpenFailedToLoad();
        }

        private void OnAppOpenFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            AppOpenFailedToDisplay(FGMax.Instance.FGAdInfo(adInfo));
        }

        protected void AppOpenLoaded(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("AppOpen Ad Loaded");
            FGAnalytics.NewAdEvent(AdAction.Loaded, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdLoaded?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdLoaded?.Invoke(adInfo);
            FGAnalytics.NewDesignEvent(EVENT_LOADING_TIME, new Dictionary<string, object>
            {
                {EVENT_PARAM_TYPE, "AppOpen"},
                {EVENT_PARAM_UNIT_ID, adInfo.AdUnitIdentifier},
                {EVENT_PARAM_NETWORK, adInfo.NetworkName},
                {EVENT_PARAM_PLACEMENT, adInfo.Placement},
                {EVENT_PARAM_TIME, _timeToLoad},
                {EVENT_PARAM_LOAD_ITERATION, _loadIteration},
            });
        }

        protected void AppOpenDisplayed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("AppOpen Ad Displayed");
            FGAnalytics.NewAdEvent(AdAction.Show, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdDisplayed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdDisplayed?.Invoke(adInfo);
        }

        protected void AppOpenClosed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("AppOpen Ad Dismissed");
            FGAnalytics.NewAdEvent(AdAction.Dismissed, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdDismissed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdDismissed?.Invoke(adInfo);
        }

        protected void AppOpenClicked(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("AppOpen Ad Clicked");
            FGAnalytics.NewAdEvent(AdAction.Clicked, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdClicked?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdClicked?.Invoke(adInfo);
        }

        protected void AppOpenImpressionValidated(string arg1, FGAdInfo adInfo)
        {
            FGMax.Instance.Log("AppOpen Impression validated");
            FGAnalytics.NewAdEvent(AdAction.Impression, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdImpression?.Invoke(arg1, adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdImpression?.Invoke(arg1, adInfo);
        }

        protected void AppOpenFailedToLoad()
        {
            FGMax.Instance.LogWarning("AppOpen failed to Load");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.AppOpen, "Max", "default");
            FGMax.Instance.Callbacks._OnAppOpenAdFailedToLoad?.Invoke();
            FGMediationManager.Instance.Callbacks._OnAppOpenAdFailedToLoad?.Invoke();
        }

        protected void AppOpenFailedToDisplay(FGAdInfo adInfo)
        {
            FGMax.Instance.LogWarning("AppOpen failed to Display");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.AppOpen, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnAppOpenAdFailedToDisplay?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnAppOpenAdFailedToDisplay?.Invoke(adInfo);
        }

        IEnumerator WaitToLoadAd()
        {
            _appOpenRetryAttempt++;
            double retryDelay = Math.Pow(2, _appOpenRetryAttempt);
            Debug.Log("Retry delay :" + retryDelay);
            yield return new WaitForSeconds((float)retryDelay);
            Load();
        }

        private void InitializePlayerPref()
        {
            AppOpenFrequency = FGRemoteConfig.GetIntValue(RC_APPOPEN_FREQ);
            AppOpenCoolDown = FGRemoteConfig.GetIntValue(RC_APPOPEN_CD);
            PlayerPrefs.SetInt(PP_APPOPEN_FREQ, AppOpenFrequency);
            PlayerPrefs.SetInt(PP_APPOPEN_CD, AppOpenCoolDown);
            _lastAdIteration = AppOpenFrequency - 1;
        }

        private bool AppOpenFrequencyCondition()
        {
            if (AppOpenFrequency == 0) return false;

            if (_lastAdIteration == AppOpenFrequency - 1)
            {
                _lastAdIteration = 0;
                return true;
            }

            _lastAdIteration++;
            return false;
        }

        private bool AppOpenFirstDayCondition()
        {
            return !FunGamesSDK.IsFirstConnection;
        }

        private bool AppOpenCoolDownCondition()
        {
            // FGMax.Instance.Log("AppOpenCoolDown = " + AppOpenCoolDown + "; Time since last inter = " +
            //                    FGMax.Instance.TimeSinceLastInterstitial);
            return FGMax.Instance.TimeSinceLastInterstitial >= AppOpenCoolDown ||
                   (-1f).Equals(FGMax.Instance.TimeSinceLastInterstitial);
        }
    }
}