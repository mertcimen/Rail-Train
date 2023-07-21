using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Analytics.API
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGAPISettings : FGModuleSettingsAbstract<FGAPISettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGAPISettings";

        protected override FGAPISettings LoadResources()
        {
            return Resources.Load<FGAPISettings>(PATH);
        }
    }
}