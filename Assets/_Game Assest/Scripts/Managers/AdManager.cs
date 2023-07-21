using System;
using System.Collections;
using UnityEngine;

public static class AdManager
{
    private static float _lastInterstitialRequestTime;

    public static void ShowInterstitial(string placement)
    {
        if (Time.time < _lastInterstitialRequestTime)
        {
            return;
        }

        // if (DataManager.IsTutorial || TutorialController.NextInterstitialTime > Time.realtimeSinceStartup)
        // {
        //     LogManager.Log($"Player In Tutorial {DataManager.IsTutorial} or Tutorial time " +
        //                    $"{TutorialController.NextInterstitialTime} not meet {Time.realtimeSinceStartup} ");
        //     return;
        // }

        placement += "_interstitial";

        //TODO: REQUEST AD HERE

        _lastInterstitialRequestTime = Time.time + 1;
    }

    public static void ShowRewarded(string placement, Action<bool> onComplete)
    {
        placement += "_rewarded";

#if UNITY_EDITOR
        onComplete?.Invoke(true);
#else
        //TODO: REQUEST AD HERE
#endif
    }

    public static void ActivateBanner()
    {
        GameManager.Instance.StartCoroutine(BannerActivationCheck());
    }

    private static IEnumerator BannerActivationCheck()
    {
        var checkInterval = new WaitForSeconds(.5f);

        //TODO: REQUEST AD HERE
        
        yield return null;
    }
}