using System;
namespace FunGames.AuthorizationTracking.ATT
{
    public class FGATTCallbacks : FGModuleCallbacks
    {
        internal Action _show;

        public event Action Show
        {
            add => _show += value;
            remove => _show -= value;
        }
    }
}