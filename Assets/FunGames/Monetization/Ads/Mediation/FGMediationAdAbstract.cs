using System;
using UnityEngine;

namespace FunGames.Mediation
{
    public abstract class FGMediationAdAbstract<T> : MonoBehaviour, IFGMediationAd where T : FGMediationAdAbstract<T>
    {
        public string AdUnitId => AdId;
        
        protected string AdId = String.Empty;
        
        protected const string EVENT_LOADING_TIME = "AdLoadingTime";
        protected const string EVENT_PARAM_TYPE = "type";
        protected const string EVENT_PARAM_UNIT_ID = "unitId";
        protected const string EVENT_PARAM_NETWORK = "network";
        protected const string EVENT_PARAM_PLACEMENT = "placement";
        protected const string EVENT_PARAM_TIME = "time";
        protected const string EVENT_PARAM_LOAD_ITERATION = "loadIteration";

        public static T Initialize(string adId)
        {
            if (String.IsNullOrEmpty(adId)) return null;
            GameObject obj = new GameObject();
            T instance = obj.AddComponent<T>();
            instance.AdId = adId;
            instance.InitializeCallbacks();
            return instance;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public abstract void InitializeCallbacks();

        public abstract void Load();

        public abstract bool IsReady();
    }
}