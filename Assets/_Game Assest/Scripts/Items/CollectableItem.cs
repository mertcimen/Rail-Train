using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectableItem : ItemBase
{
    public bool isCollected;
    public CollectableType collectableItemType;
    public int itemAmount;


    public void SetCollectable()
    {
        transform.DOScale(Vector3.one * 1.3f, 0.5f).OnComplete((() => { transform.DOScale(Vector3.one, 0.3f); }));
    }
}

public enum CollectableType
{
    Wood,
    Strawberry,
    Pumpkin,
    Cabbage,
    Passenger,
    Gold,
    Package1,
}