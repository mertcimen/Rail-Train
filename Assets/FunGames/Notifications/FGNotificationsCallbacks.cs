using System;

namespace FunGames.Notifications
{
    public class FGNotificationsCallbacks : FGModuleCallbacks
    {
        internal Action _RemoveAllNotifs;
        internal Action<int> _RemoveNotifById;
        internal Action _GetNotifUserOpenAppWith;

        public event Action RemoveAllNotifs
        {
            add => _RemoveAllNotifs += value;
            remove => _RemoveAllNotifs -= value;
        }

        public event Action<int> RemoveNotifById
        {
            add => _RemoveNotifById = value;
            remove => _RemoveNotifById -= value;
        }

        public event Action GetNotifUserOpenAppWith
        {
            add => _GetNotifUserOpenAppWith += value;
            remove => _GetNotifUserOpenAppWith -= value;
        }


#if UNITY_ANDROID
        internal Action<string, string, string, int, int, int, int> _TimeIntervalNotifs;

        public event Action<string, string, string, int, int, int, int> TimeIntervalNotifs
        {
            add => _TimeIntervalNotifs = value;
            remove => _TimeIntervalNotifs -= value;
        }

#elif UNITY_IOS
        internal Action<int, int, int, string, string, string, string, string, string> _TimeIntervalNotifs;
        public event Action<int, int, int, string, string, string, string, string, string> TimeIntervalNotifs
        {
            add => _TimeIntervalNotifs = value;
            remove => _TimeIntervalNotifs -= value;
        }

#else
    internal Action _TimeIntervalNotifs;
    public event Action TimeIntervalNotifs
    {
        add => _TimeIntervalNotifs = value;
        remove => _TimeIntervalNotifs -= value;
    }
#endif

        public override void Clear()
        {
            base.Clear();
            _RemoveAllNotifs = null;
            _RemoveNotifById = null;
            _GetNotifUserOpenAppWith = null;
            _TimeIntervalNotifs = null;
        }
    }
}