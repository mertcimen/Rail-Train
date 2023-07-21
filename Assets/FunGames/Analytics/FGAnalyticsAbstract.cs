using System;
using System.Collections.Generic;
using FunGames.Tools;

namespace FunGames.Analytics
{
    public abstract class FGAnalyticsAbstract<T> : FGModuleChild<T, FGAnalyticsCallbacks>
        where T : FGModuleChild<T, FGAnalyticsCallbacks>
    {
        protected override IFGModuleParent Parent => FGAnalyticsManager.Instance;

        private List<Action> _poolEvents = new List<Action>();

        protected abstract void Init();
        
        protected override void InitializeCallbacks()
        {
            FGAnalyticsManager.Instance.Callbacks.Initialization += Initialize;
            FGAnalyticsManager.Instance.Callbacks.OnSendProgressionEvent += SendProgressionEvent;
            FGAnalyticsManager.Instance.Callbacks.OnSendDesignEventSimple += SendDesignEventSimple;
            FGAnalyticsManager.Instance.Callbacks.OnSendDesignEventDictio += SendDesignEventDictio;
            FGAnalyticsManager.Instance.Callbacks.OnSendAdEvent += SendAdEvent;
            Callbacks.OnInitialized += SendAllPoolEvents;
        }

        protected override void InitializeModule()
        {
            Init();
        }

        protected abstract void ProgressionEvent(LevelStatus statusFG, string level, string subLevel = "",
            int score = -1);

        protected abstract void DesignEventSimple(string eventId, float eventValue);

        protected abstract void DesignEventDictio(string eventId, Dictionary<string, object> customFields);

        protected abstract void AdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement);

        public void SendProgressionEvent(LevelStatus statusFG, string level, string subLevel = "", int score = -1)
        {
            string eventContent = "{ " + statusFG + " ; " + level + " ; " + subLevel + " ; " + score + "}";

            if (!IsInitialized())
            {
                _poolEvents.Add(delegate { SendProgressionEvent(statusFG, level, subLevel, score); });
                // LogWarning("Progression Event added to pool : module has not been initialized !\n" + eventContent);
                return;
            }

            Log("Progression Event triggered : " + eventContent);
            ProgressionEvent(statusFG, level, subLevel, score);
            Callbacks._ProgressionEvent?.Invoke(statusFG, level, subLevel, score);
        }

        public void SendDesignEventSimple(string eventId, float eventValue)
        {
            string eventContent = "{" + eventId + " ; " + eventValue + "}";

            if (!IsInitialized())
            {
                _poolEvents.Add(delegate { SendDesignEventSimple(eventId, eventValue); });
                // LogWarning("Simple Design Event added to pool : module has not been initialized !\n" + eventContent);
                return;
            }

            Log("Design Event triggered : " + eventContent);
            DesignEventSimple(eventId, eventValue);
            Callbacks._DesignEventSimple?.Invoke(eventId, eventValue);
        }

        public void SendDesignEventDictio(string eventId, Dictionary<string, object> customFields, float eventValue)
        {
            string eventContent = "{" + eventId + " ; " + customFields + " ; " + eventValue + "}";

            if (!IsInitialized())
            {
                _poolEvents.Add(delegate { SendDesignEventDictio(eventId, customFields, eventValue); });
                // LogWarning("Dictio Design Event added to pool : module has not been initialized !\n" + eventContent);
                return;
            }

            Log("Design Event triggered : " + eventContent);
            DesignEventDictio(eventId, customFields);
            Callbacks._DesignEventDictio?.Invoke(eventId, customFields, eventValue);
        }

        public void SendAdEvent(AdAction adAction, AdType adType, string adSdkName, string adPlacement)
        {
            string eventContent = "{" + adAction + " ; " + adType + " ; " + adSdkName + " ; " + adPlacement + "}";

            if (!IsInitialized())
            {
                _poolEvents.Add(delegate { SendAdEvent(adAction, adType, adSdkName, adPlacement); });
                // LogWarning("Ad Event added to pool : module has not been initialized !\n" + eventContent);
                return;
            }

            Log("Ad Event triggered : " + eventContent);
            AdEvent(adAction, adType, adSdkName, adPlacement);
            Callbacks._AdEvent?.Invoke(adAction, adType, adSdkName, adPlacement);
        }

        protected void EventReceived(string eventName, bool eventReceived)
        {
            Callbacks?._EventReceived?.Invoke(eventName, eventReceived);
            // Callbacks._EventReceived = null;
        }

        private void SendAllPoolEvents(bool moduleInitialized)
        {
            if (!moduleInitialized) return;

            foreach (var progressionEvent in _poolEvents)
            {
                progressionEvent?.Invoke();
            }

            _poolEvents.Clear();
        }

        protected override void ClearInitialization()
        {
            FGAnalyticsManager.Instance.Callbacks.Initialization -= Initialize;
            FGAnalyticsManager.Instance.Callbacks.OnSendProgressionEvent -= SendProgressionEvent;
            FGAnalyticsManager.Instance.Callbacks.OnSendDesignEventSimple -= SendDesignEventSimple;
            FGAnalyticsManager.Instance.Callbacks.OnSendDesignEventDictio -= SendDesignEventDictio;
            FGAnalyticsManager.Instance.Callbacks.OnSendAdEvent -= SendAdEvent;
            Callbacks.OnInitialized -= SendAllPoolEvents;
        }
    }
}