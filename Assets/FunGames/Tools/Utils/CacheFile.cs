using System.IO;
using SimpleJSON;
using UnityEngine;

namespace FunGames.Tools.Utils
{
    public class CacheFile<T> where T : class
    {
        private string _fileName;
        private T _data;

        public string Path => Application.persistentDataPath + _fileName + ".json";
        public T Data => _data != null ? _data : Read();

        public CacheFile(string fileName, T data)
        {
            _fileName = fileName;
            _data = data;
        }

        public T Read()
        {
            if (!File.Exists(Path))
            {
                Debug.LogWarning(Path + "doesn't exist !");
                return null;
            }

            string content = File.ReadAllText(Path);
            return JsonUtility.FromJson<T>(content);
        }

        public void Create()
        {
            if (File.Exists(Path))
            {
                Debug.LogWarning(Path + "already exist !");
                return;
            }
            
            var json = new JSONObject();
            json.Add("", new JSONObject());
            File.WriteAllText(Path, json.ToString());
        }

        public void Update(T data)
        {
            File.WriteAllText(Path, JsonUtility.ToJson(data));
        }
    }
}