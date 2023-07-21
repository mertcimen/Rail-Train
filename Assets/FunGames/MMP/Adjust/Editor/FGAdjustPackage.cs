using FunGames.Tools.Debugging.Editor;
using FunGames.Tools.Utils;
using UnityEngine;

namespace FunGames.MMP.AdjustMMP
{
    public class FGAdjustPackage : FGPackageAbstract<FGAdjustPackage>
    {
        public override string JsonName => "Adjust.json";
        public override string ModulePath => FGPackageFolders.MMP + "/Adjust";
        
        public override void AddPrefabsAndSettings()
        {
            string[] adjustFolder = { "Assets/Adjust" };
            var prefabs = _prefabs;
            prefabs.AddRange(AssetsUtils.GetAssets<GameObject>(adjustFolder, "Adjust t:prefab"));
            AddPrefabsAndSettings(prefabs);
        }
        
        protected override string[] externalAssets()
        {
            return AssetsUtils.GetAssetsPath(FUNGAMES_EXTERNALS_PATH, "Adjust").ToArray();
        }
    }
}