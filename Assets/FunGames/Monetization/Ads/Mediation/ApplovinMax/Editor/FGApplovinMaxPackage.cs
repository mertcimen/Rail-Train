using FunGames.Tools.Debugging.Editor;
using FunGames.Tools.Utils;

namespace FunGames.Mediation.ApplovinMax
{
    public class FGApplovinMaxPackage : FGPackageAbstract<FGApplovinMaxPackage>
    {
        public override string JsonName => "ApplovinMax.json";
        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.ADS + "/" +
                                             FGPackageFolders.MEDIATION + "/ApplovinMax";
        
        protected override string[] externalAssets()
        {
            return AssetsUtils.GetAssetsPath(FUNGAMES_EXTERNALS_PATH, "Applovin").ToArray();
        }
    }
}