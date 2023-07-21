using System;
using UnityEngine;

namespace FunGames.AuthorizationTracking.GDPR.CustomGDPR
{
    public abstract class FGGDPRDisplayAbstract: MonoBehaviour
    {
        public int RemoteConfigValue = 0;

        public GdprFlexibility Flexibility = GdprFlexibility.ForceAccepting;

        private Action<FGGDPRStatus> _validate;

        public event Action<FGGDPRStatus> OnValidated
        {
            add => _validate += value;
            remove => _validate -= value;
        }

        [HideInInspector] protected FGGDPRStatus status;

        private void Awake()
        {
            gameObject.SetActive(false);
            status = FGUserConsent.GdprStatus;
            OnAwake();
        }

        protected abstract void OnAwake();

        public void ShowGDPR()
        {
            gameObject.SetActive(true);
        }

        public void ValidateGDPR()
        {
            gameObject.SetActive(false);
            _validate?.Invoke(status);
        }
    }

    public enum GdprFlexibility
    {
        ForceAccepting,
        Flexible
    }
}