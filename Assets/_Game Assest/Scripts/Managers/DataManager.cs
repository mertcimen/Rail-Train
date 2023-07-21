using System;
using UnityEngine;

// ReSharper disable InconsistentNaming
public static class DataManager
{
    public static bool Vibration
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.VibrationKey, 1) == 1;
        set => PlayerPrefs.SetInt(PlayerPrefKeys.VibrationKey, value ? 1 : 0);
    }

    public static Action<bool> OnSoundStateChanged;
    public static bool Sound
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.SoundKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.SoundKey, value ? 1 : 0);
            OnSoundStateChanged?.Invoke(value);
        }
    }
    
    public static Action<bool> OnMusicStateChanged;
    public static bool Music
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.SoundKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.SoundKey, value ? 1 : 0);
            OnSoundStateChanged?.Invoke(value);
        }
    }
    
    public static bool IsTutorial
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.TutorialKey, 1) == 1;
        set => PlayerPrefs.SetInt(PlayerPrefKeys.TutorialKey, value ? 1 : 0);
    }

    public static int CurrentLevelIndex
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.CurrentLevelIndexKey, 0);
        set => PlayerPrefs.SetInt(PlayerPrefKeys.CurrentLevelIndexKey, value);
    }

    public static Action<int> OnCurrencyUpdated;
    public static int Currency
    {
        get => PlayerPrefs.GetInt(PlayerPrefKeys.CurrencyKey, 0);
        set
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.CurrencyKey, value);
            OnCurrencyUpdated?.Invoke(value);
        } 
    }

    private struct PlayerPrefKeys
    {
        public const string VibrationKey = "Vibration";
        public const string SoundKey = "Sound";
        public const string MusicKey = "Sound";
        
        public const string TutorialKey = "Tutorial";
        
        public const string CurrentLevelIndexKey = "CurrentLevelIndex";
        public const string CurrencyKey = "Currency";
    }
}