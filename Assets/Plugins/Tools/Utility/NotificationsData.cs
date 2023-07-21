using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Notification Data", menuName = "Scriptable Object / Notification Data")]
public class NotificationsData : ScriptableObject
{
    [Title("Notification Data", "Contains all Notification Data", TitleAlignments.Centered, Bold = true)] [Space(15)]
    public List<NotificationDetail> notifications;

    [Serializable]
    public class NotificationDetail
    {
        public string title;
        public string description;
        public int deliveryTimeInMinutes;

        public NotificationDetail(string title, string description, int deliveryTimeInMinutes)
        {
            this.title = title;
            this.description = description;
            this.deliveryTimeInMinutes = deliveryTimeInMinutes;
        }
    }
}