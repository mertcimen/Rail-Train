using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocomotiveTask : MonoBehaviour
{
    public List<TaskIconsHolder> IconHolder = new List<TaskIconsHolder>();

    public Image taskIcon;
    public TextMeshProUGUI taskText;
    public CollectableType neededType;
    public int neededAmount;
    public int collectedAmount;

    public bool isDone;

    public void SetTaskInfo(Task levelTask)
    {
        taskText.text = collectedAmount + "/" + levelTask.neededCount.ToString();
        neededAmount = levelTask.neededCount;
        var matching = IconHolder.Where(x => x.collectableType == levelTask.TaskType).ToList();
        taskIcon.sprite = matching[0].CollectableIcon;
        neededType = levelTask.TaskType;
    }


    public void SetInfoAfterLoad(CollectableItem collectedItem)
    {
        collectedAmount += collectedItem.itemAmount;
        if (neededAmount == collectedAmount)
        {
            isDone = true;
        }

        taskText.gameObject.SetActive(false);
        taskText.transform.localScale = Vector3.zero;
        taskText.gameObject.SetActive(true);
        taskText.transform.DOScale(Vector3.one, 0.4f);
        taskText.text = collectedAmount + "/" + neededAmount.ToString();
    }
}

[Serializable]
public class TaskIconsHolder
{
    public Sprite CollectableIcon;
    public CollectableType collectableType;
}