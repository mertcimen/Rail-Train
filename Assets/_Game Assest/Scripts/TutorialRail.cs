using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialRail : MonoBehaviour
{
    public Renderer railRenderer;

    public void SetColor()
    {
        railRenderer.material.DOColor(new Color(1, 1, 1, 0.8f), 0.7f).OnComplete((() =>
        {
            railRenderer.material.DOColor(new Color(1, 1, 1, 0.232f), 0.7f).OnComplete((() => { SetColor(); }));
        }));
    }
}