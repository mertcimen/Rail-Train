#if !DISABLE_SRDEBUGGER
using System.ComponentModel;
using UnityEngine;

public class GameSROptions
{
    private static GameSROptions _instance;
    public static GameSROptions Instance => _instance ??= new GameSROptions();

    [Category("Game")]
    public float TimeScale
    {
        get => Time.timeScale;
        set => Time.timeScale = value;
    }

    [Category("Save")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared successfully!");
    }

    [Category("Economy")]
    public void AddCurrency()
    {
        DataManager.Currency += 1000;
    }

    [Category("Economy")]
    public int Currency
    {
        get => DataManager.Currency;
        set => DataManager.Currency = value;
    }

    [Category("Levels")]
    public int Level
    {
        get => DataManager.CurrentLevelIndex;
        set => DataManager.CurrentLevelIndex = value;
    }

    [Category("Levels")]
    public void ReloadScene()
    {
        LevelManager.ReloadScene();
    }

    [Category("Levels")]
    public void FailLevel()
    {
        GameManager.Instance.LevelFinish(false);
    }

    [Category("Levels")]
    public void CompleteLevel()
    {
        GameManager.Instance.LevelFinish(true);
    }

    [Category("Notifications")]
    public void TestNotifications()
    {
        NotificationManager.SendNotification(new NotificationsData.NotificationDetail("Test", "Test Notification", 1), 999);
    }

    [Category("Ad Test")]
    public void RequestInterstitial()
    {
        AdManager.ShowInterstitial("test_ad");
    }

    [Category("Ad Test")]
    public void RequestRewarded()
    {
        AdManager.ShowRewarded("test_ad", null);
    }
    
    [Category("Ad Test")]
    public void RequestBanner()
    {
        AdManager.ActivateBanner();
    }
}

#endif