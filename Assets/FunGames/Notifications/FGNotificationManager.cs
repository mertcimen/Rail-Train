using System;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Notifications
{
    public class FGNotificationManager : FGModuleParent<FGNotificationManager, FGNotificationsCallbacks>
    {
        public override string ModuleName => "Notification";
        protected override string RemoteConfigKey => "FGNotifications";
        public override Color LogColor => FGDebugSettings.settings.Notifications;

        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
            Callbacks.OnInitialized += delegate { GetNotifUserOpenAppWith(); };
        }

        public void RemoveAllNotifs()
        {
            Callbacks._RemoveAllNotifs?.Invoke();
        }

        public void RemoveNotif(int id)
        {
            Callbacks._RemoveNotifById?.Invoke(id);
        }

        public void GetNotifUserOpenAppWith()
        {
            Callbacks._GetNotifUserOpenAppWith?.Invoke();
        }

#if UNITY_IOS
    public void CreateTimeIntervalNotifs(int hours, int minutes, int seconds, string a_Identifier, string a_Title,
        string a_Body, string a_Subtitle, string a_CategoryIdentifier
            = "Category_1", string a_ThreadIdentifier = "thread1")
    {
        Callbacks._TimeIntervalNotifs?.Invoke(hours, minutes, seconds, a_Identifier, a_Title, a_Body, a_Subtitle,
            a_CategoryIdentifier, a_ThreadIdentifier);
    }

#elif UNITY_ANDROID
        public void CreateTimeIntervalNotifs(string a_Title, string a_Text, string a_ChannelId, int a_id,
            int a_hours = 0, int a_minutes = 0, int a_seconds = 0)
        {
            Callbacks._TimeIntervalNotifs?.Invoke(a_Title, a_Text, a_ChannelId, a_id, a_hours, a_minutes, a_seconds);
        }
#endif

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}