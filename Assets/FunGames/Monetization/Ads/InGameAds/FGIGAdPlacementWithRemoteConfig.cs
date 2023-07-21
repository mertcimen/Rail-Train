using FunGames.RemoteConfig;
using UnityEngine;

namespace FunGames.Monetization.InGameAds
{
    public class FGIGAdPlacementWithRemoteConfig : MonoBehaviour
    {
        [Tooltip("Ad Placement for Adverty (can be null)")]
        public GameObject advertyPlacement;
        [Tooltip("Ad Placement for Anzu (can be null)")]
        public GameObject anzuPlacement;
        [Tooltip("Ad Placement for Gadsme (can be null)")]
        public GameObject gadsmePlacement;

        private void Awake()
        {
            if (FGRemoteConfigManager.Instance.IsInitialized())
            {
                DiplayPlacementAccordingToRemoteConf(true);
            }
            else
            {
                FGRemoteConfig.Callbacks.OnInitialized += DiplayPlacementAccordingToRemoteConf;
            }
        }

        private void DiplayPlacementAccordingToRemoteConf(bool obj)
        {
            int remoteValue = FGRemoteConfig.GetIntValue(FGIGAdsManager.RC_INGAMEADS);

            switch (remoteValue)
            {
                case 0:
                    FGIGAdsManager.Instance.Log("Remote InGameAds Config set to 0 : no ads displayed");
                    Deactivate(advertyPlacement);
                    Deactivate(anzuPlacement);
                    Deactivate(gadsmePlacement);
                    break;
                case 1:
                    FGIGAdsManager.Instance.Log("Remote InGameAds Config set to 1 : Adverty ad displayed");
                    Activate(advertyPlacement);
                    Deactivate(anzuPlacement);
                    Deactivate(gadsmePlacement);
                    break;
                case 2:
                    FGIGAdsManager.Instance.Log("Remote InGameAds Config set to 2 : Anzu ad displayed");
                    Deactivate(advertyPlacement);
                    Activate(anzuPlacement);
                    Deactivate(gadsmePlacement);
                    break;
                case 3:
                    FGIGAdsManager.Instance.Log("Remote InGameAds Config set to 3 : Gadsme ad displayed");
                    Deactivate(advertyPlacement);
                    Deactivate(anzuPlacement);
                    Activate(gadsmePlacement);
                    break;
            }
        }

        private void Deactivate(GameObject gameObject)
        {
            if(gameObject!=null) gameObject.SetActive(false);
        }
        
        private void Activate(GameObject gameObject)
        {
            if(gameObject!=null) gameObject.SetActive(true);
        }
    }
}