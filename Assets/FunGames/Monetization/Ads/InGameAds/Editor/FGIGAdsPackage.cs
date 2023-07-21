using FunGames.Tools.Debugging.Editor;

namespace FunGames.Monetization.InGameAds
{
    public class FGIGAdsPackage : FGPackageAbstract<FGIGAdsPackage>
    {
        public override string JsonName => "";
        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.ADS + "/" +
                                             FGPackageFolders.IG_ADS;
    }
}