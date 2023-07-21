using System.Collections.Generic;

namespace FunGames.Tools.Debugging.Editor
{
    public class FGToolsPackage : FGPackageAbstract<FGToolsPackage>
    {
        public override string JsonName => "Main.json";
        public override string ModulePath => FGPackageFolders.TOOLS;

        protected override string[] assetFolders()
        {
            List<string> folders = new List<string>();
            folders.AddRange(base.assetFolders());
            folders.Add("Assets/RestClient");
            folders.Add( "Assets/Resources/FunGames/Debug");
            return folders.ToArray();
        }

        protected override string settingsAsset()
        {
            return "Assets/Resources/" + FGMainSettings.PATH + ".asset";
        }
    }
}