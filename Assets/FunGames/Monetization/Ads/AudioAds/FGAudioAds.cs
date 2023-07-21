namespace FunGames.Monetization.AudioAds
{
    public static class FGAudioAds
    {
        public static FGAudioAdsCallbacks Callbacks => FGAudioAdsManager.Instance.Callbacks;
        
        public static void PlaySkippableAd(bool useImage,float skipCountdown = 5)
        {
            FGAudioAdsManager.Instance.PlaySkippable(useImage, skipCountdown);
        }

        public static void PlayRewardedAd(bool useImage)
        {
            FGAudioAdsManager.Instance.PlayRewarded(useImage);
        }
    }
}