using System;
using System.Collections.Generic;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Analytics
{
    public class FGAnalyticsManager : FGModuleParent<FGAnalyticsManager, FGAnalyticsCallbacks>
    {
        public override string ModuleName => "Analytics";
        protected override string RemoteConfigKey => "FGAnalytics";
        public override Color LogColor => FGDebugSettings.settings.Analytics;

        private Action<bool> _onInitialized;

        protected override void InitializeCallbacks()
        {
            _onInitialized = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _onInitialized;
        }

        public void SendProgressionEvent(LevelStatus statusFG, string level, string subLevel = "", int score = -1)
        {
            Callbacks._ProgressionEvent?.Invoke(statusFG, level, subLevel, score);
        }

        public void SendDesignEventSimple(string eventId, float eventValue)
        {
            Callbacks._DesignEventSimple?.Invoke(eventId, eventValue);
        }

        public void SendDesignEventDictio(string eventId, Dictionary<string, object> customFields, float eventValue)
        {
            Callbacks._DesignEventDictio?.Invoke(eventId, customFields, eventValue);
        }

        public void SendAdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement)
        {
            Callbacks._AdEvent?.Invoke(adAction, adType, adSdkName, adPlacement);
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _onInitialized;
        }
    }
}