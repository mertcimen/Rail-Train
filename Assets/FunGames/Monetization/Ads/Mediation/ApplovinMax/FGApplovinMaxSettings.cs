using System;
using System.Collections.Generic;
using FunGames.Tools.Core;
using UnityEngine;

namespace FunGames.Mediation.ApplovinMax
{
    [CreateAssetMenu(fileName = FGPath.RESOURCES + PATH, menuName = PATH, order = ORDER)]
    public class FGApplovinMaxSettings : FGModuleSettingsAbstract<FGApplovinMaxSettings>
    {
        const string PATH = FGPath.FUNGAMES + "/FGApplovinMaxSettings";

        protected override FGApplovinMaxSettings LoadResources()
        {
            return Resources.Load<FGApplovinMaxSettings>(PATH);
        }

        // [Header("Applovin Max")]

        //[Tooltip("Use Max")]
        //public bool useMax;
        [Tooltip("Max Sdk Key")] public string maxSdkKey =
            "-x3h7mcZ5EdJJCd0iDab_rNf-6t9bsentb_ilJcaZ_ORIGB0P4reTeRrMeRe39-EAu-F6Bqcgah9fv-gSdoO1U";

        [Header("Default iOS Units")] public string iOSInterstitialAdUnitId = String.Empty;
        public string iOSRewardedAdUnitId= String.Empty;
        public string iOSBannerAdUnitId= String.Empty;
        public string iOSAppOpenAdUnitId= String.Empty;

        [Header("Default Android Units")] public string androidInterstitialAdUnitId= String.Empty;
        public string androidRewardedAdUnitId= String.Empty;
        public string androidBannerAdUnitId= String.Empty;
        public string androidAppOpenAdUnitId= String.Empty;

        [Header("")] public List<FGAdUnit> AdditionalUnitIds = new List<FGAdUnit>();

        public string DefaultInterstitialId
        {
            get =>
#if UNITY_IOS
                iOSInterstitialAdUnitId;
#else
                androidInterstitialAdUnitId;
#endif
        }
        
        public string DefaultRewardedId
        {
            get =>
#if UNITY_IOS
                iOSRewardedAdUnitId;
#else
                androidRewardedAdUnitId;
#endif
        }
        
        public string DefaultBannerId
        {
            get =>
#if UNITY_IOS
                iOSBannerAdUnitId;
#else
                androidBannerAdUnitId;
#endif
        }
        
        public string DefaultAppOpenId
        {
            get =>
#if UNITY_IOS
                iOSAppOpenAdUnitId;
#else
                androidAppOpenAdUnitId;
#endif
        }

    }
}