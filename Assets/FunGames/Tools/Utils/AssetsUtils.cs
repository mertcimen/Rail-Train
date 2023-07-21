#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace FunGames.Tools.Utils
{
    public class AssetsUtils
    {
        public static List<T> GetAssets<T>(string[] foldersToSearch, string filter = "") where T : UnityEngine.Object
        {
            List<string> validFolders = new List<string>();
            foreach (var folder in foldersToSearch)
            {
                if (AssetDatabase.IsValidFolder(folder)) validFolders.Add(folder);
            }

            string[] guids = AssetDatabase.FindAssets(filter, validFolders.ToArray());
            List<T> a = new List<T>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a.Add(AssetDatabase.LoadAssetAtPath<T>(path));
            }

            return a;
        }

        public static List<string> GetAssetsPath(string[] foldersToSearch, string filter = "")
        {
            List<string> validFolders = new List<string>();
            foreach (var folder in foldersToSearch)
            {
                if (AssetDatabase.IsValidFolder(folder)) validFolders.Add(folder);
            }

            string[] guids = AssetDatabase.FindAssets(filter, validFolders.ToArray());
            List<string> a = new List<string>();
            for (int i = 0; i < guids.Length; i++)
            {
                a.Add(AssetDatabase.GUIDToAssetPath(guids[i]));
            }

            return a;
        }
    }
}
#endif