using GameAnalyticsSDK;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private static AppManager _instance;
    
    private void Awake()
    {
        if (_instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
        
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }

      

        // Facebook.Unity.FB.Init(FBInitCallback);
    }

    private void Start()
    {
        StartCoroutine(ReviewManager.Start());
        GameAnalytics.Initialize();
    }

    // // ReSharper disable once InconsistentNaming
    // private void FBInitCallback()
    // {
    //     if (Facebook.Unity.FB.IsInitialized)
    //     {
    //         LogManager.Log("Facebook Initialized");
    //         
    //         Facebook.Unity.FB.ActivateApp();
    //     }
    //     else
    //     {
    //         LogManager.LogError("Facebook Initialized Failed", this);
    //     }
    // }
    //
    // private void OnApplicationPause(bool paused)
    // {
    //     if (paused)
    //         return;
    //     
    //     if (Facebook.Unity.FB.IsInitialized)
    //     {
    //         Facebook.Unity.FB.ActivateApp();
    //     }
    // }
}