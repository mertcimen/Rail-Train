using Lofelt.NiceVibrations;
using UnityEngine;

public static class HapticManager
{
    // ReSharper disable Unity.PerformanceAnalysis
    public static void SetHapticsActive(bool status)
    {
        HapticController.hapticsEnabled = status;
    }

    public static void GenerateHaptic(PresetType type)
    {
        if (!HapticController.hapticsEnabled)
            return;
        
        HapticPatterns.PlayPreset((HapticPatterns.PresetType)type);
    }
    
    private static float _lastHapticTime = 0;
    public static void GenerateHapticWithInterval(PresetType type, float interval)
    {
        if (!HapticController.hapticsEnabled || Time.time < interval + _lastHapticTime)
            return;

        _lastHapticTime = Time.time;
        HapticPatterns.PlayPreset((HapticPatterns.PresetType)type);
    }
}

public enum PresetType
{
    Selection = 0,
    Success = 1,
    Warning = 2,
    Failure = 3,
    LightImpact = 4,
    MediumImpact = 5,
    HeavyImpact = 6,
    RigidImpact = 7,
    SoftImpact = 8,
    None = -1
}