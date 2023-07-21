using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunGames.Tools
{
    [DefaultExecutionOrder(-42)]
    public abstract class FGModuleParent<T, N> : FGModuleTemplate<T, N>, IFGModuleParent
        where T : FGModuleParent<T, N> where N : FGModuleCallbacks, new()
    {
        private float _childrenToInitialize = 0;
        private bool _initializationSucceeded = true;

        public float ChildrenToInitialize => _childrenToInitialize;

        public override string ModuleVersion => FGMain.Instance.ModuleVersion;

        private List<IFGModule> _childModules = new List<IFGModule>();

        private bool _noImplementation = false;
        // private IEnumerator _initCoroutine;

        protected override void OnAwake()
        {
            FGMain.Instance.AddModule(this);
            Callbacks.OnInitialized += delegate { FGMain.Instance.ChildModuleInitialized(this); };
        }

        protected override void OnStart()
        {
            if (_childModules.Count == 0)
            {
                _noImplementation = true;
            }
        }

        protected override void InitializeModule()
        {
            if (!_noImplementation) return;
            
            Log("No implementation.");
            InitializationComplete(true);
        }

        public void AddModule(IFGModule childModule)
        {
            _childrenToInitialize++;
            _childModules.Add(childModule);
        }
        
        public void RemoveModule(IFGModule childModule)
        {
            if (!_childModules.Contains(childModule)) return;
            _childModules.Remove(childModule);
            ChildModuleInitialized(childModule);
        }
        public void ChildModuleInitialized(IFGModule childModule)
        {
            _childrenToInitialize--;
            // _childModules.Remove(childModule);
            _initializationSucceeded &= childModule.IsInitialized() || !childModule.MustBeInitialized();
            if (_childrenToInitialize == 0)
            {
                // StopCoroutine(_initCoroutine);
                InitializationComplete(_initializationSucceeded);
            }
        }

        // private void StartCheckInit()
        // {
        //     _initCoroutine = CheckInitialization();
        //     StartCoroutine(_initCoroutine);
        // }

        // private IEnumerator CheckInitialization()
        // {
        //     if (_childModules.Count == 0)
        //     {
        //         Log("No implementation.");
        //         InitializationComplete(true);
        //         yield break;
        //     }

        // float counter = 0;
        // float iteration = 0.2f;
        // float maxWaitingDelay = 10;
        // while (counter <= maxWaitingDelay)
        // {
        //     yield return new WaitForSeconds(iteration);
        //     counter += iteration;
        // }
        //
        // InitializationComplete(IsInitialized());
        // }

        protected void EndOfInitializationStatus(bool success)
        {
            if (success) return;

            foreach (var module in _childModules)
            {
                if (module.IsInitialized()) continue;

                LogError(module.ModuleName + " module initialization failed !");
            }
        }
        
        public override void Clear()
        {
            base.Clear();
            FGMain.Instance.RemoveModule(this);
        }
    }
}