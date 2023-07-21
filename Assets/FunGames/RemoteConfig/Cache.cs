using System.IO;
using SimpleJSON;
using UnityEngine;

namespace FunGames.RemoteConfig
{
    public static class Cache
    {
        public static string CachePath = Path.Combine(Application.persistentDataPath, "remoteConfig.json");

        public static void Initialize()
        {
            CheckInitialization();
            // Debug.Log(CachePath);
        }

        public static bool IsVariableCached(string key)
        {
            if (key == null)
                return false;
            CheckInitialization();

            var cachedJson = JSON.Parse(File.ReadAllText(CachePath));

            return cachedJson.HasKey(key);
        }

        public static JSONNode GetRoot()
        {
            CheckInitialization();
            return JSON.Parse(File.ReadAllText(CachePath));
        }

        public static void SaveCache(JSONNode cacheJson)
        {
            File.WriteAllText(CachePath, cacheJson.ToString());
        }

        public static void AddNode(JSONNode node, string key = "")
        {
            if (node == null)
                return;
            CheckInitialization();

            var cachedJson = JSON.Parse(File.ReadAllText(CachePath));

            cachedJson.Add(key, node);
            File.WriteAllText(CachePath, cachedJson.ToString());
        }

        public static void RemoveNode(string key)
        {
            if (key == null) return;
            
            CheckInitialization();

            var cachedJson = JSON.Parse(File.ReadAllText(CachePath));

            cachedJson.Remove(key);
            File.WriteAllText(CachePath, cachedJson.ToString());
        }

        private static void CheckInitialization()
        {
            if (File.Exists(CachePath)) return;
            var json = new JSONObject();
            json.Add("", new JSONObject());
            File.WriteAllText(CachePath, json.ToString());
        }
    }
}