using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [ReadOnly] public LevelData currentLevelData;
    [ReadOnly] public LevelController currentLevel => currentLevelData.levelController;

    [SerializeField] private List<LevelData> levels;

    private LevelData levelToInitialize;

    public LevelManager Initialize()
    {
        if (FindObjectOfType<LevelController>())
        {
            currentLevelData = new LevelData
            {
                levelId = "Test Level",
                levelController = FindObjectOfType<LevelController>()?.Initialize()
            };

            LogManager.LogWarning("Level Initialized From Scene !", this);
            return this;
        }

        if (levels is null || levels.Count <= 0)
        {
            LogManager.LogError("Levels Missing !", this);
            return this;
        }

        // var levelToInitialize = levels[DataManager.CurrentLevelIndex % levels.Count];
        if (DataManager.CurrentLevelIndex > 10)
        {
            levelToInitialize = levels[((DataManager.CurrentLevelIndex - 10) % (levels.Count - 10)) + 10];

            currentLevelData = new LevelData
            {
                levelId = levelToInitialize.levelId,
                levelController = Instantiate(levelToInitialize.levelController).Initialize()
            };
        }
        else
        {

            levelToInitialize = levels[DataManager.CurrentLevelIndex % levels.Count];
            currentLevelData = new LevelData
            {
                levelId = levelToInitialize.levelId,
                levelController = Instantiate(levelToInitialize.levelController).Initialize()
            };
        }

        return this;
    }

    public static void ReloadScene()
    {
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

[Serializable]
public class LevelData
{
    public string levelId;
    public LevelController levelController;
}