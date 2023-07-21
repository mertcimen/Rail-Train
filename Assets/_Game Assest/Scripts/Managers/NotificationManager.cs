using System;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private NotificationsData data;

    public NotificationManager Initialize()
    {
        return this;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ScheduleNotifications();
        else
            CancelAllNotifications();
    }

    private void ScheduleNotifications()
    {
        foreach (var notification in data.notifications)
        {
            var index = data.notifications.FindIndex(x => x == notification);
           
            SendNotification(notification, index);
        }
    }

    public static void SendNotification(NotificationsData.NotificationDetail detail, int index = 0)
    {
#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "highrise_games",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        
        var notification = new AndroidNotification
        {
            Title = detail.title,
            Text = detail.description,
            FireTime = System.DateTime.Now.AddMinutes(detail.deliveryTimeInMinutes)
        };

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_id", index);
#elif UNITY_IOS
        var identifier = "_" + Application.identifier + "_" + index;

        iOSNotificationCenter.RemoveScheduledNotification(identifier);
        iOSNotificationCenter.ScheduleNotification(new iOSNotification()
        {
            Title = detail.title,
            Body = detail.description,
            Identifier = identifier,
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                Repeats = false,
                TimeInterval = new TimeSpan(0, detail.deliveryTimeInMinutes, 0),
            }
        });
#endif

        var deliveryTime = DateTime.Now.AddMinutes(detail.deliveryTimeInMinutes);
        LogManager.Log($"Notification {index} scheduled for {deliveryTime}");
    }

    private static void CancelAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
    }
}