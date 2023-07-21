using FunGames.Tools.Debugging.Editor;

namespace FunGames.Analytics.API
{
    public class FGAPIPackage : FGPackageAbstract<FGAPIPackage>
    {
        public override string JsonName => "API.json";
        public override string ModulePath => FGPackageFolders.ANALYTICS + "/API";
    }
}