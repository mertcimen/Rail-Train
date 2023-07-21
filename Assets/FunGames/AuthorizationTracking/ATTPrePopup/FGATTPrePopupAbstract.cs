using FunGames.Tools;
using FunGames.Tools.Debugging;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;

namespace FunGames.AuthorizationTracking.ATTPrePopup
{
    public abstract class FGATTPrePopupAbstract<T> : FGModuleChild<T, FGModuleCallbacks>
        where T : FGModuleChild<T, FGModuleCallbacks>
    {
        protected override IFGModuleParent Parent => FGATTPrePopupManager.Instance;
        public override Color LogColor => FGDebugSettings.settings.ATT;

        protected abstract void Show();

        protected override void InitializeCallbacks()
        {
            InitWithoutTimer();
            FGATTPrePopupManager.Instance.Callbacks.Initialization += Initialize;
        }

        protected override void InitializeModule()
        {
#if UNITY_IOS
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Show();
            }
            else
            {
                Log("ATT already responded - No need for ATT PrePopup");
                PrePopupAccepted();
            }
#else
            Log("Current platform : Android - No need for ATT PrePopup");
            PrePopupAccepted();
#endif
        }

        protected void PrePopupAccepted()
        {
            InitializationComplete(true);
        }

        protected override void ClearInitialization()
        {
            FGATTPrePopupManager.Instance.Callbacks.Initialization -= Initialize;
        }
    }
}