namespace FunGames.Notifications
{
    public class FGNotification
    {
        public static FGNotificationsCallbacks Callbacks => FGNotificationManager.Instance.Callbacks;
        
        public static void RemoveAllNotifs() => FGNotificationManager.Instance.RemoveAllNotifs();
        
        public static void RemoveNotifById(int id) => FGNotificationManager.Instance.RemoveNotif(id);


#if UNITY_ANDROID
        public static void CreateTimeIntervalNotifs(string a_Title, string a_Text, string a_ChannelId, int a_id,
            int a_hours = 0, int a_minutes = 0, int a_seconds = 0)
        {
            FGNotificationManager.Instance.CreateTimeIntervalNotifs(a_Title, a_Text, a_ChannelId, a_id, a_hours, a_minutes, a_seconds);
        }
#elif UNITY_IOS
        public static void CreateTimeIntervalNotifs(int hours, int minutes, int seconds, string a_Identifier, string a_Title, string a_Body, string a_Subtitle, string a_CategoryIdentifier
 = "Category_1", string a_ThreadIdentifier = "thread1")
        {
             FGNotificationManager.Instance.CreateTimeIntervalNotifs(hours, minutes, seconds, a_Identifier, a_Title, a_Body, a_Subtitle, a_CategoryIdentifier, a_ThreadIdentifier);
        }
#endif
    }
}