using System;
using FunGames.AuthorizationTracking;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Monetization.AudioAds
{
    public class FGAudioAdsManager : FGModuleParent<FGAudioAdsManager, FGAudioAdsCallbacks>
    {
        public override string ModuleName => "Audio Ads";
        protected override string RemoteConfigKey => "FGAudioAds";
        public override Color LogColor => FGDebugSettings.settings.AudioAds;

        public const string RC_AUDIOADS = "AudioAds";

        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
            FGRemoteConfig.AddDefaultValue(RC_AUDIOADS, 0);
        }

        public void PlaySkippable(bool useImage, float skipCountdown)
        {
            Callbacks?._PlaySkippable?.Invoke(useImage, skipCountdown);
        }

        public void PlayRewarded(bool useImage)
        {
            Callbacks?._PlayRewarded?.Invoke(useImage);
        }

        public void StopPlayingAd()
        {
            Callbacks?._StopAd?.Invoke();
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}