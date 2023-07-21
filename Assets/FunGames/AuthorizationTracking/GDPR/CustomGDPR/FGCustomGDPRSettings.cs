using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGCustomGDPRSettings : FGModuleSettingsAbstract<FGCustomGDPRSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGCustomGDPRSettings";

        protected override FGCustomGDPRSettings LoadResources()
        {
            return Resources.Load<FGCustomGDPRSettings>(PATH);
        }
    }
}