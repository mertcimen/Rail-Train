using UnityEngine;
using UnityEngine.Audio;

namespace FunGames.Monetization.AudioAds
{
    public class FGAudioMixerHandler : MonoBehaviour
    {
        [Tooltip(
            "Enable this option if you want to mute all other In-Game Audios when an Audio Ad is playing (recommended).\nNote: Remind to bind your audios with the Audio Ad Mixer.")]
        public bool muteOtherInGameAudio = true;

        public float transitionTime = 0.25f;

        [Tooltip("The snapshot that is active while an ad is not playing.")]
        public AudioMixerSnapshot normalMixerSnapshot;

        [Tooltip("The snapshot that is active while an ad is playing.")]
        public AudioMixerSnapshot adPlayingSnapshot;

        void Awake()
        {
            Debug.Log("Mute audio = " + muteOtherInGameAudio);
            if (!muteOtherInGameAudio) return;

            FGAudioAds.Callbacks.OnSkippableStarted += delegate { OnAdPlaybackStarted(); };
            FGAudioAds.Callbacks.OnRewardedStarted += delegate { OnAdPlaybackStarted(); };
            FGAudioAds.Callbacks.OnSkippableCompleted += OnAdPlaybackCompleted;
            FGAudioAds.Callbacks.OnRewardedCompleted += OnAdPlaybackCompleted;
        }

        private void OnAdPlaybackStarted()
        {
            Debug.Log("Mute game audio");
            adPlayingSnapshot.TransitionTo(transitionTime);
        }

        private void OnAdPlaybackCompleted()
        {
            Debug.Log("Unmute game audio");
            normalMixerSnapshot.TransitionTo(transitionTime);
        }
    }
}