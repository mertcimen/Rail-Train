using System;

namespace FunGames.Mediation
{
    public class FGMediation
    {
        public static FGMediationCallbacks Callbacks => FGMediationManager.Instance.Callbacks;

        public static bool IsBannerReady => FGMediationManager.Instance.IsBannerReady;
        public static bool IsInterstitialReady => FGMediationManager.Instance.IsInterstitialReady;
        public static bool IsRewardedReady => FGMediationManager.Instance.IsRewardedReady;

        /// <summary>
        /// Display an Interstitial Ad
        /// </summary>
        /// <param name="placementName">The name of the Ad placement</param>
        public static void ShowInterstitial(string placementName = "default") =>
            FGMediationManager.Instance.ShowInterstitial(placementName);
        
        public static void ShowInterstitialWithId(string unitId, string placementName = "default") =>
            FGMediationManager.Instance.ShowInterstitial(unitId, placementName);

        /// <summary>
        /// Display a Rewarded Ad
        /// </summary>
        /// <param name="rewardedCallback"> The action to perform when the user can receive the reward. The boolean corresponds to the status of the reward callback (success or fail) </param>
        /// <param name="placementName">The name of the Ad placement</param>
        public static void ShowRewarded(Action<bool> rewardedCallback, string placementName = "default") =>
            FGMediationManager.Instance.ShowRewarded(rewardedCallback, placementName);

        public static void ShowRewardedWithId(Action<bool> rewardedCallback, string unitId, string placementName = "default") =>
            FGMediationManager.Instance.ShowRewarded(rewardedCallback, unitId, placementName);
        
        /// <summary>
        /// Display a Banner Ad
        /// </summary>
        public static void ShowBanner() => FGMediationManager.Instance.ShowBanner();
        
        public static void ShowBannerWithId(string unitId) => FGMediationManager.Instance.ShowBanner(unitId);

        /// <summary>
        /// Close current Banner Ad
        /// </summary>
        public static void HideBanner() => FGMediationManager.Instance.HideBanner();
        
        public static void ShowAppOpen() => FGMediationManager.Instance.ShowAppOpen();

        public static void AddAppOpenCondition(Func<bool> condition) => FGMediationManager.Instance.AddAppOpenCondition(condition);

        // public void OverrideAppOpenCondition(Func<bool> condition) => FGMediationManager.Instance.OverrideAppOpenCondition(condition);
    }
}