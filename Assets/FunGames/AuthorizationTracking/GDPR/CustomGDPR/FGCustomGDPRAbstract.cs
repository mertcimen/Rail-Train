using System.Collections.Generic;
using FunGames.RemoteConfig;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR
{
    public abstract class FGCustomGDPRAbstract<T> : FGGdprAbstract<T> where T : FGGdprAbstract<T>
    {
        public List<FGGDPRDisplayAbstract> gdprWindows = new List<FGGDPRDisplayAbstract>();
        private FGGDPRDisplayAbstract _currentGdpr;

        protected override void ShowGDPR()
        {
            int gdprDisplay = FGRemoteConfig.GetIntValue(FGGDPRManager.RC_GDPR_DISPLAY);
            foreach (var gdpr in gdprWindows)
            {
                if(gdpr.RemoteConfigValue.Equals(gdprDisplay))
                {
                    _currentGdpr = Instantiate(gdpr, transform) ;
                    break;
                }
            }
            _currentGdpr.OnValidated += (FGGDPRStatus status) =>
            {
                FGUserConsent.GdprStatus.AnalyticsAccepted = status.AnalyticsAccepted;
                FGUserConsent.GdprStatus.TargetedAdvertisingAccepted = status.TargetedAdvertisingAccepted;
                FGUserConsent.GdprStatus.IsAboveAgeLimit = status.IsAboveAgeLimit;
                GDPRAnswered();
            };
            _currentGdpr.ShowGDPR();
        }
    }
}