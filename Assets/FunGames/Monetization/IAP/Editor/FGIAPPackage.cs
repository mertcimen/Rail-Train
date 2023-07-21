using FunGames.Tools.Debugging.Editor;

namespace FunGames.Monetization.IAP.Editor
{
    public class FGIAPPackage : FGPackageAbstract<FGIAPPackage>
    {
        public override string JsonName => "";
        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.IAP;
    }
}