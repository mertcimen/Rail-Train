using FunGames.Tools.Debugging.Editor;
using FunGames.Tools.Utils;
using UnityEngine;

namespace FunGames.Analytics.GA
{
    public class FGGameAnalyticsPackage : FGPackageAbstract<FGGameAnalyticsPackage>
    {
        public override string JsonName => "GameAnalytics.json";
        public override string ModulePath => FGPackageFolders.ANALYTICS + "/GameAnalytics";
        
        public override void AddPrefabsAndSettings()
        {
            string[] gameAnalyticsFolder = { "Assets/GameAnalytics" };
            var prefabs = _prefabs;
            prefabs.AddRange(AssetsUtils.GetAssets<GameObject>(gameAnalyticsFolder, "GameAnalytics t:prefab"));
            AddPrefabsAndSettings(prefabs);
        }
        
        protected override string[] externalAssets()
        {
            return AssetsUtils.GetAssetsPath(FUNGAMES_EXTERNALS_PATH, "GA_SDK").ToArray();
        }
    }
}