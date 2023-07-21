using System;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.MMP
{
    public class FGMMPManager : FGModuleParent<FGMMPManager, FGModuleCallbacks>
    {
        public override string ModuleName => "MMP";
        protected override string RemoteConfigKey => "FGMMP";
        public override Color LogColor => FGDebugSettings.settings.MMP;

        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}