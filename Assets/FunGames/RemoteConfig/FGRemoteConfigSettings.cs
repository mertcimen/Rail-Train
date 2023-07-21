using System.Collections.Generic;
using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.RemoteConfig
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH,
        menuName = PATH, order = ORDER)]
    public class FGRemoteConfigSettings : FGModuleSettingsAbstract<FGRemoteConfigSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGRemoteConfigSettings";

        private List<FGRemoteConfigValue> _defaultValues = null;

        protected override FGRemoteConfigSettings LoadResources()
        {
            return Resources.Load<FGRemoteConfigSettings>(PATH);
        }

        public List<FGRemoteConfigValue> CustomDefaultValues = new List<FGRemoteConfigValue>();
    }
}