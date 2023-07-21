using System;
using System.Collections.Generic;
using System.Globalization;
using FunGames.Tools.Debugging.Editor;

namespace FunGames.Tools.Core.EditorWindow
{
    [Serializable]
    public class IntegrationManager
    {
        public List<ModuleURLs> ModuleURLs = new List<ModuleURLs>();
    }

    [Serializable]
    public class ModuleURLs
    {
        public string Type;
        public List<string> URLs = new List<string>();
    }
    
    [Serializable]
    public class SoftLaunchModules
    {
        public List<string> ModuleNames = new List<string>();
    }
    
    [Serializable]
    public class RemoteDataHolder
    {
        public string LastUpdateDate;
        // public SoftLaunchModules SoftLaunchModules;
        public List<ModuleListHolder> AllModules;

        public RemoteDataHolder(DateTime time)
        {
            LastUpdateDate = time.ToString(CultureInfo.InvariantCulture);
            AllModules = new List<ModuleListHolder>();
        }
    }

    [Serializable]
    public class ModuleListHolder
    {
        public string Type;
        public List<FGModuleInfo> ModuleList = new List<FGModuleInfo>();

        public ModuleListHolder(string type)
        {
            Type = type;
        }
    }
    
    public class ModuleViewer
    {
        public string localVersion;
        public string remoteVersion;
    }
}