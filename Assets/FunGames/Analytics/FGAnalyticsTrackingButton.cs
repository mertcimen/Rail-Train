using FunGames.AuthorizationTracking;
using UnityEngine;
using UnityEngine.UI;

namespace FunGames.Analytics
{
    public class FGAnalyticsTrackingButton : MonoBehaviour
    {
        public Toggle enableAnalyticsToggle;

        private void Awake()
        {
            enableAnalyticsToggle.isOn = FGUserConsent.GdprStatus.AnalyticsAccepted;
            enableAnalyticsToggle.onValueChanged.AddListener(UpdateAnalyticsConsent);
        }

        private void UpdateAnalyticsConsent(bool value)
        {
            Debug.Log("Analytics consent changed to : " + value);
            FGUserConsent.GdprStatus.AnalyticsAccepted = false;
        }
    }
}