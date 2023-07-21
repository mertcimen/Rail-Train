using FunGames.Tools.Debugging.Editor;

namespace FunGames.RemoteConfig
{
    public class FGRemoteConfigPackage : FGPackageAbstract<FGRemoteConfigPackage>
    {
        public override string JsonName => "";
        public override string ModulePath => FGPackageFolders.REMOTE_CONFIG;
    }
}