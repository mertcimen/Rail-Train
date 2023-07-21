using System;
using FunGames.RemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Debugging;
using FunGames.Tools.Utils;
using UnityEngine;

namespace FunGames.AuthorizationTracking.GDPR
{
    public abstract class FGGdprAbstract<T> : FGModuleChild<T, FGGdprCallbacks>
        where T : FGModuleChild<T, FGGdprCallbacks>
    {
        public override Color LogColor => FGDebugSettings.settings.GDPR;
        protected abstract int RemoteConfig {get;}
        protected abstract void ShowGDPR();

        protected override IFGModuleParent Parent => FGGDPRManager.Instance;

        protected override void InitializeCallbacks()
        {
            InitWithoutTimer();
            FGGDPRManager.Instance.Callbacks.Initialization += Initialize;
            FGGDPRManager.Instance.Callbacks.Show += Show;
        }

        protected override void InitializeModule()
        {
            int gdprConfig = FGRemoteConfig.GetIntValue(FGGDPRManager.RC_GDPR_TYPE);
            if (!RemoteConfig.Equals(gdprConfig))
            {
                LogWarning("GDPPR config is set to " + gdprConfig + " = No " + ModuleName);
                ClearInitialization();
                InitializationComplete(true);
                return;
            }
            
            if(!FGUserConsent.IsAttCompliant)
            {
                Log("ATT refused : GDPR refused automatically");
                ClearInitialization();
                InitializationComplete(false);
                return;
            }

            Log("Fetching localization...");
            LocalisationUtils.GetLocalisationCode(InitWithLocalisation);
        }

        private void InitWithLocalisation(Location location)
        {
            if (String.IsNullOrEmpty(location.countryCode)) Log("Couldn't retrieve localisation code !");
            else Log("Localisation found : {" + location.countryCode + " ; " + location.regionCode + "}");

            FGGDPRManager.Instance.location = location;

            if (FGGDPRManager.Instance.IsGDPRAlreadyAnswered &&
                FGGDPRManager.Instance.GdprStatus.IsFullyAccepted)
            {
                Log("GDPR already responded - Accepted ");
                GDPRAnswered();
            }
            else if (LocalisationUtils.isGDPRApplied(location))
            {
                Show();
            }
            else
            {
                Log("No need for GDPR");
                GDPRAnswered();
            }
        }

        public void Show()
        {
            ShowGDPR();
            Callbacks?._show?.Invoke();
        }

        protected void GDPRAnswered()
        {
            // Callbacks?._validate?.Invoke(FGUserConsent.GdprStatus);
            // FGGDPRManager.Instance.Callbacks?._validate?.Invoke(FGUserConsent.GdprStatus);
            InitializationComplete(true);
        }

        protected override void ClearInitialization()
        {
            FGGDPRManager.Instance.Callbacks.Initialization -= Initialize;
            FGGDPRManager.Instance.Callbacks.Show -= Show;
        }
    }
}