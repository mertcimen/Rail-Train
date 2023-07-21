using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    [InfoBox("Main Camera with CinemachineBrain is required.", InfoMessageType.Error, "CheckForCameraBrain")]
    [ReadOnly, Required]
    public CinemachineBrain mainCameraBrain;

    [SerializeField] private CinemachineVirtualCamera defaultVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera targetedVirtualCamera;

    private CinemachineVirtualCamera _activeVirtualCamera;

    private List<Transition> _transitions;
   

    public CameraManager Initialize()
    {
        mainCameraBrain = GetComponentInChildren<CinemachineBrain>();

        _activeVirtualCamera = defaultVirtualCamera;
        _transitions = new List<Transition>();

        return this;
    }

    public void SetPlayerTarget(Transform playerTransform)
    {
        defaultVirtualCamera.gameObject.SetActive(true);
        
        defaultVirtualCamera.m_Follow = playerTransform;
        defaultVirtualCamera.m_LookAt = playerTransform;
    }

    public void ActivateVirtualCamera(CinemachineVirtualCamera virtualCamera = null)
    {
        if (virtualCamera)
        {
            _activeVirtualCamera = virtualCamera;
            virtualCamera.gameObject.SetActive(true);
        }
        else
        {
            _activeVirtualCamera.gameObject.SetActive(false);
            _activeVirtualCamera = defaultVirtualCamera;
            _activeVirtualCamera.gameObject.SetActive(true);
        }
    }

    private CinemachineVirtualCamera _activeTargetCamera;

    public void SetCameraTarget(Transform targetTransform)
    {
        _activeTargetCamera = Instantiate(targetedVirtualCamera, transform, true);
        _activeTargetCamera.m_Follow = targetTransform;
        _activeTargetCamera.m_LookAt = targetTransform;

        ActivateVirtualCamera(_activeTargetCamera);
    }

    public void AddCameraTarget(Transform targetTransform, Action onCompleteAction = null, float actionDelay = 0)
    {
        var transition = new Transition()
        {
            TransitionTarget = targetTransform,
            TransitionCompleteAction = onCompleteAction,
            TransitionDelay = actionDelay
        };

        _transitions.Add(transition);

        ShowNextTarget();
    }

    public void RemoveCameraTarget(Transform targetTransform)
    {
        if (!_activeTargetCamera || _activeTargetCamera.m_Follow != targetTransform)
            return;

        ActivateVirtualCamera();
        Destroy(_activeTargetCamera.gameObject, TransitionTime + .1f);
    }

    private Transition _activeTransition;

    private void ShowNextTarget()
    {
        if (_transitions.Count <= 0)
        {
            ActivateVirtualCamera();
            return;
        }

        if (_activeTransition == null)
        {
            StartCoroutine(TransitionCoroutine());
        }
    }

    private const float TransitionTime = 1f;

    private IEnumerator TransitionCoroutine()
    {
        _activeTransition = _transitions[0];
        //Go to targeted transform
        var newTarget = Instantiate(targetedVirtualCamera, transform, true);
        newTarget.m_Follow = _activeTransition.TransitionTarget;
        newTarget.m_LookAt = _activeTransition.TransitionTarget;
        _transitions.RemoveAt(0);
        ActivateVirtualCamera(newTarget);

        //Wait for transition
        yield return new WaitForSeconds(TransitionTime);

        _activeTransition.TransitionCompleteAction?.Invoke();

        yield return new WaitForSeconds(_activeTransition.TransitionDelay);

        _activeTransition = null;
        Destroy(newTarget.gameObject, TransitionTime + .1f);
        ShowNextTarget();
    }

    private class Transition
    {
        public Transform TransitionTarget;
        public Action TransitionCompleteAction;
        public float TransitionDelay;
    }

    //Used by OdinInspector for Editor Control
    private bool CheckForCameraBrain()
    {
        mainCameraBrain = GetComponentInChildren<CinemachineBrain>();

        return mainCameraBrain is null;
    }
}