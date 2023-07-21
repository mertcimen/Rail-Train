using FunGames.Mediation;
using UnityEngine;

public class FGApplovinMaxExample : MonoBehaviour
{
    //DebugInterstitial
    private const string InterstitialPlacement = "";
    private const string RewardedPlacement = "DebugRewarded";
    
    private bool _interLoaded = false;
    private bool _rewardedLoaded= false;
    private bool _bannerLoaded= false;

    private void Awake()
    {
        // MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += InterCallback;
        // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += RewardedCallback;
        // MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerCallback;
        //
        // MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += InterCallback;
        // MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += RewardedCallback;
        // MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerCallback;
    }

    public void RequestAmazonAd()
    {
        // FGAmazonWithMax.Instance.RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        FGMediation.ShowInterstitial(InterstitialPlacement); 
    }

    public void ShowRewarded()
    {
        FGMediation.ShowRewarded(null,RewardedPlacement); 
    }
    
    public void ShowBanner()
    {
        FGMediation.ShowBanner();
    }
    
    public void HideBanner()
    {
        FGMediation.HideBanner();
    }
    
    public void ShowMaxDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }

    public void ShowFullDiagnostic()
    {
        // FGMediation.Initialize();
        Debug.Log("************** Applovin Max ***************");  
        CheckMaxSdkInitialisation();
        CheckAdIDs();
        Debug.Log("*************************************************"); 
    }
    
    private void CheckMaxSdkInitialisation()
    {
        AssertionLog(MaxSdk.IsInitialized(), "Max SDK Initialisation");
    }
    
    private void CheckAdIDs()
    {
        AssertionLog(FGMediation.IsInterstitialReady, "Interstitials ");
        AssertionLog(FGMediation.IsRewardedReady, "Rewarded ");
        AssertionLog(FGMediation.IsBannerReady, "Banners ");
    }
    
    private void InterCallback<T>(string a , T b)
    {
        _interLoaded = b is MaxSdk.AdInfo;
        AssertionLog(_interLoaded, "Interstitials ");
    }
    
    private void RewardedCallback<T>(string a , T b)
    {
        _rewardedLoaded = b is MaxSdk.AdInfo;
        AssertionLog(_rewardedLoaded, "Rewarded ");
    }
    
    private void BannerCallback<T>(string a , T b)
    {
        _bannerLoaded = b is MaxSdk.AdInfo;
        AssertionLog(_bannerLoaded, "Banner ");
    }
    
    private void AssertionLog(bool result, string message)
    {
        if (result)
        {
            Debug.Log($"<color=green>{message + ": OK!"}</color>\n");
        }
        else
        {
            Debug.LogError(message + ": NOK!");
        }
      
    }
}
