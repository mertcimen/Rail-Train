using System;
namespace FunGames.AuthorizationTracking.GDPR
{
    public class FGGdprCallbacks : FGModuleCallbacks
    {
        internal Action _show;
        // internal Action<FGGDPRStatus> _validate;

        public event Action Show
        {
            add => _show += value;
            remove => _show -= value;
        }

        //
        // public event Action<FGGDPRStatus> OnValidated
        // {
        //     add => _validate += value;
        //     remove => _validate -= value;
        // }

        public override void Clear()
        {
            base.Clear();
            _show = null;
            // _validate = null;
        }
    }
}