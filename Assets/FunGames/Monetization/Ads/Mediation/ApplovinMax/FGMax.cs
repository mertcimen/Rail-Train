using System;
using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.AuthorizationTracking;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGMax : FGMediationAbstract<FGMax>
    {
        internal FGMediationMaxInterstitialAd DefaultInterstitial;
        internal FGMediationMaxRewardedAd DefaultRewarded;
        internal FGMediationMaxBannerAd DefaultBanner;
        internal FGMediationMaxAppOpenAd DefaultAppOpen;

        public override IFGModuleSettings Settings => FGApplovinMaxSettings.settings;
        public override string ModuleName => "ApplovinMax";
        protected override string RemoteConfigKey => "FGApplovinMax";
        public override Color LogColor => FGDebugSettings.settings.ApplovinMax;

        internal float _timeSinceLastInterstitial = -1;
        public float TimeSinceLastInterstitial => _timeSinceLastInterstitial;

        private Dictionary<string, IFGMediationAd> _fgMediationAds =
            new Dictionary<string, IFGMediationAd>();

        private FGMediationMaxBannerAd currentBannerAd;
        private Action<bool> _updateConsentCallback;

        protected override void InitializeCallbacks()
        {
            base.InitializeCallbacks();
            _updateConsentCallback = delegate { UpdateConsent(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _updateConsentCallback;
            Callbacks.InitializeAds += LoadAds;

            // InitializeMediationAds();

            MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent +=
                (adUnitId, adInfo) => OnRvInterAdShow("REWARDED_INTER", adInfo);
            MaxSdkCallbacks.CrossPromo.OnAdRevenuePaidEvent +=
                (adUnitId, adInfo) => OnCrosspromoAdShow("XPROMO", adInfo);
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (adUnitId, adInfo) => OnMrecAdShow("MREC", adInfo);
        }

        // Start is called on the start of FunGamesAds
        protected override void InitializeModule()
        {
            InitializeMediationAds();
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                InitializationComplete(MaxSdk.IsInitialized());
            };

            UpdateConsent();
            MaxSdk.SetSdkKey(FGApplovinMaxSettings.settings.maxSdkKey);
            MaxSdk.InitializeSdk();
            LogConfig();
        }

        private void Update()
        {
            if (_timeSinceLastInterstitial >= 0f) _timeSinceLastInterstitial += Time.deltaTime;
        }

        private void UpdateConsent()
        {
            Log("Consent updated !");
            MaxSdk.SetHasUserConsent(FGUserConsent.HasFullConsent);
            MaxSdk.SetIsAgeRestrictedUser(false);
            MaxSdk.SetDoNotSell(!FGUserConsent.HasFullConsent);
            if (FGUserConsent.HasFullConsent) MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
        }

        private void InitializeMediationAds()
        {
            DefaultInterstitial =
                FGMediationMaxInterstitialAd.Initialize(FGApplovinMaxSettings.settings.DefaultInterstitialId.Trim());
            DefaultRewarded =
                FGMediationMaxRewardedAd.Initialize(FGApplovinMaxSettings.settings.DefaultRewardedId.Trim());
            DefaultBanner = FGMediationMaxBannerAd.Initialize(FGApplovinMaxSettings.settings.DefaultBannerId.Trim());
            DefaultAppOpen = FGMediationMaxAppOpenAd.Initialize(FGApplovinMaxSettings.settings.DefaultAppOpenId.Trim());

            if (DefaultInterstitial)_fgMediationAds.Add(DefaultInterstitial.AdUnitId, DefaultInterstitial);
            if (DefaultRewarded)_fgMediationAds.Add(DefaultRewarded.AdUnitId, DefaultRewarded);
            if (DefaultBanner)_fgMediationAds.Add(DefaultBanner.AdUnitId, DefaultBanner);
            if (DefaultAppOpen) _fgMediationAds.Add(DefaultAppOpen.AdUnitId, DefaultAppOpen);

            foreach (var fgAdUnit in FGApplovinMaxSettings.settings.AdditionalUnitIds)
            {
                if (isForCurrentPlatform(fgAdUnit)) _fgMediationAds.Add(fgAdUnit.unitId.Trim(), InitializeAd(fgAdUnit));
            }
        }

        private IFGMediationAd InitializeAd(FGAdUnit fgAdUnit)
        {
            switch (fgAdUnit.adType)
            {
                case FGAdType.Interstitial:
                    return FGMediationMaxInterstitialAd.Initialize(fgAdUnit.unitId.Trim());
                case FGAdType.Rewarded:
                    return FGMediationMaxRewardedAd.Initialize(fgAdUnit.unitId.Trim());
                case FGAdType.Banner:
                    return FGMediationMaxBannerAd.Initialize(fgAdUnit.unitId.Trim());
            }

            return null;
        }

        private bool isForCurrentPlatform(FGAdUnit fgAdUnit)
        {
            if (String.IsNullOrEmpty(fgAdUnit.unitId)) return false;

            if (fgAdUnit.platform.Equals(FGPlatform.Android))
            {
#if UNITY_ANDROID
                return true;
#endif
            }

            if (fgAdUnit.platform.Equals(FGPlatform.IOS))
            {
#if UNITY_IOS
                return true;
#endif
            }

            return false;
        }

        private void LogConfig()
        {
            Dictionary<string, Dictionary<string, object>>
                config = new Dictionary<string, Dictionary<string, object>>();
            config.Add("ApplovinMaxSDK", new Dictionary<string, object> { { "Version", MaxSdk.Version } });

            foreach (var network in MaxSdk.GetAvailableMediatedNetworks())
            {
                config.Add(network.Name,
                    new Dictionary<string, object>
                        { { "Adapter_Version", network.AdapterVersion }, { "Sdk_Version", network.SdkVersion } });
            }

            foreach (var info in config)
            {
                FGAnalytics.NewDesignEvent("MaxConfig:" + info.Key, info.Value);
                Log("Network: " + info.Key);
                foreach (var versions in info.Value)
                {
                    Log("\t" + versions.Key + " : " + versions.Value);
                }
            }
        }

        protected override void LoadAds()
        {
            Log("Initialize Ads");
            foreach (var fgMediationAd in _fgMediationAds)
            {
                fgMediationAd.Value.Load();
            }
        }

        protected override void ShowBannerAd()
        {
            DefaultBanner.Show();
        }

        protected override void ShowBannerAd(string unitId)
        {
            FGMediationMaxBannerAd bannerAd = (FGMediationMaxBannerAd)_fgMediationAds[unitId];
            if (bannerAd == null || !bannerAd.IsReady())
            {
                ShowBannerAd();
                return;
            }

            bannerAd.Show();
        }

        protected override void HideBannerAd()
        {
            foreach (var fgMediationAd in _fgMediationAds)
            {
                if (fgMediationAd.Value is FGMediationMaxBannerAd) ((FGMediationMaxBannerAd)fgMediationAd.Value).Hide();
            }
        }

        protected override void ShowInterstitialAd(string interstitialPlacementName)
        {
            DefaultInterstitial.Show(interstitialPlacementName);
        }

        protected override void ShowInterstitialAd(string interstitialPlacementName, string unitId)
        {
            FGMediationMaxInterstitialAd interstitialAd = (FGMediationMaxInterstitialAd)_fgMediationAds[unitId];
            if (interstitialAd == null || !interstitialAd.IsReady())
            {
                ShowInterstitialAd(interstitialPlacementName);
                return;
            }

            interstitialAd.Show(interstitialPlacementName);
        }

        protected override void ShowRewardedAd(Action<bool> rewardedCallBack, string rewardedPlacementName)
        {
            DefaultRewarded.Show(rewardedCallBack, rewardedPlacementName);
        }

        protected override void ShowRewardedAd(Action<bool> rewardedCallBack, string rewardedPlacementName,
            string unitId)
        {
            ((FGMediationMaxRewardedAd)_fgMediationAds[unitId])?.Show(rewardedCallBack, rewardedPlacementName);

            FGMediationMaxRewardedAd rewardedAd = (FGMediationMaxRewardedAd)_fgMediationAds[unitId];
            if (rewardedAd == null || !rewardedAd.IsReady())
            {
                ShowRewardedAd(rewardedCallBack, rewardedPlacementName);
                return;
            }

            rewardedAd.Show(rewardedCallBack, rewardedPlacementName);
        }

        protected override void ShowAppOpen()
        {
            DefaultAppOpen.ShowAdIfReady();
        }

        private void OnCrosspromoAdShow(string adType, MaxSdkBase.AdInfo adinfo)
        {
            FGAdInfo fgAdInfo = new FGAdInfo(adinfo.AdUnitIdentifier, adinfo.AdFormat, adinfo.NetworkName,
                adinfo.NetworkPlacement, adinfo.Placement, adinfo.CreativeIdentifier, adinfo.Revenue,
                adinfo.RevenuePrecision);
            CrosspromoImpressionValidated(adType, fgAdInfo);
        }

        private void OnMrecAdShow(string adType, MaxSdkBase.AdInfo adinfo)
        {
            FGAdInfo fgAdInfo = new FGAdInfo(adinfo.AdUnitIdentifier, adinfo.AdFormat, adinfo.NetworkName,
                adinfo.NetworkPlacement, adinfo.Placement, adinfo.CreativeIdentifier, adinfo.Revenue,
                adinfo.RevenuePrecision);
            MrecImpressionValidated(adType, fgAdInfo);
        }

        private void OnRvInterAdShow(string adType, MaxSdkBase.AdInfo adinfo)
        {
            FGAdInfo fgAdInfo = new FGAdInfo(adinfo.AdUnitIdentifier, adinfo.AdFormat, adinfo.NetworkName,
                adinfo.NetworkPlacement, adinfo.Placement, adinfo.CreativeIdentifier, adinfo.Revenue,
                adinfo.RevenuePrecision);
            FGMediationMaxRewardedAd.RewardedImpressionValidated(adType, fgAdInfo);
        }

        internal FGAdInfo FGAdInfo(MaxSdkBase.AdInfo adinfo)
        {
            FGAdInfo fgAdInfo = new FGAdInfo(adinfo.AdUnitIdentifier, adinfo.AdFormat, adinfo.NetworkName,
                adinfo.NetworkPlacement, adinfo.Placement, adinfo.CreativeIdentifier, adinfo.Revenue,
                adinfo.RevenuePrecision);
            return fgAdInfo;
        }

        internal void LoadInterstitials()
        {
            foreach (IFGMediationAd fgMediationAd in _fgMediationAds.Values)
            {
                if (fgMediationAd is FGMediationMaxInterstitialAd) fgMediationAd.Load();
            }
        }

        internal void LoadRewarded()
        {
            foreach (IFGMediationAd fgMediationAd in _fgMediationAds.Values)
            {
                if (fgMediationAd is FGMediationMaxRewardedAd) fgMediationAd.Load();
            }
        }

        internal void LoadBanners()
        {
            foreach (IFGMediationAd fgMediationAd in _fgMediationAds.Values)
            {
                if (fgMediationAd is FGMediationMaxBannerAd) fgMediationAd.Load();
            }
        }

        protected override void ClearInitialization()
        {
            base.ClearInitialization();
            FGUserConsent.GDPRCallbacks.OnInitialized -= _updateConsentCallback;
        }
    }
}