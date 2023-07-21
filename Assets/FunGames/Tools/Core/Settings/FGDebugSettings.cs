using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Tools.Debugging
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGDebugSettings : FGModuleSettingsAbstract<FGDebugSettings>
    {
        public const string PATH = FGPath.FUNGAMES + "/Debug/FGDebugSettings";
        static Color DEFAULT_COLOR = Color.white;

        #region Params
        [Space]
        [Header("Debug")]
        public bool logEnabled = true;
        [Space]
        [Header("Analytics")]
        public Color Analytics = DEFAULT_COLOR;
        public Color API= DEFAULT_COLOR;
        public Color GameAnalytics= DEFAULT_COLOR;
        public Color Firebase= DEFAULT_COLOR;
        public Color Flurry= DEFAULT_COLOR;
        [Space]
        [Header("TrackingAuthorization")]
        public Color TrackingAuthorization= DEFAULT_COLOR;
        public Color ATT= DEFAULT_COLOR;
        public Color GDPR= DEFAULT_COLOR;
        [Space]
        [Header("Monetisation")]
        public Color Mediation= DEFAULT_COLOR;
        public Color ApplovinMax= DEFAULT_COLOR;
        public Color Amazon= DEFAULT_COLOR;
        [Space]
        public Color InGameAds= DEFAULT_COLOR;
        public Color Adverty= DEFAULT_COLOR;
        public Color Gadsme= DEFAULT_COLOR;
        public Color Anzu= DEFAULT_COLOR;
        [Space]
        public Color AudioAds= DEFAULT_COLOR;
        public Color AudioMob= DEFAULT_COLOR;
        public Color Odeeo= DEFAULT_COLOR;
        [Space]
        public Color CrossPromo= DEFAULT_COLOR;
        public Color TNCrossPromo= DEFAULT_COLOR;
        [Space]
        public Color IAP= DEFAULT_COLOR;
        public Color UnityIAP= DEFAULT_COLOR;
        [Space]
        [Header("MMP")]
        public Color MMP= DEFAULT_COLOR;
        public Color Adjust= DEFAULT_COLOR;
        [Space]
        [Header("Notifications")]
        public Color Notifications= DEFAULT_COLOR;
        public Color UnityNotifications= DEFAULT_COLOR;
        [Space]
        [Header("RemoteConfig")]
        public Color RemoteConfig= DEFAULT_COLOR;
        public Color TapNationRemoteConfig= DEFAULT_COLOR;

        #endregion

        protected override FGDebugSettings LoadResources()
        {
            return Resources.Load<FGDebugSettings>(PATH);
        }
    }
}