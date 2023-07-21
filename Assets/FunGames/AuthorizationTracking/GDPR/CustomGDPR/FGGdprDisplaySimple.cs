using UnityEngine.UI;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR
{
    public class FGGdprDisplaySimple : FGGDPRDisplayAbstract
    {
        public Button confirmChoicesButton;
        public Toggle acceptTargetedAdvertising;
        public Toggle acceptAnalytics;
        public Toggle isAboveAgeLimit;

        protected override void OnAwake()
        {
            confirmChoicesButton.onClick.AddListener(ValidateGDPR);

            acceptTargetedAdvertising.onValueChanged.AddListener(TargetAdvertisingChanged);
            acceptAnalytics.onValueChanged.AddListener(AnalyticsChanged);
            isAboveAgeLimit.onValueChanged.AddListener(AgeLimitChanged);

            HandleFlexibility();
            acceptTargetedAdvertising.isOn = GdprFlexibility.Flexible.Equals(Flexibility);;
            acceptAnalytics.isOn = GdprFlexibility.Flexible.Equals(Flexibility);;
            isAboveAgeLimit.isOn = GdprFlexibility.Flexible.Equals(Flexibility);;
        }

        private void TargetAdvertisingChanged(bool arg0)
        {
            status.TargetedAdvertisingAccepted = arg0;
            HandleFlexibility();
        }

        private void AnalyticsChanged(bool arg0)
        {
            status.AnalyticsAccepted = arg0;
            HandleFlexibility();
        }

        private void AgeLimitChanged(bool arg0)
        {
            status.IsAboveAgeLimit = arg0;
            HandleFlexibility();
        }

        protected void HandleFlexibility()
        {
            switch (Flexibility)
            {
                case GdprFlexibility.ForceAccepting:
                    bool isFullyAccepted =
                        acceptTargetedAdvertising.isOn && acceptAnalytics.isOn && isAboveAgeLimit.isOn;
                    confirmChoicesButton.interactable = isFullyAccepted;
                    break;
                case GdprFlexibility.Flexible:
                    confirmChoicesButton.interactable = true;
                    break;
            }
        }
    }
}