using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private Vector3 lastPosition;


    private float checkInterval = 3f;

    private bool isMoved;

    private void Update()
    {
        checkInterval -= Time.deltaTime;
        if (isMoved)
        {
            transform.DOKill();
        }

        if (checkInterval < 0)
        {
            if (!isMoved)
            {
                StartMove();
            }
            // if (Vector3.Distance(lastPosition, transform.position) > 1f)
            // {
            //     StartMove();
            // }
            // else
            // {
            //     moveTween.Kill();
            // }

            checkInterval = 3f;
        }
    }


    void Start()
    {
        lastPosition = transform.position;
    }

    public void SetlastPos()
    {
        transform.DOKill();
        isMoved = false;
        lastPosition = transform.position;
    }

    public void ResetTime()
    {
        isMoved = true;
        checkInterval = 3f;
    }

    private Tween moveTween;

    private void StartMove()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        moveTween.Kill();
        moveTween = transform.DOScale(Vector3.one * 1.2f, .8f).SetLoops(-1, LoopType.Yoyo);
    }
}