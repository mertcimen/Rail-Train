using com.adjust.sdk;
using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.MMP.AdjustMMP
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGAdjustSettings : FGModuleSettingsAbstract<FGAdjustSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGAdjustSettings";

        protected override FGAdjustSettings LoadResources()
        {
            return Resources.Load<FGAdjustSettings>(PATH);
        }

        [Tooltip("Adjust App Token")] public string AppToken;

        [Tooltip("Adjust Log Level")] public AdjustLogLevel logLevel = AdjustLogLevel.Info;

        // [Tooltip("Adjust Environment")] public AdjustEnvironment environment = AdjustEnvironment.Sandbox;
        public bool sendInBackground = true;

        public string AdjustEventInterID;
        public string AdjustEventRewardedID;

        [Header("Retention Event")] public string AdjustTokenRet1;
        public string AdjustTokenRet3;
        public string AdjustTokenRet5;
        public string AdjustTokenRet7;

        [Header("Rewarded event")] public int nbRV1 = 5;
        public string nbRV1Token;

        [Space] public int nbRV2 = 10;
        public string nbRV2Token;

        [Space] public int nbRV3 = 15;
        public string nbRV3Token;

        [Space] public int nbRV4 = 20;
        public string nbRV4Token;

        [Space] public int nbRV5 = 25;
        public string nbRV5Token;
    }
}