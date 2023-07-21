using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisabledColor : MonoBehaviour
{
    [SerializeField] private Color disabledColor;

    // ReSharper disable InconsistentNaming
    private Action OnDisabled, OnEnabled;
    
    private void Awake()
    {
        if (TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
        {
            var startColor = textMeshProUGUI.color;
            
            OnEnabled += () => textMeshProUGUI.color = startColor;
            OnDisabled += () => textMeshProUGUI.color = disabledColor;
        }

        if (TryGetComponent(out Image image))
        {
            var startColor = image.color;

            OnEnabled += () => image.color = startColor;
            OnDisabled += () => image.color = disabledColor;
        }
    }

    public void SetState(bool state)
    {
        if (state)
        {
            OnEnabled?.Invoke();
        }
        else
        {
            OnDisabled?.Invoke();
        }
    }
}
