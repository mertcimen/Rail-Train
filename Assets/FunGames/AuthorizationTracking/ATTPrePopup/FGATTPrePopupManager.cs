using System;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.AuthorizationTracking.ATTPrePopup
{
    public class FGATTPrePopupManager : FGModuleParent<FGATTPrePopupManager,FGModuleCallbacks>
    {
        public override string ModuleName => "ATTPrePopup";
        protected override string RemoteConfigKey => "FGATTPrePopup";
        public override Color LogColor => FGDebugSettings.settings.TrackingAuthorization;
        
        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            InitWithoutTimer();
            _initialization = delegate { Initialize(); };
            FGRemoteConfig.Callbacks.OnInitialized += _initialization;
        }

        protected override void ClearInitialization()
        {
            FGRemoteConfig.Callbacks.OnInitialized -= _initialization;
        }
    }
}