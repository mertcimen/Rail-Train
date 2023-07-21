using FunGames.Tools.Debugging.Editor;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR.Editor
{
    public class FGCustomGdprPackage : FGPackageAbstract<FGCustomGdprPackage>
    {
        public override string JsonName => "CustomGDPR.json";
        public override string ModulePath  => FGPackageFolders.TRACKING_AUTHORIZATION + "/GDPR/CustomGDPR";
    }
}