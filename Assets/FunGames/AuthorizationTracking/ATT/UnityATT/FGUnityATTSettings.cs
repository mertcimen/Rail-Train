using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.AuthorizationTracking.ATT.UnityATT
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGUnityATTSettings : FGModuleSettingsAbstract<FGUnityATTSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGUnityATTSettings";

        protected override FGUnityATTSettings LoadResources()
        {
            return Resources.Load<FGUnityATTSettings>(PATH);
        }
    }
}