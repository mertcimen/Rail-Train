using System;
using UnityEngine;
using UnityEngine.Networking;

namespace FunGames.Tools.Core.EditorWindow
{
    public class EditorWebRequest
    {
        
        private static int _requestCounter = 0;
        
        public static void SendRequest(UnityWebRequest[] webRequests, Action<UnityWebRequest> callback)
        {
            // Debug.Log("Send request to url : " + webRequests[_requestCounter].url);
            webRequests[_requestCounter].SendWebRequest().completed +=
                (req) => { RequestCompleted(webRequests, callback); };
        }
        
        public static UnityWebRequest SimpleRequest(string url)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            return webRequest;
        }
        
        public static UnityWebRequest DownloadFileRequest(string url, string destinationPath)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            webRequest.downloadHandler = new DownloadHandlerFile(destinationPath);
            return webRequest;
        }

        private static void RequestCompleted(UnityWebRequest[] webRequests, Action<UnityWebRequest> callback)
        {
            Debug.Log(webRequests[_requestCounter].url);
            if (webRequests[_requestCounter].result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request " + _requestCounter + " succeeded!");
                callback?.Invoke(webRequests[_requestCounter]);
                _requestCounter = 0;
            }
            else
            {
                if (_requestCounter.Equals(webRequests.Length - 1))
                {
                    Debug.Log("Request " + _requestCounter + " failed! Stop trying.");
                    _requestCounter = 0;
                }
                else
                {
                    Debug.Log("Request " + _requestCounter + " failed! Trying again...");
                    webRequests[_requestCounter].Abort();
                    webRequests[_requestCounter].Dispose();
                    _requestCounter++;
                    SendRequest(webRequests, callback);
                }
            }
        }
    }
}