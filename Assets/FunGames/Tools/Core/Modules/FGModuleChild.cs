using System.Collections;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Tools
{
    [DefaultExecutionOrder(-41)]
    public abstract class FGModuleChild<T, N> : FGModuleTemplate<T, N> where T : FGModuleChild<T, N> where N : FGModuleCallbacks, new()
    {
        protected abstract IFGModuleParent Parent { get; }

        public override string ModuleVersion => Settings.Version;

        public abstract IFGModuleSettings Settings { get; }
        protected override void OnAwake()
        {
            Parent?.AddModule(this);
            SetMaxInitTime(3);
            Callbacks.OnInitialized += delegate {  Parent?.ChildModuleInitialized(this); };
        }

        protected override void OnStart()
        {
        }

        public override void Clear()
        {
            base.Clear();
            Parent?.RemoveModule(this);
        }
    }
}