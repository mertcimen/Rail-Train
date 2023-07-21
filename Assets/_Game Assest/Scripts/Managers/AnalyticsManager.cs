using System.Collections.Generic;
using System.Linq;

public static class AnalyticsManager
{
    private static int _lastPlayedLevelIndex;
    
    public static void Initialize()
    {

    }

    public static void OnLevelStart(Dictionary<string, object> extraParams = null)
    {
        var lvlIndex = DataManager.CurrentLevelIndex;
        var lvlId = GameManager.Instance.levelManager.currentLevelData.levelId;

        var startType = _lastPlayedLevelIndex == lvlIndex ? StartType.Retry : StartType.First;

        _lastPlayedLevelIndex = lvlIndex;

        // AnalyticsService.Instance.CustomData("onLevelStarted", new Dictionary<string, object>()
        // {
        //     { "levelIndex", lvlIndex},
        //     { "levelId", lvlId},
        //     { "startType", startType.ToString()}
        // }); 
        
        LogManager.Log($"AnalyticsHelper OnLevelStarted - Level index:{lvlIndex}, Level id:{lvlId} " +
                       $"Start type:{startType}");
    }

    public static void OnLevelFinish(EndType endType, Dictionary<string, object> extraParams = null)
    {
        var lvlIndex = DataManager.CurrentLevelIndex;
        var lvlId = GameManager.Instance.levelManager.currentLevelData.levelId;

        // AnalyticsService.Instance.CustomData("onLevelFinish", new Dictionary<string, object>()
        // {
        //     { "levelIndex", lvlIndex},
        //     { "levelId", lvlId},
        // }); 
        
        LogManager.Log($"AnalyticsHelper OnLevelEnded - Level index:{lvlIndex}, Level id:{lvlId} " +
                       $"End type:{endType}, extra params:{extraParams}");
    }
    
    public static void OnProgress(string eventName, Dictionary<string, object> parameters = null)
    {
        var parametersString = "Null";

        if (parameters is not null)
            parametersString = parameters.Aggregate("",
                (current, param) => current + $"{System.Environment.NewLine} Key : {param.Key}, Value : {param.Value}");

        // AnalyticsService.Instance.CustomData("onProgress", parameters); 
        
        LogManager.Log($"AnalyticsHelper OnProgress - Event Name :{eventName} , Parameters :{parametersString}");
    }
}

public enum StartType
{
    First,
    Retry,
    Continue,
}

public enum EndType
{
    Success,
    Fail,
    Quit,
    Skip,
}