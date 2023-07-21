    using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace FunGames.Notifications
{
    public class FGNotificationCSV : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private TextAsset notifsCsvFile; // Reference of CSV file
        private static TextAsset notifsCsvFileStatic;
        private static char lineSeperator = '\r'; // It defines line seperate character
        private static char fieldSeperator = ','; // It defines field seperate chracter
        private static bool isOnAGO = false;

        private bool SendNotifCohorts = true;

        public class NotificationFG
        {
            public int id;
            public string title;
            public string body;
            public int hours;
            public int minutes;
            public int seconds;
            public string category;

            public void sendNotification()
            {
#if UNITY_ANDROID
                FGNotification.CreateTimeIntervalNotifs(title, body, "channel_id", id, hours, minutes, seconds);
#elif UNITY_IOS
            FGNotification.CreateTimeIntervalNotifs(hours, minutes, seconds, id.ToString(), title, body, null );
#endif
            }
        }

        private Dictionary<string, List<NotificationFG>> listNotif;

        // Start is called before the first frame update
        void Start()
        {
            isOnAGO = true;
            ParseNotification();
            SendAbtestNotification();
        }

        /// <summary>
        /// Function use to Parse the csv file store in notifsCsvFile into a dictionnary listNotif where the key is the category and the value is the list of notification corresponding to category
        /// </summary>
        public void ParseNotification()
        {
            try
            {
                listNotif = new Dictionary<string, List<NotificationFG>>();
                string[] lines = notifsCsvFile.text.Split(lineSeperator);
                foreach (string line in lines)
                {
                    // Creation of the notification
                    string l = line;
                    if (line.StartsWith("\n"))
                        l = line.Substring(1);
                    string[] cells = l.Split(fieldSeperator);
                    Debug.Log("id = " + cells[0] + " Title = " + cells[1] + " Body = " + cells[2].Replace("\"", "") +
                              " hours = " + cells[3] + " minutes = " + cells[4] + " seconds = " + cells[5] +
                              " category = " + cells[6]);
                    NotificationFG n = new NotificationFG();
                    int.TryParse(cells[0], out n.id);
                    n.title = cells[1];
                    n.body = cells[2].Replace("\"", "");
                    int.TryParse(cells[3], out n.hours);
                    int.TryParse(cells[4], out n.minutes);
                    int.TryParse(cells[5], out n.seconds);
                    n.category = cells[6];

                    // Add it to the list of notification
                    if (listNotif.ContainsKey(n.category))
                    {
                        listNotif[n.category].Add(n);
                    }
                    else
                    {
                        listNotif.Add(n.category, new List<NotificationFG>());
                        listNotif[n.category].Add(n);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                // throw;
            }
        }

        public void OnAfterDeserialize()
        {
            notifsCsvFileStatic = notifsCsvFile;
            Debug.Log("AfterDeserialize func call to set Notification Static file for checking in post process");
        }

#if UNITY_EDITOR
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (!isOnAGO) return;
            bool fail = false;
            Dictionary<string, List<NotificationFG>> listNotif = new Dictionary<string, List<NotificationFG>>();
            string[] lines = notifsCsvFileStatic.text.Split(lineSeperator);
            foreach (string line in lines)
            {
                string l = line;
                if (line.StartsWith("\n"))
                    l = line.Substring(1);
                string[] cells = l.Split(fieldSeperator);
                if (cells.Length != 7)
                    throw new System.OperationCanceledException(
                        "Build was canceled by the user. The notification CSV file cannot be loaded.Please check it!");
                Debug.Log("id = " + cells[0] + " Title = " + cells[1] + " Body = " + cells[2].Replace("\"", "") +
                          " hours = " + cells[3] + " minutes = " + cells[4] + " seconds = " + cells[5] +
                          " category = " + cells[6]);
                NotificationFG n = new NotificationFG();
                fail = int.TryParse(cells[0], out n.id) ? fail : true;
                n.title = cells[1];
                n.body = cells[2].Replace("\"", "");
                fail = int.TryParse(cells[3], out n.hours) ? fail : true;
                fail = int.TryParse(cells[4], out n.minutes) ? fail : true;
                fail = int.TryParse(cells[5], out n.seconds) ? fail : true;
                n.category = cells[6];
                if (fail)
                    throw new System.OperationCanceledException(
                        "Build was canceled by the user. The notification CSV file cannot be loaded.Please check it!");
                if (listNotif.ContainsKey(n.category))
                {
                    listNotif[n.category].Add(n);
                }
                else
                {
                    listNotif.Add(n.category, new List<NotificationFG>());
                    listNotif[n.category].Add(n);
                }
            }
        }
#endif
        public void SendAbtestNotification()
        {
            // FGNotification.Initialisation += InitCallback;
            // FGNotification.Init();
            InitCallback();
        }

        public void InitCallback()
        {
            FGNotification.RemoveAllNotifs();

            foreach (KeyValuePair<string, List<NotificationFG>> categoryNotif in listNotif)
            {
                categoryNotif.Value[Random.Range(0, categoryNotif.Value.Count)].sendNotification();
            }
        }

        public void OnBeforeSerialize()
        {
            //throw new System.NotImplementedException();
        }
    }
}