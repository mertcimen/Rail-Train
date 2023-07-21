using System;
using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Tools
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGMainSettings : FGModuleSettingsAbstract<FGMainSettings>
    {
        public const string PATH = FGPath.FUNGAMES + "/FGMainSettings";

        [Tooltip("Name of the game as displayed in the Fungames UIs (ex: Custom GDRP, Custom PrePopup etc.)")]
        public string GameName = String.Empty;
        
        public string ApiKey = String.Empty;
        
        protected override FGMainSettings LoadResources()
        {
            return Resources.Load<FGMainSettings>(PATH);
        }
    }
}