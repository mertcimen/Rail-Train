using System;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
public class TouchManager : MonoBehaviour
{
    [SerializeField] private bool ignoreUITouches = true;

    [Header("Is Player Touching"), ShowInInspector, ReadOnly]
    public static bool IsTouching = false;

    public Action<Vector2> OnTouchBegin;
    public Action<Vector3> OnTouchMoveWorld;
    public Action<Vector2> OnTouchMoveScreen;
    public Action OnTouchEnd;

    public TouchManager Initialize()
    {
        return this;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private LeanFinger _activeFinger;

    private void OnFingerDown(LeanFinger obj)
    {
        if (_activeFinger is not null)
            return;

        if (obj.IsOverGui && ignoreUITouches)
            return;

        _activeFinger = obj;
        IsTouching = true;
        OnTouchBegin?.Invoke(obj.ScreenPosition);
    }

    public static float TouchDistance = 1f;

    private void OnFingerUpdate(LeanFinger obj)
    {
        if (obj.IsOverGui && ignoreUITouches)
            return;

        if (obj != _activeFinger)
            return;

        OnTouchMoveWorld?.Invoke(obj.GetWorldDelta(TouchDistance));
        OnTouchMoveScreen?.Invoke(obj.ScreenPosition);
    }

    private void OnFingerUp(LeanFinger obj)
    {
        if (_activeFinger != obj)
            return;

        if (obj.IsOverGui && ignoreUITouches)
            return;

        _activeFinger = null;
        IsTouching = false;
        OnTouchEnd?.Invoke();
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }
}