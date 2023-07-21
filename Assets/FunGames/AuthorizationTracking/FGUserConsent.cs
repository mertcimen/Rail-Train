using FunGames.AuthorizationTracking.ATT;
using FunGames.AuthorizationTracking.ATTPrePopup;
using FunGames.AuthorizationTracking.GDPR;

namespace FunGames.AuthorizationTracking
{
    public class FGUserConsent
    {
        public static FGModuleCallbacks ATTPrePopupCallbacks => FGATTPrePopupManager.Instance.Callbacks;
        public static FGModuleCallbacks ATTCallbacks => FGATTManager.Instance.Callbacks;
        public static FGGdprCallbacks GDPRCallbacks => FGGDPRManager.Instance.Callbacks;

        public static bool IsAttCompliant => FGATTManager.Instance.isATTCompliant;

        public static FGGDPRStatus GdprStatus => FGGDPRManager.Instance.GdprStatus;

        public static bool IsIABCompliant => FGGDPRManager.Instance.IsIABCompliant;

        public static string ConsentString => FGGDPRManager.Instance.TcfV2String;

        public static bool HasFullConsent
        {
            get => IsAttCompliant && GdprStatus.IsFullyAccepted;
        }

        public static Location Location => FGGDPRManager.Instance.location;

        public static void ShowATT() => FGATTManager.Instance.ShowATT();

        public static void ShowGDPR() => FGGDPRManager.Instance.ShowGDPR();
    }
}