using System;
using System.Collections;
using FunGames.Mediation;
using FunGames.Tools.Core.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FunGames.Tools
{
    public class FGLoadingScreen : MonoBehaviour
    {
        public Image ProgressBar;

        public LoadedAction ActionWhenLoaded = LoadedAction.LoadNextScene;

        public float MaxLoadingTime = 10;

        public ProgressMode ProgressMode = ProgressMode.LoadWithInit;

        private IEnumerator _checkInitCoroutine;
        private bool _useCheckInit = true;
        private float _increment = 0;
        private Action<bool> _onSdkInitialized;

        private void Awake()
        {
            _checkInitCoroutine = CheckInitialization();
            StartCoroutine(_checkInitCoroutine);

            FGMediation.Callbacks.OnAppOpenDisplayed += Close;

            if (ProgressMode.Equals(ProgressMode.LoadWithInit))
            {
                _onSdkInitialized = delegate { CloseLoadingScreen(); };
                FunGamesSDK.Callbacks.OnInitialized += _onSdkInitialized;
            }
               
        }

        private void Close(FGAdInfo obj)
        {
            CloseLoadingScreen();
        }

        private void Update()
        {
            if (ProgressBar == null) return;
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            switch (ProgressMode)
            {
                case ProgressMode.LoadWithInit:
                    ProgressBar.fillAmount = 1 - (FGMain.Instance.ChildrenToInitialize / FGMain.Instance.TotalModules);
                    break;
                case ProgressMode.LoadWithTime:
                    ProgressBar.fillAmount += Time.deltaTime/MaxLoadingTime;
                    break;
            }
        }

        private void CloseLoadingScreen()
        {
            switch (ActionWhenLoaded)
            {
                case LoadedAction.ClosePanel:
                {
                    gameObject.SetActive(false);
                    break;
                }
                case LoadedAction.LoadNextScene:
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                }
            }
        }

        private IEnumerator CheckInitialization()
        {
            float counter = 0;
            float iteration = 0.2f;
            float maxWaitingDelay = MaxLoadingTime;
            while (counter <= maxWaitingDelay)
            {
                yield return new WaitForSeconds(iteration);
                counter += iteration;
            }

            CloseLoadingScreen();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (_checkInitCoroutine != null) StopCoroutine(CheckInitialization());
            FGMediation.Callbacks.OnAppOpenDisplayed -= Close;
            FunGamesSDK.Callbacks.OnInitialized -= _onSdkInitialized; 
        }
    }

    public enum LoadedAction
    {
        LoadNextScene,
        ClosePanel
    }

    public enum ProgressMode
    {
        LoadWithInit,
        LoadWithTime
    }
}