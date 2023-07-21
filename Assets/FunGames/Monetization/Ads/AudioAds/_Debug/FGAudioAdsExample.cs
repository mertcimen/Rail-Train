using FunGames.Monetization.AudioAds;
using UnityEngine;

public class FGAudioAdsExample : MonoBehaviour
{
    public void PlaySkippableAudioWithImage()
    {
        FGAudioAds.PlaySkippableAd(true, 3);
    }
    
    public void PlayRewardedAudioWithImage()
    {
        FGAudioAds.PlayRewardedAd(true);
    }
    
    public void PlaySkippableAudioWithoutImage()
    {
        FGAudioAds.PlaySkippableAd(false, 3);
    }
    
    public void PlayRewardedAudioWithoutImage()
    {
        FGAudioAds.PlayRewardedAd(false);
    }
}
