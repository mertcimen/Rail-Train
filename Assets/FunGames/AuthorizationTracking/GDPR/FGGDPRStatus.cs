namespace FunGames.AuthorizationTracking.GDPR
{
    public class FGGDPRStatus
    {
        public bool TargetedAdvertisingAccepted = true;
        public bool AnalyticsAccepted = true;
        public bool IsAboveAgeLimit = true;

        public void SetGDPRValues(bool result)
        {
            TargetedAdvertisingAccepted = result;
            IsAboveAgeLimit = result;
        }

        public bool IsFullyAccepted
        {
            get => TargetedAdvertisingAccepted && IsAboveAgeLimit;
        }

        public static FGGDPRStatus FullyAccepted
        {
            get
            {
                FGGDPRStatus status = new FGGDPRStatus();
                status.SetGDPRValues(true);
                return status;
            }
        }
    }
}