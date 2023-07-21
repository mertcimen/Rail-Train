using System;
using FunGames.Tools.Debugging.Editor;

namespace FunGames.Mediation
{
    public class FGMediationPackage : FGPackageAbstract<FGMediationPackage>
    {
        public override string JsonName => String.Empty;
        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.ADS + "/" +
                                             FGPackageFolders.MEDIATION;
    }
}