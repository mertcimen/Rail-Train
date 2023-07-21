using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject noInternetPopUp;

    public Animator cloudsAnimator;
    public GameObject failPanel;
    public Button rewindButton;

    private void OnEnable()
    {
        DataManager.OnCurrencyUpdated += UpdateCurrencyText;
    }

    private void Start()
    {
        UpdateCurrencyText(DataManager.Currency);
        InvokeRepeating(nameof(CheckForInternetConnection), 1f, 3f);
    }

    public void Restart()
    {
        LevelManager.ReloadScene();
    }

    public void OnRewind()
    {
        HapticManager.GenerateHaptic(PresetType.LightImpact);
        GameManager.Instance.levelManager.currentLevel.touchController.RewindRails();
    }

    private void CheckForInternetConnection()
    {
        var isReachable = Application.internetReachability != NetworkReachability.NotReachable;
        noInternetPopUp.SetActive(!isReachable);
    }

    private void UpdateCurrencyText(int value)
    {
        currencyText.text = value.LargeIntToString();
    }

    private void OnDisable()
    {
        DataManager.OnCurrencyUpdated -= UpdateCurrencyText;
    }
}