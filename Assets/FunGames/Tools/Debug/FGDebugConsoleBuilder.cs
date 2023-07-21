using System;
using System.Collections.Generic;
using FunGames.RemoteConfig;
using FunGames.Tools.Debugging;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-39)]
public class FGDebugConsoleBuilder : MonoBehaviour
{
    public GameObject container;
    
    public RectTransform menuButtons;

    public Text moduleNameText;
    
    public Button previousButton;

    public Button nextButton;

    public List<DebugElement> menus = new List<DebugElement>();

    private List<DebugElement> pool = new List<DebugElement>();

    private int _currentIndex = 0;
    
    [SerializeField] private Sprite spriteOpenIcon;
    [SerializeField] private Sprite spriteCloseIcon;
    private Image uiToggleButtonImage;
    [SerializeField] private Button uiToggleButton;
    [SerializeField] private FGDebugConsole console;
    private bool isOpen = false;

    private const string RC_DEBUGMODE = "DebugMode";

    private void Awake()
    {
        FGRemoteConfig.AddDefaultValue(RC_DEBUGMODE, 0);
        container.SetActive(false);
        console.Init();
        uiToggleButtonImage = uiToggleButton.GetComponent<Image>();
        uiToggleButton.onClick.AddListener(ToggleLogUI);
        FGRemoteConfig.Callbacks.OnInitialized += DisplayConsole;
    }

    private void DisplayConsole(bool obj)
    {
        container.SetActive(FGRemoteConfig.GetBooleanValue(RC_DEBUGMODE));
    }

    // Start is called before the first frame update
    void Start()
    {
        previousButton.onClick.AddListener(ShowPrevious);
        nextButton.onClick.AddListener(ShowNext);

        for (int i = 0; i < menus.Count; i++)
        {
            if(menus[i] == null || menus[i].debugPanel == null) continue;
            GameObject obj = Instantiate(menus[i].debugPanel, menuButtons.transform);
            pool.Add(new DebugElement(menus[i].moduleName, obj));
            obj.SetActive(false);
        }

        if (pool.Count == 0) return;
        moduleNameText.text = menus[_currentIndex].moduleName;
        pool[0].debugPanel.SetActive(true);
    }
    
    private void ToggleLogUI(){
        isOpen = !isOpen;
        uiToggleButtonImage.sprite = isOpen ? spriteCloseIcon : spriteOpenIcon;
        console.transform.parent.gameObject.SetActive(isOpen);
    }

    private void ShowPrevious()
    {
        if (pool.Count == 0) return;

        pool[_currentIndex].debugPanel.SetActive(false);
        if (_currentIndex == 0) _currentIndex = pool.Count - 1;
        else _currentIndex--;
        moduleNameText.text = menus[_currentIndex].moduleName;
        pool[_currentIndex].debugPanel.SetActive(true);
    }

    private void ShowNext()
    {
        if (pool.Count == 0) return;

        pool[_currentIndex].debugPanel.SetActive(false);
        if (_currentIndex == pool.Count - 1) _currentIndex = 0;
        else _currentIndex++;
        moduleNameText.text = menus[_currentIndex].moduleName;
        pool[_currentIndex].debugPanel.SetActive(true);
    }
}

[Serializable]
public class DebugElement
{
    public DebugElement(string moduleName, GameObject debugPanel)
    {
        this.moduleName = moduleName;
        this.debugPanel = debugPanel;
    }
    
    public string moduleName;
    public GameObject debugPanel;
}