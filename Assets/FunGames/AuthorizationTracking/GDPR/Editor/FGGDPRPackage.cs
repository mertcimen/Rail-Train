using FunGames.Tools.Debugging.Editor;

namespace FunGames.AuthorizationTracking.GDPR.Editor
{
    public class FGGDPRPackage : FGPackageAbstract<FGGDPRPackage>
    {
        public override string JsonName => "";
        public override string ModulePath => FGPackageFolders.TRACKING_AUTHORIZATION + "/GDPR";
    }
}