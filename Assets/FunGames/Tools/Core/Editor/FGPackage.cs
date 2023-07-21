using System.Collections.Generic;
using UnityEngine;

namespace FunGames.Tools.Debugging.Editor
{
    public abstract class FGPackage
    {
        public abstract string ModuleName { get; }
        public abstract string PackageName { get; }
        public abstract string PackageURL { get; }
        public abstract string[] AssetFolders { get; }
        public abstract string SettingsAsset { get; }
        public abstract string[] ExternalAssets { get; }
        public abstract string DestinationPath { get; }
        public abstract List<GameObject> Prefabs { get; }
        public abstract string JsonFile { get; }
        
        public abstract string GetModuleVersion();
        
        public abstract void AddPrefabsAndSettings();
        
        public abstract void UpdateSettings();
    }
}