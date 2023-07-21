using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialHighlight : MonoBehaviour
{
    public const float ShowDuration = .5f;

    [ChildGameObjectsOnly] public Button screenButton;

    [SerializeField, ChildGameObjectsOnly] private Transform unmask;
    [SerializeField, ChildGameObjectsOnly] private Transform hand;
    [SerializeField, ChildGameObjectsOnly] private TextMeshProUGUI hintText;

    private Sprite _circleDefault;
    private Image _unmaskImage;
    private Image _handImage;
    private Animator _handAnimator;
    private Camera _mainCamera;
    
    public static readonly int Scratch = Animator.StringToHash("Scratch");

    private void Awake()
    {
        if (!DataManager.IsTutorial)
            gameObject.SetActive(false);
        
        _mainCamera = Camera.main;
        _unmaskImage = unmask.GetComponent<Image>();
        _handImage = hand.GetComponent<Image>();
        _circleDefault = _unmaskImage.sprite;
        _handAnimator = hand.GetComponent<Animator>();
    }

    public void SetHandTrigger(int id)
    {
        _handAnimator.SetTrigger(id);
    }

    public void ShowWorld(Transform targetTransform, float targetScale = 1f)
    {
        ShowUI(_mainCamera.WorldToScreenPoint(targetTransform.position), targetScale);
    }

    public void ShowWorld(Transform targetTransform, Vector3 offset ,float targetScale = 1f)
    {
        ShowUI(_mainCamera.WorldToScreenPoint(targetTransform.position + offset), targetScale);
    }
    
    public void ShowUI(Vector3 screenPosition, Sprite unMaskSprite, string hint, float targetScale = 1f)
    {
        ShowUI(screenPosition, targetScale);

        _unmaskImage.sprite = unMaskSprite;
        hand.gameObject.SetActive(false);

        hintText.DOKill();
        hintText.text = hint;
        hintText.gameObject.SetActive(true);
        hintText.transform.position = screenPosition + 256 * Vector3.down;
        hintText.color = Color.clear;
        hintText.DOColor(Color.white, .2f).SetDelay(ShowDuration);
    }

    public void ShowUI(Vector3 screenPosition, float targetScale = 1f, bool handEnabled = true)
    {
        hintText.gameObject.SetActive(false);

        _unmaskImage.sprite = _circleDefault;

        unmask.DOKill();
        unmask.transform.position = screenPosition;
        unmask.DOScale(targetScale, ShowDuration).SetEase(Ease.OutCubic);

        hand.gameObject.SetActive(handEnabled);
        hand.transform.position = screenPosition;
    }

    public void Hide(bool disableAfter = false)
    {
        unmask.DOKill();

        unmask.DOMove(new Vector3(Screen.width * .5f, Screen.height * .5f), ShowDuration);
        unmask.DOScale(30f, ShowDuration).SetEase(Ease.InCubic).OnComplete(() =>
        {
            if (disableAfter)
                gameObject.SetActive(false);
        });

        hand.gameObject.SetActive(false);
        hintText.gameObject.SetActive(false);
    }
    
    private bool _isTutorial = true;
    private IEnumerator TutorialCoroutineWorld(Vector3 startPosition, Vector3 endPosition)
    {
        while(_isTutorial)
        {
            yield return new WaitForSeconds(.2f);

            _handImage.transform.position = _mainCamera.WorldToScreenPoint(startPosition);
                    
            _handImage.DOColor(Color.white, .2f);
            _handImage.transform.DOScale(.75f, .2f);
                    
            yield return new WaitForSeconds(.2f);

            _handImage.transform.DOMove(_mainCamera.WorldToScreenPoint(endPosition), 1.5f)
                .SetEase(Ease.InOutCubic);
            
            yield return new WaitForSeconds(1.5f);
            
            _handImage.DOColor(Color.clear, .2f);
            _handImage.transform.DOScale(1f, .2f);
            
            yield return new WaitForSeconds(.5f);
        }
    }
    
    private IEnumerator TutorialCoroutineUI(Vector3 startPosition, Vector3 endPosition)
    {
        while(_isTutorial)
        {
            yield return new WaitForSeconds(.2f);

            _handImage.transform.position = startPosition;
                    
            _handImage.DOColor(Color.white, .2f);
            _handImage.transform.DOScale(.75f, .2f);
                    
            yield return new WaitForSeconds(.2f);

            _handImage.transform.DOMove(endPosition, 1.5f)
                .SetEase(Ease.InOutCubic);
            
            yield return new WaitForSeconds(1.5f);
            
            _handImage.DOColor(Color.clear, .2f);
            _handImage.transform.DOScale(1f, .2f);
            
            yield return new WaitForSeconds(.5f);
        }
    }
}