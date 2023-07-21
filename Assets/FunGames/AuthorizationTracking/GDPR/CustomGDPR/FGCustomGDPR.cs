using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR
{
    public class FGCustomGDPR : FGCustomGDPRAbstract<FGCustomGDPR>
    {
        // public GameObject firstFrame;
        // public GameObject secondFrame;
        // // public Button PlayButton;
        // public Button AgreeButton;
        // public Button SettingsButton;
        // private int activeButton = 0;
        //
        public override IFGModuleSettings Settings => FGCustomGDPRSettings.settings;
        public override string ModuleName => "TN CUSTOM GDPR";
        protected override string RemoteConfigKey => "FGCustomGDPR";
        public override Color LogColor => FGDebugSettings.settings.GDPR;
        protected override int RemoteConfig => 0;

        // protected override int ConfigValue => 0;
        // protected override void OnAwake()
        // {
        //     base.OnAwake();
        //     // PlayButton.onClick.AddListener(ValidateGDPR);
        //     AgreeButton.onClick.AddListener(() =>
        //     {
        //         status = FGGDPRStatus.FullyAccepted;
        //         ValidateGDPR();
        //     });
        //     SettingsButton.onClick.AddListener(OnClickSettings);
        //     firstFrame.SetActive(true);
        //     secondFrame.SetActive(false);
        //     
        // }
        //
        // void OnClickSettings()
        // {
        //     firstFrame.SetActive(false);
        //     secondFrame.SetActive(true);
        // }
    }
}