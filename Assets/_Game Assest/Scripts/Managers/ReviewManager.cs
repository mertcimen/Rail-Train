using System.Collections;
using UnityEngine;

public static class ReviewManager
{
    private const int TargetSecond = 60 * 1 /*(Minutes)*/;
    
    private const string ReviewKey = "Review_State";
    private const string DurationKey = "Review_Duration";

    private static bool ReviewRequested
    {
        get => PlayerPrefs.GetInt(ReviewKey, 0) == 1;
        set => PlayerPrefs.SetInt(ReviewKey, value ? 1 : 0);
    }

    private static int ReviewDuration
    {
        get => PlayerPrefs.GetInt(DurationKey, 0);
        set => PlayerPrefs.SetInt(DurationKey, value);
    }

    public static IEnumerator Start()
    {
        if (ReviewRequested)
            yield break;

        //Don't show review for first 30 seconds
        yield return new WaitForSeconds(30f);

        var checkInterval = new WaitForSeconds(1f);

        while (true)
        {
            yield return checkInterval;

            ReviewDuration++;

            if (ReviewDuration <= TargetSecond)
                continue;
            
            RequestReview();
            yield break;
        }
    }

    public static void RequestReview()
    {
        ReviewRequested = true;

#if UNITY_IOS
        UnityEngine.iOS.Device.RequestStoreReview();
#elif UNITY_ANDROID
        GameManager.Instance.StartCoroutine(GooglePlayReview());
#endif

        LogManager.Log($"Review Requested @{Time.realtimeSinceStartup:N}");
    }

#if UNITY_ANDROID
    private static IEnumerator GooglePlayReview()
    {
        var reviewManager = new Google.Play.Review.ReviewManager();
        
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            LogManager.LogError("Review Request Error",requestFlowOperation.Error.ToString());
            yield break;
        }
        
        var _playReviewInfo = requestFlowOperation.GetResult();
        
        var launchFlowOperation = reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        
        if (launchFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            LogManager.LogError("Review Launch Error",launchFlowOperation.Error.ToString());
            yield break;
        }
    }
#endif
}