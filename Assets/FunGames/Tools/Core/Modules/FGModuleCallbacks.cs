using System;

namespace FunGames
{
    public class FGModuleCallbacks
    {
        internal Action _onInitialization;
        internal Action<bool> _onInitialized;
        
        public event Action Initialization
        {
            add => _onInitialization += value;
            remove => _onInitialization -= value;
        }

        public event Action<bool> OnInitialized
        {
            add => _onInitialized += value;
            remove => _onInitialized -= value;
        }

        public virtual void Clear()
        {
            _onInitialization = null;
            _onInitialized = null;
        }
    }
}