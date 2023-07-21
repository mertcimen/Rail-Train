using System.Collections.Generic;
using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Monetization.IAP
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH,
        menuName = PATH, order = ORDER)]
    public class FGIAPSettings : FGModuleSettingsAbstract<FGIAPSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGIAPSettings";

        protected override FGIAPSettings LoadResources()
        {
            return Resources.Load<FGIAPSettings>(PATH);
        }

        public List<FGIAPProduct> Products;
    }
}