using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;


public class ItemHolder : MonoBehaviour
{
    public ItemBase CurrentItem;

    public Vector2Int GridPosition;


    public GameObject collectableIndicatorPrefab;

    public GameObject indicator;
    public bool isForTutorial;
    public bool isForTutorialCorner;
    public GameObject tutorialIndicator;
    public GameObject tutorialIndicatorCorner;
    [SerializeField] private Vector3 indicatorRotation;
    public Action OnTargetSet;

    public GameObject tutorialRailObj;

    [ReadOnly] public int ID;

    public void SetItem(ItemBase item)
    {
        CurrentItem = item;

        OnTargetSet?.Invoke();
    }

    public int GetId()
    {
        return ID;
    }

    public void SetCurrentItem()
    {
        CurrentItem = GetComponentInChildren<ItemBase>();

        if (CurrentItem is null)
        {
            CurrentItem = Instantiate(GameManager.Instance.levelManager.currentLevel.railObject, transform.position,
                transform.rotation, transform);
            CurrentItem.name = "Rail" + GridPosition.ToString();
            (CurrentItem as Rail).Holder = this;
        }

        if (CurrentItem is CollectableItem)
        {
            indicator = Instantiate(collectableIndicatorPrefab, transform.position, quaternion.identity, transform);
            indicator.transform.localRotation = Quaternion.Euler(new Vector3(270, 0, 0));
            indicator.transform.localScale = Vector3.one * 0.5f;
        }

        if (GameManager.Instance.levelManager.currentLevel.isTutorialLevel && !isForTutorial)
        {
            var collider = GetComponent<Collider>();
            collider.enabled = false;
        }

        if (GameManager.Instance.levelManager.currentLevel.isTutorialLevel && isForTutorial && isForTutorialCorner)
        {
            tutorialRailObj = Instantiate(tutorialIndicatorCorner, transform.position,
                quaternion.Euler(indicatorRotation),
                transform);
            return;
        }

        if (GameManager.Instance.levelManager.currentLevel.isTutorialLevel && isForTutorial)
        {
            tutorialRailObj = Instantiate(tutorialIndicator, transform.position, Quaternion.Euler(indicatorRotation),
                transform);
        }
    }
}