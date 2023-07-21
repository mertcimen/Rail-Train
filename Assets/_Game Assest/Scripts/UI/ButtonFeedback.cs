using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Animation Feedback")] [SerializeField]
    private bool isAnimated = true;

    [ShowIf("isAnimated")] [SerializeField]
    private bool isScaleBigger = true;

    [Header("Audio Feedback")] [SerializeField]
    private bool hasClick = true;

    [Header("Haptic Feedback")] [SerializeField]
    private bool hasHaptic = true;

    private Button _button;
    private Transform _buttonTargetTransform;
    private Vector3 _startSize;

    private bool _isPointerDown;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonTargetTransform = _button.targetGraphic.transform;

        _startSize = _buttonTargetTransform ? _buttonTargetTransform.transform.localScale : transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        _isPointerDown = true;

        if (hasHaptic)
            HapticManager.GenerateHaptic(PresetType.MediumImpact);

        if (isAnimated)
            ScaleTo(isScaleBigger ? _startSize * 1.1f : _startSize * 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPointerDown)
            _isPointerDown = false;
        else
            return;

        if (!_button.interactable || !isAnimated) return;
        ScaleTo(_startSize);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isPointerDown)
            _isPointerDown = false;
        else
            return;

        if (hasClick)
            GameManager.Instance.audioManager.PlayUIButtonClick();

        if (hasHaptic)
            HapticManager.GenerateHaptic(PresetType.Selection);

        if (!_button.interactable || !isAnimated) return;
        ScaleTo(_startSize);
    }

    private const float ScaleDuration = 0.1f;
    private Tween _scaleTween;

    private void ScaleTo(Vector3 targetScale)
    {
        _scaleTween?.Kill(true);

        _scaleTween = _buttonTargetTransform
            ? _buttonTargetTransform.DOScale(targetScale, ScaleDuration)
            : transform.DOScale(targetScale, ScaleDuration);
    }

    private void OnDestroy()
    {
        _scaleTween?.Kill(true);
    }
}