using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private float startVolume;

    private void OnEnable()
    {
        audioSource ??= GetComponent<AudioSource>();
        startVolume = audioSource.volume;

        DataManager.OnSoundStateChanged += OnSoundSettingsChanged;

        audioSource.Play();
        audioSource.volume = DataManager.Sound ? startVolume : 0f;
    }

    private void OnDisable()
    {
        DataManager.OnSoundStateChanged -= OnSoundSettingsChanged;

        audioSource.Stop();
    }

    private Coroutine activeCoroutine;
    private void OnSoundSettingsChanged(bool isSound)
    {
        if (activeCoroutine is not null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        activeCoroutine = StartCoroutine(DoVolume(isSound ? startVolume : 0));
    }

    private void OnValidate()
    {
        audioSource ??= GetComponent<AudioSource>();
    }

    private IEnumerator DoVolume(float targetLevel)
    {
        while (Math.Abs(audioSource.volume - targetLevel) > .0001f)
        {
            yield return null;

            audioSource.volume = Mathf.Lerp(audioSource.volume, targetLevel, 10 * Time.deltaTime);
        }
    }
}