using UnityEngine;

public class UISettings : MonoBehaviour
{
    [SerializeField] private ToggleButton hapticToggle;
    [SerializeField] private ToggleButton soundToggle;
    [SerializeField] private ToggleButton musicToggle;
    
    private void Start()
    {
        hapticToggle.SetValue(DataManager.Vibration);
        soundToggle.SetValue(DataManager.Sound);
    }

    public void OnSoundToggled(bool value)
    {
        DataManager.Sound = value;
        DataManager.Music = value;
        
        GameManager.Instance.audioManager.UpdateAudioStates();
    }

    public void OnMusicToggled(bool value)
    {
        DataManager.Music = value;
        
        GameManager.Instance.audioManager.UpdateAudioStates();
    }

    public void OnHapticToggled(bool value)
    {
        DataManager.Vibration = value;
        HapticManager.SetHapticsActive(value);
    }
}
