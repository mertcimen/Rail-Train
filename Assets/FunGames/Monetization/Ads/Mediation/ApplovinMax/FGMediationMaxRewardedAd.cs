using System;
using System.Collections;
using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.Tools.Core.Modules;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGMediationMaxRewardedAd : FGMediationAdAbstract<FGMediationMaxRewardedAd>
    {
        private int _rewardedRetryAttempt;
        private Action<bool> _rewardedCallback;

        private float _timeToLoad = 0;
        private float _loadIteration = 0;

        public override void InitializeCallbacks()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdShowEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedToLoadEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
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
                MaxSdk.LoadRewardedAd(AdId);
            }
            catch
            {
                FGMax.Instance.LogError("Failed Load Rewarded : Please Check Ad Unit");
            }
        }

        public override bool IsReady()
        {
            if (FunGamesSDK.IsNoAds) return false;
            return MaxSdk.IsRewardedAdReady(AdId);
        }


        public void Show(Action<bool> rewardedCallBack, string rewardedPlacementName)
        {
            if (FunGamesSDK.IsNoAds)
            {
                FGMax.Instance.LogWarning("Ad will not show : Ads removed.");
                return;
            }
            
            FGMax.Instance.Log("Show Rewarded Ad");
            _rewardedCallback = rewardedCallBack;

            if (MaxSdk.IsRewardedAdReady(AdId))
            {
                try
                {
                    MaxSdk.ShowRewardedAd(AdId, rewardedPlacementName);
                    FGAnalytics.NewDesignEvent("Rewarded" + rewardedPlacementName + ":succeeded");
                }
                catch (Exception e)
                {
                    FGAnalytics.NewDesignEvent("RewardedError" + rewardedPlacementName + ":UserQuitBeforeEndingAd");
                    FGMax.Instance.LogError(e.StackTrace);
                    throw;
                }
            }
            else
            {
                FGAnalytics.NewDesignEvent("RewardedNoAd" + rewardedPlacementName + ":NoAdReady");
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            _timeToLoad = Time.time - _timeToLoad;
            _rewardedRetryAttempt = 0;
            RewardedLoaded(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            RewardedDisplayed(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            Load();
            RewardedClosed(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            RewardedClicked(FGMax.Instance.FGAdInfo(adInfo));
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            RewardedReceived(FGMax.Instance.FGAdInfo(adInfo));
            _rewardedCallback?.Invoke(true);
        }


        private void OnRewardedAdFailedToLoadEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            _rewardedRetryAttempt++;
            var retryDelay = Math.Pow(2, _rewardedRetryAttempt);
            StartCoroutine(WaitToLoadRewarded((float)retryDelay));
            RewardedFailedToLoad();
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            Load();
            RewardedFailedToDisplay(FGMax.Instance.FGAdInfo(adInfo));
            _rewardedCallback?.Invoke(false);
        }

        private void OnRewardedAdShowEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (!adUnitId.Equals(AdId)) return;
            FGAdInfo fgAdInfo = new FGAdInfo(adinfo.AdUnitIdentifier, adinfo.AdFormat, adinfo.NetworkName,
                adinfo.NetworkPlacement, adinfo.Placement, adinfo.CreativeIdentifier, adinfo.Revenue,
                adinfo.RevenuePrecision);
            RewardedImpressionValidated(adUnitId, fgAdInfo);
        }

        IEnumerator WaitToLoadRewarded(float time)
        {
            yield return new WaitForSeconds(time);
            Load();
        }
        
        protected void RewardedLoaded(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Ad Loaded");
            FGAnalytics.NewAdEvent(AdAction.Loaded, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedLoaded?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedLoaded?.Invoke(adInfo);
            FGAnalytics.NewDesignEvent(EVENT_LOADING_TIME, new Dictionary<string, object>
            {
                {EVENT_PARAM_TYPE, "Rewarded"},
                {EVENT_PARAM_UNIT_ID, adInfo.AdUnitIdentifier},
                {EVENT_PARAM_NETWORK, adInfo.NetworkName},
                {EVENT_PARAM_PLACEMENT, adInfo.Placement},
                {EVENT_PARAM_TIME, _timeToLoad},
                {EVENT_PARAM_LOAD_ITERATION, _loadIteration},
            });
            //Log("Rewarded Ready = " + FGMediation.IsRewardedReady);
        }

        protected void RewardedDisplayed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Ad Displayed");
            FGAnalytics.NewAdEvent(AdAction.Show, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedDisplayed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedDisplayed?.Invoke(adInfo);
        }


        protected void RewardedClosed(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Ad Closed");
            FGAnalytics.NewAdEvent(AdAction.Dismissed, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedDismissed?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedDismissed?.Invoke(adInfo);
        }

        protected void RewardedClicked(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Ad Clicked");
            FGAnalytics.NewAdEvent(AdAction.Clicked, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedClicked?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedClicked?.Invoke(adInfo);
        }


        public static void RewardedImpressionValidated(string arg1, FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Impression validated");
            FGAnalytics.NewAdEvent(AdAction.Impression, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedAdImpression?.Invoke(arg1, adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedAdImpression?.Invoke(arg1, adInfo);
        }

        protected void RewardedReceived(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Reward Received");
            FGAnalytics.NewAdEvent(AdAction.RewardReceived, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardReceived?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardReceived?.Invoke(adInfo);
        }

        protected void RewardedFailedToLoad()
        {
            FGMax.Instance.LogWarning("Rewarded failed to Load");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.RewardedVideo, "Max", "default");
            FGMax.Instance.Callbacks._OnRewardedFailedToLoad?.Invoke();
            FGMediationManager.Instance.Callbacks._OnRewardedFailedToLoad?.Invoke();
            //Log("Rewarded Ready = " + FGMediation.IsRewardedReady);
        }

        protected void RewardedFailedToDisplay(FGAdInfo adInfo)
        {
            FGMax.Instance.Log("Rewarded Ad failed to Display");
            FGAnalytics.NewAdEvent(AdAction.FailedShow, AdType.RewardedVideo, adInfo.NetworkName, adInfo.Placement);
            FGMax.Instance.Callbacks._OnRewardedFailedToDisplay?.Invoke(adInfo);
            FGMediationManager.Instance.Callbacks._OnRewardedFailedToDisplay?.Invoke(adInfo);
        }
    }
}