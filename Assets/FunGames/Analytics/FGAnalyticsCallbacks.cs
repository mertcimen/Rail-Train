using System;
using System.Collections.Generic;

namespace FunGames.Analytics
{
    public class FGAnalyticsCallbacks : FGModuleCallbacks
    {
        internal Action<LevelStatus, string, string, int> _ProgressionEvent;
        internal Action<string, float> _DesignEventSimple;
        internal Action<string, Dictionary<string, object>, float> _DesignEventDictio;
        internal Action<AdAction, AdType, string, string> _AdEvent;
        internal Action<string, bool> _EventReceived;
        
        public event Action<LevelStatus, string, string, int> OnSendProgressionEvent
        {
            add => _ProgressionEvent += value;
            remove => _ProgressionEvent -= value;
        }


        public event Action<string, float> OnSendDesignEventSimple
        {
            add => _DesignEventSimple += value;
            remove => _DesignEventSimple -= value;
        }
        
        public event Action<string, Dictionary<string, object>, float> OnSendDesignEventDictio
        {
            add => _DesignEventDictio += value;
            remove => _DesignEventDictio -= value;
        }
        
        public event Action<AdAction, AdType, string, string> OnSendAdEvent
        {
            add => _AdEvent += value;
            remove => _AdEvent -= value;
        }
        
        public event Action<string,bool> OnEventReceived
        {
            add => _EventReceived += value;
            remove => _EventReceived -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _ProgressionEvent = null;
            _DesignEventSimple = null;
            _DesignEventDictio = null;
            _AdEvent = null;
        }
    }
}