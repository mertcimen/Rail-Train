using Proyecto26;
using System;
using System.Collections.Generic;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Core;
using FunGames.Tools.Debugging;
using UnityEngine;
using UnityEngine.Analytics;

namespace FunGames.Analytics.API
{
    [Serializable]
    public class FunGamesTracking
    {
        public string idfa;
        public string bundle_id;
        public string session_id;
        public string os;
        public string build;
        public List<Metrics> metrics;
    }

    [Serializable]
    public class Metrics
    {
        public string evt;
        public string value;
        public string ts;
    }

    public class FGAPIManager : FGAnalyticsAbstract<FGAPIManager>
    {
        private const string AnalyticsUrl = "https://api.tnapps.xyz/v1/tracking";
        private static string _idfa = "";

        public override IFGModuleSettings Settings => FGAPISettings.settings;
        public override string ModuleName => "APIManager";
        protected override string RemoteConfigKey => "FGTapNationAPI";
        public override Color LogColor => FGDebugSettings.settings.API;

        protected override void Init()
        {
            if (!FGUserConsent.GdprStatus.AnalyticsAccepted)
            {
                LogWarning("User didn't allow Analytics. Module will not initialize.");
                InitializationComplete(false);
                return;
            }
            
            var datetimeString = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            if (Application.isEditor == false)
            {
                Application.RequestAdvertisingIdentifierAsync(
                    (advertisingId, trackingEnabled, error) =>
                    {
                        _idfa = FGUserConsent.GdprStatus.TargetedAdvertisingAccepted ? advertisingId : "no_idfa";
                        NewEvent("ga_user", datetimeString);
                    }
                );
            }
            else
            {
                _idfa = "unity-editor";
                NewEvent("ga_user", datetimeString);
            }
            
            NewEvent("Initialization","", InitializationCompleteCallback);
        }
        
        private void InitializationCompleteCallback(bool success)
        {
            if (String.IsNullOrEmpty(FGMainSettings.settings.ApiKey))
            {
                LogError("TapNation's API Key is missing in FGMainSettings.");
            }
            
            InitializationComplete(success);
        }

        protected override void ProgressionEvent(LevelStatus statusFG, string level, string subLevel = "",
            int score = -1)
        {
            NewEvent(statusFG.ToString(), level + subLevel + score);
        }

        protected override void DesignEventSimple(string eventId, float eventValue)
        {
            NewEvent(eventId, eventValue.ToString());
        }

        protected override void DesignEventDictio(string eventId, Dictionary<string, object> customFields)
        {
            NewEvent(eventId, customFields.ToString());
        }

        protected override void AdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement)
        {
            NewEvent(adAction.ToString() + "-" + adType.ToString(), adSdkName + "-" + adPlacement);
        }

        private void NewEvent(string eventName, string value, Action<bool> callback = null)
        {
            var userInfo = GetUserInfo();

            var trackingParams = new FunGamesTracking()
            {
                idfa = userInfo["idfa"],
                bundle_id = userInfo["bundle_id"],
                session_id = userInfo["session_id"],
                os = userInfo["os"],
                build = userInfo["build"],
                metrics = new List<Metrics>
                {
                    new Metrics
                    {
                        evt = eventName,
                        value = value,
                        ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
                    }
                }
            };

            var bodyString = JsonUtility.ToJson(trackingParams);
            var hash = FGAPIHelpers.CreateToken(bodyString);
            var bitString = FGAPIHelpers.GetBitString();

            RestClient.DefaultRequestHeaders["Content-Type"] = "application/json";
            RestClient.DefaultRequestHeaders["Authorization"] = "hmac " + bitString + " " + hash;
            RestClient.DefaultRequestHeaders["User-Agent"] = SystemInfo.deviceModel;

            RestClient.Post(AnalyticsUrl, trackingParams).Then(response =>
            {
                // Debug.Log("API POST");
                callback?.Invoke(true);
                EventReceived(eventName,true);
                //ParseResponse(response.Text);
            }).Catch(err =>
            {
                LogError(err.Message);
                callback?.Invoke(false);
                EventReceived(eventName,true);
            });
        }

        private Dictionary<string, string> GetUserInfo(Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            var userId = AnalyticsSessionInfo.userId;
            var sessionId = AnalyticsSessionInfo.sessionId.ToString();

            parameters.Add("bundle_id", Application.identifier);
            parameters.Add("user_id", userId);
            parameters.Add("session_id", sessionId);
            parameters.Add("idfa", _idfa);
            parameters.Add("os", SystemInfo.operatingSystem);
            parameters.Add("build", Application.version);

            return parameters;
        }
    }
}