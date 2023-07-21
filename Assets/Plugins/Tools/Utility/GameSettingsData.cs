using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings Data", menuName = "Scriptable Object / Game Settings Data")]
public class GameSettingsData : ScriptableObject
{
    [Title("Game Settings", "Contains Game Settings Data", TitleAlignments.Centered, Bold = true)]
    private const float Time = 1f;
    
    [Header("Debug Logs")]
    public bool enableLogs = true;
    [Header("Skip Main Menu UI")]
    public bool skipReadyState = true;
}