using System.Collections.Generic;
using FunGames.Analytics;
using UnityEngine;

public class FGDebugAnalyticsExample : MonoBehaviour
{
    public void ProgressionStart()
    {
        FGAnalytics.NewProgressionEvent(LevelStatus.Start, "TestLevel");
    }

    public void ProgressionFail()
    {
        FGAnalytics.NewProgressionEvent(LevelStatus.Fail, "TestLevel");
    }

    public void ProgressionComplete()
    {
        FGAnalytics.NewProgressionEvent(LevelStatus.Complete, "TestLevel");
    }

    public void ProgressionDesign()
    {
        FGAnalytics.NewDesignEvent("this is an event");
    }

    public void ProgressionDesignDictionary()
    {
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        tmp.Add("A key", "A Value");
        tmp.Add("An other key", "An other Value");
        FGAnalytics.NewDesignEvent("DictionnaryEvent", tmp);
    }
}