using System.Collections;
using System.Collections.Generic;
using FunGames.Analytics;
using FunGames.RemoteConfig;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.Tools
{
    public abstract class FGModuleTemplate<T, N> : FGSingleton<T>, IFGModule
        where T : MonoBehaviour where N : FGModuleCallbacks, new()
    {
        public abstract string ModuleVersion { get; }
        public abstract string ModuleName { get; }
        protected abstract string RemoteConfigKey { get; }

        public N Callbacks
        {
            get => _callbacks;
        }

        public abstract Color LogColor { get; }

        public float TimeForInitialization => _totalInitTime;

        private bool _isInitialized = false;

        private float _timer = 0;

        private float _totalInitTime = 0;

        private N _callbacks = new N();

        private IEnumerator _checkInitCoroutine;
        private bool _useCheckInit = true;
        private float _maxInitTime = 5;

        public const string EVENT_INITIALISATION_START = "InitialisationStart";
        public const string EVENT_INITIALISATION_COMPLETE = "InitialisationComplete";

        private const string EVENT_PARAM_MODULE = "module";
        private const string EVENT_PARAM_VERSION = "version";
        private const string EVENT_PARAM_SUCCESS = "success";
        private const string EVENT_PARAM_INIT_TIME = "initTime";

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        public virtual bool MustBeInitialized()
        {
            return FGRemoteConfig.GetBooleanValue(RemoteConfigKey);
        }
        // protected abstract N InitModuleCallback();

        /// <summary>
        /// InitializeCallbacks is the first action performed on Awake
        /// </summary>
        protected abstract void InitializeCallbacks();

        protected abstract void OnAwake();

        protected abstract void OnStart();

        protected abstract void InitializeModule();
        
        protected abstract void ClearInitialization();

        private void Awake()
        {
            var instances = FindObjectsOfType(typeof(T));
            if (instances.Length > 1)
            {
                Log("Module has already been instantiated.");
                Destroy(gameObject);
                return;
            }

            // _callbacks = new N();
            DontDestroyOnLoad(this);
            InitializeCallbacks();
            FGRemoteConfig.AddDefaultValue(RemoteConfigKey, 1);
            OnAwake();

        }

        private void Start()
        {
            Log("Version : " + ModuleVersion);
            OnStart();
        }

        /// <summary>
        /// Initialize should be called in an initialization callback or on Start
        /// </summary>
        public void Initialize()
        {
            if (!MustBeInitialized())
            {
                LogWarning("Module disabled by Config !");
                Clear();
                return;
            }
            
            if (_isInitialized)
            {
                LogWarning("Module already initialized !");
                return;
            }

            Log("Initialization started...");
            _timer = Time.time;

            if (_useCheckInit)
            {
                _checkInitCoroutine = CheckInitialization();
                StartCoroutine(_checkInitCoroutine);
            }

            InitializeModule();
            Callbacks._onInitialization?.Invoke();

            FGAnalytics.NewDesignEvent(EVENT_INITIALISATION_START, new Dictionary<string, object>()
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_VERSION, ModuleVersion }
            });
        }

        protected void InitializationComplete(bool success)
        {
            if (_isInitialized)
            {
                LogWarning("End of initialization has already been triggered !");
                return;
            }

            _totalInitTime = Time.time - _timer;
            if (_checkInitCoroutine != null) StopCoroutine(_checkInitCoroutine);
            if (success) Log("...initialization succeeded after " + _totalInitTime + " secs !");
            else LogError("...initialization failed after " + _totalInitTime + " secs !");

            _isInitialized = success;
            Callbacks._onInitialized?.Invoke(success);
            FGAnalytics.NewDesignEvent(EVENT_INITIALISATION_COMPLETE, new Dictionary<string, object>()
            {
                { EVENT_PARAM_MODULE, ModuleName },
                { EVENT_PARAM_VERSION, ModuleVersion },
                { EVENT_PARAM_SUCCESS, success },
                { EVENT_PARAM_INIT_TIME, _totalInitTime }
            });
        }

        internal void ForceInitialization()
        {
            InitializeCallbacks();
            Initialize();
        }

        public void Log(string message)
        {
            if (!FGDebugSettings.settings.logEnabled) return;
            if (message.Contains(EVENT_INITIALISATION_START) || message.Contains(EVENT_INITIALISATION_COMPLETE)) return;
            FGDebug.Log(FormatLog(message), LogColor);
        }

        public void LogError(string message)
        {
            if (!FGDebugSettings.settings.logEnabled) return;
            FGDebug.Log(FormatLog(message), Color.red);
        }

        public void LogWarning(string message)
        {
            if (!FGDebugSettings.settings.logEnabled) return;
            Debug.LogWarning(FormatLog(message));
        }

        private string FormatLog(string message)
        {
            return "[FG " + ModuleName.Trim() + "] " + message;
        }

        private IEnumerator CheckInitialization()
        {
            float counter = 0;
            float iteration = 0.2f;
            float maxWaitingDelay = _maxInitTime;
            while (counter <= maxWaitingDelay && !IsInitialized())
            {
                yield return new WaitForSeconds(iteration);
                counter += iteration;
            }

            InitializationComplete(IsInitialized());
        }

        protected void InitWithoutTimer()
        {
            _useCheckInit = false;
        }

        protected void SetMaxInitTime(float time)
        {
            _maxInitTime = time;
        }

        public virtual void Clear()
        {
            ClearInitialization();
            Callbacks.Clear();
            _isInitialized = false;
        }
    }
}