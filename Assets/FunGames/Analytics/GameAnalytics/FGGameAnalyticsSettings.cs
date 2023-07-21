using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Analytics.GA
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH,
        menuName = PATH, order = ORDER)]
    public class FGGameAnalyticsSettings : FGModuleSettingsAbstract<FGGameAnalyticsSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGGameAnalyticsSettings";

        protected override FGGameAnalyticsSettings LoadResources()
        {
            return Resources.Load<FGGameAnalyticsSettings>(PATH);
        }

        [Header("GameAnalytics")] [Tooltip("GameAnalytics Ios Game Key")]
        public string gameAnalyticsIosGameKey;

        [Tooltip("GameAnalytics Ios Secret Key")]
        public string gameAnalyticsIosSecretKey;

        [Tooltip("GameAnalytics Android Game Key")]
        public string gameAnalyticsAndroidGameKey;

        [Tooltip("GameAnalytics Android Secret Key")]
        public string gameAnalyticsAndroidSecretKey;
    }
}