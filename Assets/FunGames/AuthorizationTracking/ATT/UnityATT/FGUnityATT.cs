using System.Collections;
using FunGames.Tools.Debugging;
using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using Unity.Advertisement.IosSupport;
#endif

namespace FunGames.AuthorizationTracking.ATT.UnityATT
{
    public class FGUnityATT : FGATTAbstract<FGUnityATT>
    {
        public override IFGModuleSettings Settings => FGUnityATTSettings.settings;
        public override string ModuleName => "Unity ATT";

        protected override string RemoteConfigKey => "FGUnityATT";
        public override Color LogColor => FGDebugSettings.settings.ATT;

        public override void ShowATT()
        {
#if UNITY_IOS && !UNITY_EDITOR
            RequestATT();
            StartCoroutine(LaunchCallback());
        }

        private void RequestATT()
        {
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Log("Request Tracking");
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
        }

        private IEnumerator LaunchCallback()
        {
            Log("Tracking Callback");
            while (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                   ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Log("Tracking Status : " + ATTrackingStatusBinding.GetAuthorizationTrackingStatus());
                yield return null;
            }

            ATTAnswered();
        }

        private void ATTAnswered()
        {
            switch (ATTrackingStatusBinding.GetAuthorizationTrackingStatus())
            {
                case ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED:
                case ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED:
                case ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED:
                    ATTAnswered(false);
                    break;
                case ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED:
                    ATTAnswered(true);
                    break;
            }
#endif
        }
    }
}