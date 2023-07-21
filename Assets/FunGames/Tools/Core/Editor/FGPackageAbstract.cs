#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FunGames.Tools.Utils;
using UnityEditor;
using UnityEngine;

namespace FunGames.Tools.Debugging.Editor
{
    public abstract class FGPackageAbstract<T> : FGPackage where T : FGPackageAbstract<T>, new()
    {
        public abstract string JsonName { get; }

        public abstract string ModulePath { get; }

        public override string ModuleName => _moduleName;
        public override string PackageName => _packageName;
        public override string PackageURL => _packageURL;
        public override string[] AssetFolders => _assetFolders;
        public override string SettingsAsset => _settingsAsset;

        public override string[] ExternalAssets => _externalAssets;
        public override string DestinationPath => _destinationPath;
        public override List<GameObject> Prefabs => _prefabs;
        public override string JsonFile => _jsonFile;

        protected string[] FUNGAMES_EXTERNALS_PATH = {"Assets/FunGames_Externals"};

        protected FGModuleInfo _moduleInfo;
        protected string _moduleName;
        protected string _packageName;
        protected string[] _assetFolders;
        protected string _settingsAsset;
        protected string[] _externalAssets;
        protected string _destinationPath;
        protected List<GameObject> _prefabs;
        protected string _jsonFile;
        protected string _packageURL;

        private static T instance = null;

        public static T Package
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        protected FGPackageAbstract()
        {
            _jsonFile = Application.dataPath + "/FunGames/" + ModulePath + "/" + JsonName;
            _moduleInfo = getModuleInfo();
            _moduleName = _moduleInfo.Name;
            _packageURL = _moduleInfo.PackageURL;
            _packageName = getPackageName();
            _assetFolders = assetFolders();
            _settingsAsset = settingsAsset();
            _externalAssets = externalAssets();
            _destinationPath = destinationFolder();
            _prefabs = prefabAssets();
        }

        protected virtual string[] externalAssets()
        {
            return new string[] { };
        }

        protected FGModuleInfo getModuleInfo()
        {
            if (String.IsNullOrEmpty(JsonName)) return new FGModuleInfo();

            if (!File.Exists(_jsonFile))
            {
                Debug.LogWarning("Module info JSON file not found for this module.");
                return new FGModuleInfo();
            }

            return JsonUtility.FromJson<FGModuleInfo>(File.ReadAllText(_jsonFile));
        }

        protected string getPackageName()
        {
            return "FG" + _moduleInfo.Name;
        }

        protected virtual string[] assetFolders()
        {
            string[] folders =
            {
                "Assets/FunGames/" + ModulePath,
                "Assets/FunGames/" + ModulePath + "/Editor",
                "Assets/FunGames/" + ModulePath + "/_Debug",
                "Assets/FunGames/" + ModulePath + "/_Package"
            };
            return folders;
        }

        protected virtual string settingsAsset()
        {
            return "Assets/Resources/FunGames/" + _packageName + "Settings.asset";
        }


        protected List<GameObject> prefabAssets()
        {
            List<GameObject> prefabs = new List<GameObject>();
#if UNITY_EDITOR
            string[] folder = assetFolders();
            string[] toolFolder = { "Assets/FunGames/Tools" };
            prefabs.AddRange(AssetsUtils.GetAssets<GameObject>(toolFolder, "FunGameSDK t:prefab"));
            prefabs.AddRange(AssetsUtils.GetAssets<GameObject>(folder, _packageName + " t:prefab"));
#endif
            return prefabs;
        }

        protected string destinationFolder()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("/");
            stringBuilder.Append(ModulePath);
            return stringBuilder.ToString();
        }

        // private static string Path(params string[] root)
        // {
        //     StringBuilder stringBuilder = new StringBuilder();
        //     for (int i = 0; i < root.Length; i++)
        //     {
        //         if (i != 0) stringBuilder.Append("/");
        //         stringBuilder.Append(root[i]);
        //     }
        //
        //     return stringBuilder.ToString();
        // }

        public override string GetModuleVersion()
        {
            return getModuleInfo().Version;
        }

        public override void AddPrefabsAndSettings()
        {
            _prefabs = prefabAssets();
            AddPrefabsAndSettings(_prefabs);
        }

        public override void UpdateSettings()
        {
            string settingsFileName = Path.GetFileNameWithoutExtension(_settingsAsset);
            if (File.Exists(_settingsAsset))
            {
                // string[] asset = { _settingsAsset };
                var settingAsset = AssetDatabase.LoadAssetAtPath<IFGModuleSettings>(_settingsAsset);
                settingAsset.Version = GetModuleVersion();
                Debug.Log(settingsFileName + " updated !");
            }
        }

        protected void AddPrefabsAndSettings(List<GameObject> prefabs)
        {
            if (prefabs.Count == 0)
            {
                Debug.LogError("Prefabs couldn't be added for " + _packageName);
                return;
            }

            foreach (var prefab in prefabs)
            {
                if (prefab == null)
                {
                    Debug.LogError("Prefabs not found for " + _packageName);
                    return;
                }
            }

            SceneBuilder.AddPrefabsToScene(prefabs);
            CreateSettingsAsset();
        }

        private void CreateSettingsAsset()
        {
            string settingsFileName = Path.GetFileNameWithoutExtension(_settingsAsset);
            if (File.Exists(_settingsAsset))
            {
                Debug.Log(settingsFileName + " already exists.");
                UpdateSettings();
                return;
            }
            
            try
            {
                IFGModuleSettings settings = (IFGModuleSettings)ScriptableObject.CreateInstance(settingsFileName);
                settings.Version = GetModuleVersion();
                AssetDatabase.CreateAsset(settings, _settingsAsset);
                UpdateSettings();
            }
            catch (Exception e)
            {
                Debug.LogError("An exception occured while attempting to create " + settingsFileName + " : \n" + e);
            }
        }
    }
}

#endif