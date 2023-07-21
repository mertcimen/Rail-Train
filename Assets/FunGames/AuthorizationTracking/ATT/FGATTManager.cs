using System;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.AuthorizationTracking.ATT
{
    public class FGATTManager : FGModuleParent<FGATTManager, FGATTCallbacks>
    {
        public override string ModuleName => "ATT";
        protected override string RemoteConfigKey => "FGATT";
        public override Color LogColor => FGDebugSettings.settings.TrackingAuthorization;

        [HideInInspector] public bool isATTCompliant = true;

        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            InitWithoutTimer();
            _initialization = delegate { Initialize(); };
            FGUserConsent.ATTPrePopupCallbacks.OnInitialized += _initialization;
        }

        public void ShowATT()
        {
            Callbacks?._show?.Invoke();
        }

        protected override void ClearInitialization()
        {
            FGRemoteConfig.Callbacks.OnInitialized -= _initialization;
        }
    }
}