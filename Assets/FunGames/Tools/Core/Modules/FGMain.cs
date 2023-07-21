using System;
using System.Globalization;
using FunGames;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using FunGames.Tools.Utils;
using UnityEngine;

[DefaultExecutionOrder(-40)]
public class FGMain : FGModuleParent<FGMain, FGModuleCallbacks>
{
    // Start is called before the first frame update
    public override string ModuleVersion => FGDebugSettings.settings.Version;
    public override string ModuleName => "SDK";
    protected override string RemoteConfigKey => "FunGamesSDK";
    public override Color LogColor => Color.black;

    [HideInInspector] public float TotalModules = 0;
    private int _moduleCount = 0;

    private const string PP_NO_ADS = "noAds";
    private const string PP_DATE_FIRST_CO = "dateFirstCo";
    private const string PP_DATE_LAST_CO = "dateLastCo";

    private bool _isNoAds = false;
    private bool _isFirstConnection = false;
    private int _daysSinceFirstConnection = 0;
    private int _daysSinceLastConnection = 0;

    protected override void InitializeCallbacks()
    {
        InitWithoutTimer();
    }

    protected override void OnAwake()
    {
        TotalModules = ChildrenToInitialize;
        _isNoAds = PlayerPrefs.HasKey(PP_NO_ADS);
    }

    protected override void OnStart()
    {
        base.OnStart();
        Initialize();
    }

    protected override void InitializeModule()
    {
        DateTime today = DateTime.Now;
        DateTime dateFirstCo;
        DateTime dateLastCo;

        _isFirstConnection = !PlayerPrefs.HasKey(PP_DATE_FIRST_CO);
        if (_isFirstConnection)
        {
            string dateAsString = today.ToString(CultureInfo.InvariantCulture);
            PlayerPrefs.SetString(PP_DATE_FIRST_CO, dateAsString);
            Log("Save Date First Connection : " + dateAsString);
            dateFirstCo = today;
            dateLastCo = today;
        }
        else
        {
            dateFirstCo = DateUtils.ConvertInvariant(PlayerPrefs.GetString(PP_DATE_FIRST_CO));
            dateLastCo = DateUtils.ConvertInvariant(PlayerPrefs.GetString(PP_DATE_LAST_CO));
        }

        _daysSinceFirstConnection = (int)today.Subtract(dateFirstCo).TotalDays;
        _daysSinceLastConnection = (int)today.Subtract(dateLastCo).TotalDays;

        PlayerPrefs.SetString(PP_DATE_LAST_CO, today.ToString(CultureInfo.InvariantCulture));
        Log("Day since first connection : " + _daysSinceFirstConnection);
        Log("Day since last connection : " + _daysSinceLastConnection);

        FGRemoteConfigManager.Instance.InitializeDefaultValues();
    }

    public void RemoveAds()
    {
        Log("Ads removed !");
        PlayerPrefs.SetInt(PP_NO_ADS, 1);
        _isNoAds = true;
    }

    public bool IsNoAd()
    {
        return _isNoAds;
    }


    public bool IsFirstConnection()
    {
        return _isFirstConnection;
    }

    public int DaysSinceFirstConnection()
    {
        return _daysSinceFirstConnection;
    }

    public int DaysSinceLastConnection()
    {
        return _daysSinceLastConnection;
    }

    protected override void ClearInitialization()
    {
        TotalModules = 0;
    }
}