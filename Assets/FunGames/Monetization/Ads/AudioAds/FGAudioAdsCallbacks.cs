using System;

namespace FunGames.Monetization.AudioAds
{
    public class FGAudioAdsCallbacks : FGModuleCallbacks
    {
        internal Action _StopAd;
        internal Action<bool, float> _PlaySkippable;
        internal Action _OnSkippableStarted;
        internal Action _OnSkippableCompleted;

        internal Action<bool> _PlayRewarded;
        internal Action _OnRewardedStarted;
        internal Action _OnRewardedCompleted;

        // private static Action _onAdPausedEvent;
        // private static Action _onAdResumedEvent;
        // private static Action _onAdErrorEvent;

        public event Action<bool, float> OnPlaySkippable
        {
            add => _PlaySkippable += value;
            remove => _PlaySkippable -= value;
        }

        public event Action StopAd
        {
            add => _StopAd += value;
            remove => _StopAd -= value;
        }

        public event Action OnSkippableStarted
        {
            add => _OnSkippableStarted += value;
            remove => _OnSkippableStarted -= value;
        }

        public event Action OnSkippableCompleted
        {
            add => _OnSkippableCompleted += value;
            remove => _OnSkippableCompleted -= value;
        }

        public event Action<bool> OnPlayRewarded
        {
            add => _PlayRewarded += value;
            remove => _PlayRewarded -= value;
        }

        public event Action OnRewardedStarted
        {
            add => _OnRewardedStarted += value;
            remove => _OnRewardedStarted -= value;
        }

        public event Action OnRewardedCompleted
        {
            add => _OnRewardedCompleted += value;
            remove => _OnRewardedCompleted -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _StopAd = null;
            _PlaySkippable = null;
            _OnSkippableStarted = null;
            _OnSkippableCompleted = null;

            _PlayRewarded = null;
            _OnRewardedStarted = null;
            _OnRewardedCompleted = null;
        }
    }
}