using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Core.Modules;

namespace FunGames.Monetization.AudioAds
{
    public abstract class FGAudioAdsAbstract<T> : FGModuleChild<T, FGAudioAdsCallbacks>
        where T : FGModuleChild<T, FGAudioAdsCallbacks>
    {
        
        private const string EVENT_ID_PLAY_REQUESTED = "AudioAdPlayRequested";
        private const string EVENT_ID_AD_STARTED = "AudioAdStarted";
        private const string EVENT_ID_AD_COMPLETED = "AudioAdCompleted";
        
        private const string EVENT_PARAM_MODULE = "module";
        private const string EVENT_PARAM_TYPE = "type";
        
        private const string EVENT_SKIPPABLE = "Skippable";
        private const string EVENT_REWARDED = "Rewarded";
        
        protected override IFGModuleParent Parent => FGAudioAdsManager.Instance;

        private bool _initialized;

        protected abstract int ConfigValue { get; }

        protected abstract void Init();
        protected abstract void PlaySkippableAd(bool useImage, float skipCountdown);
        protected abstract void PlayRewardedAd(bool useImage);
        protected abstract void StopPlayingAd();

        protected override void InitializeCallbacks()
        {
            FGAudioAdsManager.Instance.Callbacks.Initialization += Initialize;
            FGAudioAdsManager.Instance.Callbacks.OnPlaySkippable += PlaySkippable;
            FGAudioAdsManager.Instance.Callbacks.OnPlayRewarded += PlayRewarded;
            FGAudioAdsManager.Instance.Callbacks.StopAd += StopAd;
        }

        protected override void InitializeModule()
        {
            int audioAdsConfig = FGRemoteConfig.GetIntValue(FGAudioAdsManager.RC_AUDIOADS);
            if (!ConfigValue.Equals(audioAdsConfig))
            {
                LogWarning("AudioAds config is set to " + audioAdsConfig + " = No " + ModuleName);
                ClearInitialization();
                InitializationComplete(true);
                return;
            }

            Init();
        }

        public void PlaySkippable(bool useImage, float skipCountdown)
        {
            if (FunGamesSDK.IsNoAds)
            {
                LogWarning("Ad will not play : Ads removed.");
                return;
            }

            Log("Play Skippable");
            FGAnalytics.NewDesignEvent(EVENT_ID_PLAY_REQUESTED, new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_SKIPPABLE }
            });

            PlaySkippableAd(useImage, skipCountdown);
        }

        public void PlayRewarded(bool useImage)
        {
            if (FunGamesSDK.IsNoAds)
            {
                LogWarning("Ad will not play : Ads removed.");
                return;
            }

            Log("Play Rewarded");
            FGAnalytics.NewDesignEvent(EVENT_ID_PLAY_REQUESTED, new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_REWARDED }
            });
            PlayRewardedAd(useImage);
        }

        protected void SkippablePlayed()
        {
            Log("Skippable Started");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_STARTED, new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_SKIPPABLE }
            });
            
            Callbacks?._OnSkippableStarted?.Invoke();
            FGAudioAdsManager.Instance.Callbacks?._OnSkippableStarted?.Invoke();
        }

        protected void SkippableCompleted()
        {
            Log("Skippable completed");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_COMPLETED,new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_SKIPPABLE}
            });
            Callbacks?._OnSkippableCompleted?.Invoke();
            FGAudioAdsManager.Instance.Callbacks?._OnSkippableCompleted?.Invoke();
        }

        protected void RewardedPlayed()
        {
            Log("Rewarded Started");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_STARTED, new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_REWARDED }
            });
            Callbacks?._OnRewardedStarted?.Invoke();
            FGAudioAdsManager.Instance.Callbacks?._OnRewardedStarted?.Invoke();
        }

        public void RewardedCompleted()
        {
            Log("Rewarded completed");
            FGAnalytics.NewDesignEvent(EVENT_ID_AD_COMPLETED,new Dictionary<string, object>
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_TYPE, EVENT_REWARDED}
            });
            Callbacks?._OnRewardedCompleted?.Invoke();
            FGAudioAdsManager.Instance.Callbacks?._OnRewardedCompleted?.Invoke();
        }

        public void StopAd()
        {
            Log("Stop Playing Ad");
            StopPlayingAd();
        }

        protected override void ClearInitialization()
        {
            FGAudioAdsManager.Instance.Callbacks.Initialization -= Initialize;
            FGAudioAdsManager.Instance.Callbacks.OnPlaySkippable -= PlaySkippable;
            FGAudioAdsManager.Instance.Callbacks.OnPlayRewarded -= PlayRewarded;
            FGAudioAdsManager.Instance.Callbacks.StopAd -= StopAd;
        }

        public override bool MustBeInitialized()
        {
            return base.MustBeInitialized() && !FunGamesSDK.IsNoAds;
        }
    }
}