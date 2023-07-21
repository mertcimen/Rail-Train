using FunGames.Tools.Debugging.Editor;

namespace FunGames.AuthorizationTracking.ATT.UnityATT
{
    public class FGUnityATTPackage : FGPackageAbstract<FGUnityATTPackage>
    {
        public override string JsonName => "UnityATT.json";
        public override string ModulePath => FGPackageFolders.TRACKING_AUTHORIZATION + "/ATT/UnityATT";
    }
}