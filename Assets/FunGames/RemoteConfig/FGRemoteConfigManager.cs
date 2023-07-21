using System.Collections.Generic;
using FunGames.RemoteConfig.TNRemoteConfig;
using FunGames.Tools;
using FunGames.Tools.Core.Modules;
using FunGames.Tools.Debugging;
using UnityEngine;

namespace FunGames.RemoteConfig
{
    public class FGRemoteConfigManager : FGModuleParent<FGRemoteConfigManager, FGModuleCallbacks>
    {
        public override string ModuleName => "Remote Config";
        protected override string RemoteConfigKey => "FGRemoteConfig";
        public override Color LogColor => FGDebugSettings.settings.RemoteConfig;

        public Dictionary<string, object> ValuesDictionary = new Dictionary<string, object>();

        public FGABTest CurrentABTest;

        protected override void InitializeCallbacks()
        {
            FunGamesSDK.Callbacks.Initialization += Initialize;
        }

        public void InitializeDefaultValues()
        {
            Dictionary<string, object> defaultValues = GetCustomDefaultValues();
            foreach (var defaultValue in defaultValues)
            {
                ValuesDictionary.Add(defaultValue.Key, defaultValue.Value);
            }
        }


        public object GetValueByKey(string key)
        {
            if (ValuesDictionary is null || key is null) return null;

            return ValuesDictionary[key];
        }

        private Dictionary<string, object> GetCustomDefaultValues()
        {
            Dictionary<string, object> defaultValues = new Dictionary<string, object>();
            if (!FGRemoteConfigSettings.settings) return defaultValues;
            foreach (var remoteConf in FGRemoteConfigSettings.settings.CustomDefaultValues)
            {
                defaultValues.Add(remoteConf.name, remoteConf.value);
            }

            return defaultValues;
        }

        /// <summary>
        /// Must be called on Awake.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddDefaultValues( Dictionary<string, object> defaultValues)
        {
            foreach (var rcVariable in defaultValues)
            {
                AddDefaultValue(rcVariable.Key, rcVariable.Value);
            }
        }

        /// <summary>
        /// Must be called on Awake.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddDefaultValue(string key, object value)
        {
            if (ValuesDictionary is null || key is null)
            {
                LogWarning("ValuesDictionary or key is null");
                return;
            }

            if (ValuesDictionary.ContainsKey(key))
            {
                LogWarning("Default value already added to the dictionary.");
                return;
            }
            ValuesDictionary.Add(key, value);
        }
        
        /// <summary>
        /// Must be called on Awake.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void OverrideDefaultValue(string key, object value)
        {
            if (ValuesDictionary is null || key is null)
            {
                LogWarning("ValuesDictionary or key is null");
                return;
            }

            if (!ValuesDictionary.ContainsKey(key))
            {
                LogWarning("Can't override default because it doesn't exists in the dictionary.");
                return;
            }
            ValuesDictionary[key] = value;
        }

        protected override void ClearInitialization()
        {
            FunGamesSDK.Callbacks.Initialization -= Initialize;
        }
    }
}