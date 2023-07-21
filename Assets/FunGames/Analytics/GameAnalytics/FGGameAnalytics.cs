using GameAnalyticsSDK;
using System;
using System.Collections.Generic;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Analytics.GA
{
    public class FGGameAnalytics : FGAnalyticsAbstract<FGGameAnalytics>
    {
        public override IFGModuleSettings Settings => FGGameAnalyticsSettings.settings;
        public override string ModuleName => "GameAnalytics";
        protected override string RemoteConfigKey => "FGGameAnalytics";
        public override Color LogColor => FGDebugSettings.settings.GameAnalytics;

        private bool isInit = false;

        /// <summary>
        /// Private function that initializes all GA elements
        /// </summary>
        protected override void Init()
        {
            if (isInit)
            {
                LogWarning("GameAnalytics is already initialized");
                return;
            }

            isInit = true;

            var gameAnalytics = FindObjectOfType<GameAnalytics>();

            if (gameAnalytics == null)
            {
                throw new Exception("It seems like you haven't instantiated GameAnalytics GameObject");
            }

#if UNITY_IOS
            string gameKey = FGGameAnalyticsSettings.settings.gameAnalyticsIosGameKey.Trim();
            string gameSecretKey = FGGameAnalyticsSettings.settings.gameAnalyticsIosSecretKey.Trim();
            AddOrUpdatePlatform(RuntimePlatform.IPhonePlayer, gameKey, gameSecretKey);
#else
            string gameKey = FGGameAnalyticsSettings.settings.gameAnalyticsAndroidGameKey.Trim();
            string gameSecretKey = FGGameAnalyticsSettings.settings.gameAnalyticsAndroidSecretKey.Trim();
            AddOrUpdatePlatform(RuntimePlatform.Android, gameKey, gameSecretKey);
#endif
            GameAnalytics.SettingsGA.InfoLogBuild = false;
            GameAnalytics.SettingsGA.InfoLogEditor = false;
            GameAnalytics.SettingsGA.SubmitFpsAverage = true;
            GameAnalytics.SettingsGA.SubmitFpsCritical = true;
            GameAnalyticsILRD.SubscribeMaxImpressions();
            GameAnalytics.SetEnabledEventSubmission(FGUserConsent.GdprStatus.AnalyticsAccepted);
            GameAnalytics.Initialize();

            InitializationComplete(!String.IsNullOrEmpty(gameKey) &&
                                   !String.IsNullOrEmpty(gameSecretKey));

            if (String.IsNullOrEmpty(gameKey) || String.IsNullOrEmpty(gameSecretKey))
                LogError("Some Key is missing in FG GameAnalytics Settings");
        }

        /// <summary>
        /// Init the Game Analytic Settings for the game on each plateform is on
        /// </summary>
        /// <param name="platform">Android or iOS</param>
        /// <param name="gameKey">GA Gamekey (public key)</param>
        /// <param name="secretKey">GA Secret Key</param>
        private static void AddOrUpdatePlatform(RuntimePlatform platform, string gameKey, string secretKey)
        {
            if (!GameAnalytics.SettingsGA.Platforms.Contains(platform))
            {
                GameAnalytics.SettingsGA.AddPlatform(platform);
            }

            var index = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);

            GameAnalytics.SettingsGA.UpdateGameKey(index, gameKey);
            GameAnalytics.SettingsGA.UpdateSecretKey(index, secretKey);
            GameAnalytics.SettingsGA.Build[index] = Application.version;
        }

        /// <summary>
        /// Remove the Settings of GA for a plateform
        /// </summary>
        /// <param name="platform">Android or iOS</param>
        private static void RemovePlatform(RuntimePlatform platform)
        {
            if (!GameAnalytics.SettingsGA.Platforms.Contains(platform)) return;

            var index = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
            GameAnalytics.SettingsGA.RemovePlatformAtIndex(index);
        }

        /// <summary>
        /// Private function that sends a Progression Event from GA with FGA data
        /// </summary>
        /// <param name="statusFG"></param>
        /// <param name="level"></param>
        /// <param name="subLevel"></param>
        /// <param name="score"></param>
        protected override void ProgressionEvent(LevelStatus statusFG, string level, string subLevel = "",
            int score = -1)
        {
            GAProgressionStatus status;

            switch (statusFG)
            {
                case LevelStatus.Complete:
                    status = GAProgressionStatus.Complete;
                    break;
                case LevelStatus.Fail:
                    status = GAProgressionStatus.Fail;
                    break;
                default:
                    status = GAProgressionStatus.Start;
                    break;
            }

            if (score == -1)
            {
                GameAnalytics.NewProgressionEvent(status, ValidString(level), ValidString(subLevel));
            }
            else
            {
                GameAnalytics.NewProgressionEvent(status, ValidString(level), ValidString(subLevel), score);
            }
        }

        /// <summary>
        /// Private function that sends a simple Design Event from GA with FGA data
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventValue"></param>
        protected override void DesignEventSimple(string eventId, float eventValue)
        {
            GameAnalytics.NewDesignEvent(ValidString(eventId), eventValue);
        }

        /// <summary>
        /// Private function that sends a Desisgn Event with dictionnary data store in it from GA with FGA data
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="customFields"></param>
        /// <param name="eventValue"></param>
        protected override void DesignEventDictio(string eventId, Dictionary<string, object> customFields)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            foreach (var keyValue in customFields)
            {
                fields.Add(ValidString(keyValue.Key), ValidString(keyValue.Value.ToString()));
            }

            GameAnalytics.NewDesignEvent(ValidString(eventId), fields);
        }

        /// <summary>
        /// Private function that sends a Ad Event from GA with FGA data
        /// </summary>
        /// <param name="adAction"></param>
        /// <param name="adType"></param>
        /// <param name="adSdkName"></param>
        /// <param name="adPlacement"></param>
        protected override void AdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement)
        {
            GAAdAction action;
            switch (adAction)
            {
                case AdAction.Clicked:
                    action = GAAdAction.Clicked;
                    break;
                case AdAction.FailedShow:
                    action = GAAdAction.FailedShow;
                    break;
                case AdAction.Loaded:
                    action = GAAdAction.Loaded;
                    break;
                case AdAction.Request:
                    action = GAAdAction.Request;
                    break;
                case AdAction.RewardReceived:
                    action = GAAdAction.RewardReceived;
                    break;
                case AdAction.Show:
                    action = GAAdAction.Show;
                    break;
                default:
                    action = GAAdAction.Undefined;
                    break;
            }

            GAAdType type;
            switch (adType)
            {
                case AdType.Banner:
                    type = GAAdType.Banner;
                    break;
                case AdType.Interstitial:
                    type = GAAdType.Interstitial;
                    break;
                case AdType.OfferWall:
                    type = GAAdType.OfferWall;
                    break;
                case AdType.Playable:
                    type = GAAdType.Playable;
                    break;
                case AdType.RewardedVideo:
                    type = GAAdType.RewardedVideo;
                    break;
                case AdType.Video:
                    type = GAAdType.Video;
                    break;
                default:
                    type = GAAdType.Undefined;
                    break;
            }

            GameAnalytics.NewAdEvent(action, type, ValidString(adSdkName), ValidString(adPlacement));
        }

        protected override void ClearInitialization()
        {
            base.ClearInitialization();
            RemovePlatform(Application.platform);
        }

        private string ValidString(string str)
        {
            return str.Replace(" ", "_");
        }
    }
}