using System;
using System.Collections.Generic;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Mediation
{
    public class FGMediationManager : FGModuleParent<FGMediationManager, FGMediationCallbacks>
    {
        public override string ModuleName => "Mediation";
        protected override string RemoteConfigKey => "FGMediation";
        public override Color LogColor => FGDebugSettings.settings.Mediation;

        private bool _isBannerLoaded = false;
        private bool _isInterstitialLoaded = false;
        private bool _isRewardedLoaded = false;

        private List<Func<bool>> _appOpenConditions = new List<Func<bool>>();

        public bool IsBannerReady => _isBannerLoaded;
        public bool IsInterstitialReady => _isInterstitialLoaded;
        public bool IsRewardedReady => _isRewardedLoaded;

        protected override void InitializeCallbacks()
        {
            Callbacks.OnBannerAdLoaded += adInfo => _isBannerLoaded = true;
            Callbacks.OnInterstitialAdLoaded += adInfo => _isInterstitialLoaded = true;
            Callbacks.OnRewardedAdLoaded += adInfo => _isRewardedLoaded = true;

            Callbacks.OnBannerAdFailedToLoad += () => _isBannerLoaded = false;
            Callbacks.OnInterstitialAdFailedToLoad += () => _isInterstitialLoaded = false;
            Callbacks.OnRewardedAdFailedToLoad += () => _isRewardedLoaded = false;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Initialize();
        }

        public void ShowBanner()
        {
            Callbacks._ShowBanner?.Invoke();
        }

        public void ShowBanner(string unitId)
        {
            Callbacks._ShowSpecificBanner?.Invoke(unitId);
        }

        public void HideBanner()
        {
            Callbacks._HideBanner?.Invoke();
        }

        public void ShowInterstitial(string interstitalPlacementName)
        {
            Callbacks._ShowInter?.Invoke(interstitalPlacementName);
        }

        public void ShowInterstitial(string unitId, string interstitalPlacementName)
        {
            Callbacks._ShowSpecificInter?.Invoke(interstitalPlacementName, unitId);
        }

        public void ShowRewarded(Action<bool> rewardedCallback, string rewardedPlacementName)
        {
            Callbacks._ShowRV?.Invoke(rewardedCallback, rewardedPlacementName);
        }

        public void ShowRewarded(Action<bool> rewardedCallback, string unitId, string rewardedPlacementName)
        {
            Callbacks._ShowSpecificRV?.Invoke(rewardedCallback, rewardedPlacementName, unitId);
        }

        public void ShowAppOpen()
        {
            Callbacks._ShowAppOpen?.Invoke();
        }

        public void AddAppOpenCondition(Func<bool> condition)
        {
            _appOpenConditions.Add(condition);
        }

        public bool MatchAppOpenCondition()
        {
            return CalculateCondition(_appOpenConditions.ToArray());
        }

        private bool CalculateCondition(Func<bool>[] conditions)
        {
            bool result = true;
            foreach (var condition in conditions)
            {
                result &= condition.Invoke();
            }

            return result;
        }

        protected override void ClearInitialization()
        {
            // throw new NotImplementedException();
        }
    }
}