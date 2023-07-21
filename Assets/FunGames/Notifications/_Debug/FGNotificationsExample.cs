using System;
using System.Collections;
using FunGames.Notifications;
using UnityEngine;

public class FGNotificationsExample : MonoBehaviour
{
    private const string CHANNEL_ID = "channel_id";
    private const string NOTIF_TITLE = "Test Notif";
    private const string NOTIF_TEXT = "This a test notification";
    private const int NOTIF_ID1 = 1234567890;
    private const int NOTIF_TIME_SECONDS = 10;
    private const int DELAY_FOR_CHECKING = 2;
    
    
    public void SendTestNotification()
    {
        SendTestNotification(NOTIF_ID1);
    }


    private void SendTestNotification(int id)
    {
        Debug.Log("Notification sent! it should be received in " + NOTIF_TIME_SECONDS + " seconds !");
#if UNITY_IOS
        FGNotification.CreateTimeIntervalNotifs(0,0,NOTIF_TIME_SECONDS,id.ToString(),NOTIF_TITLE,NOTIF_TEXT,String.Empty);
#elif UNITY_ANDROID
        FGNotification.CreateTimeIntervalNotifs(NOTIF_TITLE, NOTIF_TEXT, CHANNEL_ID, id, 0, 0,
            NOTIF_TIME_SECONDS);
#endif
    }

    public void SendNotifAndCloseApp()
    {
        SendTestNotification(NOTIF_ID1);
        StartCoroutine(closeApp());
    }

    private IEnumerator closeApp()
    {
        yield return new WaitForSeconds(DELAY_FOR_CHECKING);
        Application.Quit();
    }
    
    public void SendNotifFromCSV()
    {
        FGNotificationCSV fgNotifCsv = FindObjectOfType<FGNotificationCSV>();
        Debug.Log("You should receive 2 notifications !");
        fgNotifCsv.SendAbtestNotification();
    }
}