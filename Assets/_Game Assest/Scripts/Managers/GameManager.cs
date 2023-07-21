using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Action OnGameStateChanged;

    private GameState _gameState;

    [ShowInInspector, ReadOnly]
    public GameState GameState
    {
        get => _gameState;
        private set
        {
            _gameState = value;
            OnGameStateChanged?.Invoke();
        }
    }

    [Space(10)] [Header("Game Settings")] [Required, SerializeField]
    public GameSettingsData gameSettingsData;

    [Space(10)] [Header("Singleton Container")] [ReadOnly, Required]
    public TouchManager touchManager;

    [ReadOnly, Required] public AudioManager audioManager;
    [ReadOnly, Required] public LevelManager levelManager;
    [ReadOnly, Required] public UIManager uiManager;
    [ReadOnly, Required] public CameraManager cameraManager;
    [ReadOnly, Required] public NotificationManager notificationManager;
    [ReadOnly, Required] public ObjectPoolingManager objectPoolingManager;

    protected override void Init()
    {
        base.Init();

        GameState = GameState.Loading;

        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        AnalyticsManager.Initialize();
        HapticManager.SetHapticsActive(DataManager.Vibration);

        LogManager.Initialize(gameSettingsData.enableLogs);
        OnGameStateChanged += () => LogManager.Log($"Game State is : {GameState}");

        objectPoolingManager = GetComponentInChildren<ObjectPoolingManager>(true).Initialize();
        cameraManager = GetComponentInChildren<CameraManager>(true).Initialize();
        touchManager = GetComponentInChildren<TouchManager>(true).Initialize();
        audioManager = GetComponentInChildren<AudioManager>(true).Initialize(); //This depends on CameraManager
        levelManager = GetComponentInChildren<LevelManager>(true).Initialize();
        uiManager = GetComponentInChildren<UIManager>(true).Initialize();
        uiManager.overlay.cloudsAnimator.SetTrigger("Start");
        notificationManager = GetComponentInChildren<NotificationManager>(true).Initialize();
    }

    private void Start()
    {
        GameState = GameState.Ready;

        if (gameSettingsData.skipReadyState)
            LevelStart();
    }

    public void LevelStart()
    {
        if (GameState != GameState.Ready)
        {
            LogManager.LogError("Game State is not Ready", this);
            return;
        }

        GameState = GameState.Gameplay;

        AnalyticsManager.OnLevelStart();
    }

    public void LevelFinish(bool isSuccess)
    {
        if (GameState != GameState.Gameplay)
        {
            LogManager.LogError("Game State is not Gameplay", this);
            return;
        }

        GameState = isSuccess ? GameState.Complete : GameState.Fail;

        AnalyticsManager.OnLevelFinish(isSuccess ? EndType.Success : EndType.Fail);

        if (isSuccess)
        {
            DataManager.CurrentLevelIndex++;
        }
    }

    private void OnValidate()
    {
        touchManager = GetComponentInChildren<TouchManager>(true);
        audioManager = GetComponentInChildren<AudioManager>(true);
        levelManager = GetComponentInChildren<LevelManager>(true);
        uiManager = GetComponentInChildren<UIManager>(true);
        cameraManager = GetComponentInChildren<CameraManager>(true);
        notificationManager = GetComponentInChildren<NotificationManager>(true);
        objectPoolingManager = GetComponentInChildren<ObjectPoolingManager>(true);
    }
}