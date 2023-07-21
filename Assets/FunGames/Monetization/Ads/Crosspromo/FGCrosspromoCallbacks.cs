using System;

namespace FunGames.Monetization.Crosspromo
{
    public class FGCrosspromoCallbacks : FGModuleCallbacks
    {
        internal Action _playCrosspromo;
        internal Action _closeCrosspromo;
        internal Action<bool> _crosspromoCompleted;

        public event Action OnPlay
        {
            add => _playCrosspromo += value;
            remove => _playCrosspromo -= value;
        }

        public event Action OnClose
        {
            add => _closeCrosspromo += value;
            remove => _closeCrosspromo -= value;
        }

        public event Action<bool> OnCompleted
        {
            add => _crosspromoCompleted += value;
            remove => _crosspromoCompleted -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _playCrosspromo = null;
            _closeCrosspromo = null;
            _crosspromoCompleted = null;
        }
    }
}