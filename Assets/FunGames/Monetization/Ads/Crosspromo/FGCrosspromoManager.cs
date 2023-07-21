using System;
using FunGames.AuthorizationTracking;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Monetization.Crosspromo
{
    public class FGCrosspromoManager : FGModuleParent<FGCrosspromoManager, FGCrosspromoCallbacks>
    {
        public override string ModuleName => "Crosspromo";
        protected override string RemoteConfigKey => "FGCrosspromo";
        public override Color LogColor => FGDebugSettings.settings.CrossPromo;
        
        private Action<bool> _initialization;

        protected override void InitializeCallbacks()
        {
            _initialization = delegate { Initialize(); };
            FGUserConsent.GDPRCallbacks.OnInitialized += _initialization;
        }

        public void Play()
        {
            Callbacks._playCrosspromo?.Invoke();
        }
        
        public void Close()
        {
            Callbacks._closeCrosspromo?.Invoke();
        }

        public void Completed(bool success)
        {
            Callbacks._crosspromoCompleted?.Invoke(success);
        }

        protected override void ClearInitialization()
        {
            FGUserConsent.GDPRCallbacks.OnInitialized -= _initialization;
        }
    }
}