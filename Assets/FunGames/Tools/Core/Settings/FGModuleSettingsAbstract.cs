using System;
using FunGames.Tools.Debugging;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public abstract class FGModuleSettingsAbstract<T> : IFGModuleSettings
    where T : FGModuleSettingsAbstract<T>, new()
{
    protected const int ORDER = 10;

    protected abstract T LoadResources();

    public override string Version
    {
        get => version;
        set
        {
            version = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }

    [Space][HideInInspector] public string version = String.Empty;

    private static T _instance;

    private static object _lock = new object();

    public static T settings
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var s = CreateInstance<T>();
                        _instance = s.LoadResources();
                    }
                }
            }

            return _instance;
        }
    }
}