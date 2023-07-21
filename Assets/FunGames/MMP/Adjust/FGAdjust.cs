using com.adjust.sdk;
using System;
using FunGames.Analytics;
using FunGames.AuthorizationTracking;
using FunGames.Mediation;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.MMP.AdjustMMP
{
    public class FGAdjust : FGModuleChild<FGAdjust, FGModuleCallbacks>
    {
        public override IFGModuleSettings Settings => FGAdjustSettings.settings;
        public override string ModuleName => "Adjust";
        protected override string RemoteConfigKey => "FGAdjust";
        public override Color LogColor => FGDebugSettings.settings.Adjust;

        protected override IFGModuleParent Parent => FGMMPManager.Instance;

        private const string RC_ADJUST_SANDBOX = "AdjustSandbox";

        private const string PP_PLAYTIME = "playtime";
        private const string PP_NB_SESSION = "nbSession";
        private const string PP_NB_RV = "nbRV";
        private const string PP_SESSION_LENGHT = "sessionLenght";

        private const string PP_ADJUST_TOKEN_RET1 = "AdjustTokenRet1";
        private const string PP_ADJUST_TOKEN_RET3 = "AdjustTokenRet3";
        private const string PP_ADJUST_TOKEN_RET5 = "AdjustTokenRet5";
        private const string PP_ADJUST_TOKEN_RET7 = "AdjustTokenRet7";

        private int daysSinceFirstConnection;
        private int daysSinceLastConnection;
        private int nbSessionToday;
        private int nbRV;

        private Action<string, FGAdInfo> _interImpressionCallback;
        private Action<string, FGAdInfo> _bannerImpressionCallback;
        private Action<string, FGAdInfo> _rewardedImpressionCallback;
        private Action<string, FGAdInfo> _crosspromoImpressionCallback;
        private Action<string, FGAdInfo> _mrecImpressionCallback;

        private static bool _subscribed = false;

        protected override void InitializeCallbacks()
        {
            FGRemoteConfig.AddDefaultValue(RC_ADJUST_SANDBOX, 0);

            FGMMPManager.Instance.Callbacks.Initialization += Initialize;

            _interImpressionCallback = (adUnitId, adInfo) => runCallback("INTER", adInfo);
            _bannerImpressionCallback = (adUnitId, adInfo) => runCallback("BANNER", adInfo);
            _rewardedImpressionCallback = (adUnitId, adInfo) => runCallback("REWARDED", adInfo);
            _crosspromoImpressionCallback = (adUnitId, adInfo) => runCallback("XPROMO", adInfo);
            _mrecImpressionCallback = (adUnitId, adInfo) => runCallback("MREC", adInfo);

            FGMediation.Callbacks.OnInterstitialAdImpression += _interImpressionCallback;
            FGMediation.Callbacks.OnBannerAdImpression += _bannerImpressionCallback;
            FGMediation.Callbacks.OnRewardedAdImpression += _rewardedImpressionCallback;
            FGMediation.Callbacks.OnCrosspromoAdImpression += _crosspromoImpressionCallback;
            FGMediation.Callbacks.OnMrecAdImpression += _mrecImpressionCallback;
            // FGMediation.Callbacks.OnRvInterAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("REWARDED_INTER", adInfo);
            _subscribed = true;
        }

        protected override void InitializeModule()
        {
            AdjustEnvironment environment = GetEnvironment();
            Log("Environment : " + environment);

            AdjustConfig adjustConfig = new AdjustConfig(FGAdjustSettings.settings.AppToken.Trim(), environment);
            adjustConfig.setLogLevel(FGAdjustSettings.settings.logLevel);
            adjustConfig.setAttributionChangedDelegate(attributionChangedDelegate);
            adjustConfig.setPreinstallTrackingEnabled(true);
            adjustConfig.setSendInBackground(FGAdjustSettings.settings.sendInBackground);
            Adjust.start(adjustConfig);
            AdjustThirdPartySharing adjustThirdPartySharing =
                new AdjustThirdPartySharing(FGUserConsent.GdprStatus.TargetedAdvertisingAccepted);
            Adjust.trackThirdPartySharing(adjustThirdPartySharing);

            if (FGRemoteConfig.CurrentABTest != null)
            {
                Adjust.addSessionCallbackParameter("user_cohort", FGRemoteConfig.CurrentABTest.Id);
            }

            Adjust.addSessionCallbackParameter("FunGamesSDK", FGMain.Instance.ModuleVersion);

            if (!FGUserConsent.GdprStatus.AnalyticsAccepted) Adjust.trackMeasurementConsent(false);

            InitializationComplete(!String.IsNullOrEmpty(FGAdjustSettings.settings.AppToken));

            if (String.IsNullOrEmpty(FGAdjustSettings.settings.AppToken))
                LogError("App Token is missing in FG Adjust Settings");

            InitializePlayerPrefs();
        }


        private void InitializePlayerPrefs()
        {
            if (FGMain.Instance.IsFirstConnection()) PlayerPrefs.SetInt(PP_PLAYTIME, 0);

            // Retention
            daysSinceFirstConnection = FGMain.Instance.DaysSinceFirstConnection();
            daysSinceLastConnection = FGMain.Instance.DaysSinceLastConnection();

            if (daysSinceFirstConnection == 1)
            {
                if (!PlayerPrefs.HasKey(PP_ADJUST_TOKEN_RET1))
                {
                    PlayerPrefs.SetInt(PP_ADJUST_TOKEN_RET1, 1);
                    AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustTokenRet1);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 3)
            {
                if (!PlayerPrefs.HasKey(PP_ADJUST_TOKEN_RET3))
                {
                    PlayerPrefs.SetInt(PP_ADJUST_TOKEN_RET3, 3);
                    AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustTokenRet3);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 5)
            {
                if (!PlayerPrefs.HasKey(PP_ADJUST_TOKEN_RET5))
                {
                    PlayerPrefs.SetInt(PP_ADJUST_TOKEN_RET5, 5);
                    AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustTokenRet5);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 7)
            {
                if (!PlayerPrefs.HasKey(PP_ADJUST_TOKEN_RET7))
                {
                    PlayerPrefs.SetInt(PP_ADJUST_TOKEN_RET7, 7);
                    AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustTokenRet7);
                    Adjust.trackEvent(adjustEvent);
                }
            }

            // NB Session
            // if (PlayerPrefs.HasKey(PP_DATE_LAST_CO))
            // {
            //     DateTime store = Convert.ToDateTime(PlayerPrefs.GetString(PP_DATE_LAST_CO), CultureInfo.InvariantCulture);
            //     PlayerPrefs.SetString(PP_DATE_LAST_CO, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //     DateTime today = DateTime.Now;
            //
            //     TimeSpan elapsed = today.Subtract(store);
            //     daysSinceLastConnection = (int)elapsed.TotalDays;
            // }
            // else
            // {
            //     PlayerPrefs.SetString(PP_DATE_LAST_CO, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //     daysSinceLastConnection = 0;
            // }


            if (PlayerPrefs.HasKey(PP_NB_SESSION))
            {
                if (daysSinceLastConnection != 0)
                {
                    PlayerPrefs.SetInt(PP_NB_SESSION, 0);
                    nbSessionToday = 0;
                }
                else
                {
                    nbSessionToday = PlayerPrefs.GetInt(PP_NB_SESSION);
                    nbSessionToday = nbSessionToday + 1;
                    PlayerPrefs.SetInt(PP_NB_SESSION, nbSessionToday);
                }
            }
            else
            {
                PlayerPrefs.SetInt(PP_NB_SESSION, 0);
                nbSessionToday = 0;
            }


            /*if (PlayerPrefs.HasKey("retention"))
            {
                PlayerPrefs.SetInt("retention", PlayerPrefs.GetInt("retention")+1);
            }
            else
            {
                PlayerPrefs.SetInt("retention", 0);
                nbSessionToday = 0;
            }*/

            if (PlayerPrefs.HasKey(PP_NB_RV))
            {
                PlayerPrefs.SetInt(PP_NB_RV, 0);
            }

            PlayerPrefs.SetInt(PP_SESSION_LENGHT, 0);


            if (_subscribed)
            {
                Debug.Log("Ignoring duplicate adjust max subscription");
                return;
            }
        }

        private void runCallback(string format, FGAdInfo adInfo)
        {
            if (format == "INTER")
            {
                AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustEventInterID);
                adjustEvent.setRevenue(0, "USD");
                Adjust.trackEvent(adjustEvent);
            }
            else if (format == "REWARDED")
            {
                nbRV = PlayerPrefs.GetInt(PP_NB_RV) + 1;
                PlayerPrefs.SetInt(PP_NB_RV, nbRV);
                AdjustEvent adjustEvent = new AdjustEvent(FGAdjustSettings.settings.AdjustEventRewardedID);
                adjustEvent.setRevenue(0, "USD");
                Adjust.trackEvent(adjustEvent);
                if (nbRV == FGAdjustSettings.settings.nbRV1)
                {
                    Adjust.trackEvent(new AdjustEvent(FGAdjustSettings.settings.nbRV1Token));
                }
                else if (nbRV == FGAdjustSettings.settings.nbRV2)
                {
                    Adjust.trackEvent(new AdjustEvent(FGAdjustSettings.settings.nbRV2Token));
                }

                else if (nbRV == FGAdjustSettings.settings.nbRV3)
                {
                    Adjust.trackEvent(new AdjustEvent(FGAdjustSettings.settings.nbRV3Token));
                }

                else if (nbRV == FGAdjustSettings.settings.nbRV4)
                {
                    Adjust.trackEvent(new AdjustEvent(FGAdjustSettings.settings.nbRV4Token));
                }

                else if (nbRV == FGAdjustSettings.settings.nbRV5)
                {
                    Adjust.trackEvent(new AdjustEvent(FGAdjustSettings.settings.nbRV5Token));
                }
            }

            // initialise with AppLovin MAX source
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            // set revenue and currency
            adjustAdRevenue.setRevenue(adInfo.Revenue, "USD");
            // optional parameters
            adjustAdRevenue.setAdRevenueNetwork(adInfo.NetworkName);
            adjustAdRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
            adjustAdRevenue.setAdRevenuePlacement(adInfo.Placement);
            // track ad revenue
            Adjust.trackAdRevenue(adjustAdRevenue);
        }

        public void attributionChangedDelegate(AdjustAttribution attribution)
        {
            Log("Attribution changed");
            FGAnalytics.NewDesignEvent("NetworkAttribution:" + attribution.network);
        }

        public static AdjustEnvironment GetEnvironment()
        {
            return FGRemoteConfig.GetBooleanValue(RC_ADJUST_SANDBOX)
                ? AdjustEnvironment.Sandbox
                : AdjustEnvironment.Production;
        }

        protected override void ClearInitialization()
        {
            FGMMPManager.Instance.Callbacks.Initialization -= Initialize;
            FGMediation.Callbacks.OnInterstitialAdImpression -= _interImpressionCallback;
            FGMediation.Callbacks.OnBannerAdImpression -= _bannerImpressionCallback;
            FGMediation.Callbacks.OnRewardedAdImpression -= _rewardedImpressionCallback;
            FGMediation.Callbacks.OnCrosspromoAdImpression -= _crosspromoImpressionCallback;
            FGMediation.Callbacks.OnMrecAdImpression -= _mrecImpressionCallback;
        }
        //#endif
    }
}