using FunGames.Tools.Debugging.Editor;

namespace FunGames.Monetization.Ads.Crosspromo
{
    public class FGCrosspromoPackage : FGPackageAbstract<FGCrosspromoPackage>
    {
        public override string JsonName => "";

        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.ADS + "/" +
                                             FGPackageFolders.CROSSPROMO;
    }
}