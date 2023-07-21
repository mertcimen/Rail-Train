using System.Collections.Generic;

namespace FunGames.Analytics
{
    public static class FGAnalytics
    {
        public static FGAnalyticsCallbacks Callbacks => FGAnalyticsManager.Instance.Callbacks;

        public static void NewProgressionEvent(LevelStatus status, string level, string subLevel = "", int score = -1)
        {
            FGAnalyticsManager.Instance.SendProgressionEvent(status, level, subLevel, score);
        }

        public static void NewDesignEvent(string eventId, float eventValue = 0)
        {
            FGAnalyticsManager.Instance.SendDesignEventSimple(eventId, eventValue);
        }
        
        public static void NewDesignEvent(string eventId, Dictionary<string, object> customFields, float eventValue = 0)
        {
            FGAnalyticsManager.Instance.SendDesignEventDictio(eventId, customFields, eventValue);
        }

        public static void NewAdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement)
        {
            FGAnalyticsManager.Instance.SendAdEvent(adAction, adType, adSdkName, adPlacement);
        }
    }
    
    public enum LevelStatus
    {
        Start = 1,
        Complete = 2,
        Fail = 3
    };

    public enum AdAction
    {
        Clicked = 1,
        FailedShow = 2,
        Loaded = 3,
        Request = 4,
        RewardReceived = 5,
        Show = 6,
        Undefined = 7,
        Dismissed = 8,
        Impression = 9
    };

    public enum AdType
    {
        Banner = 1,
        Interstitial = 2,
        OfferWall = 3,
        Playable = 4,
        RewardedVideo = 5,
        Video = 6,
        Undefined = 7,
        AppOpen = 8
    };

}
