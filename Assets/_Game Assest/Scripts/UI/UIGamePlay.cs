using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIGamePlay : MonoBehaviour
{
    public GameObject TutoHand;


    public List<TrainTasks> TrainTasksList = new List<TrainTasks>();

    private bool canShowTuto = true;
    public TaskInfo taskPrefab;

    public void SetTutoHand(Vector3 targetPos, Vector3 secTargetPos)
    {
        var pos = Camera.main.WorldToScreenPoint(targetPos);
        var secPos = Camera.main.WorldToScreenPoint(secTargetPos);
        TutoHand.transform.position = pos;
        TutoHand.transform.localScale = Vector3.one;
        TutoHand.SetActive(true);


        if (DataManager.CurrentLevelIndex > 0)
        {
            TutoHand.transform.DOScale(Vector3.one * .5f, .4f).OnComplete(() =>
            {
                TutoHand.transform.DOMove(new Vector3(pos.x, secPos.y), 1.5f).OnComplete((() =>
                {
                    TutoHand.transform.DOMove(secPos, 1f).OnComplete((() =>
                    {
                        TutoHand.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete((() =>
                        {
                            if (canShowTuto)
                            {
                                SetTutoHand(targetPos, secTargetPos);
                            }
                            else
                            {
                                TutoHand.SetActive(false);
                            }
                        }));
                    }));
                }));
            });
        }
        else
            TutoHand.transform.DOScale(Vector3.one * .5f, .4f).OnComplete(() =>
            {
                TutoHand.transform.DOMove(secPos, 1.5f).OnComplete((() =>
                {
                    TutoHand.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete((() =>
                    {
                        if (canShowTuto)
                        {
                            SetTutoHand(targetPos, secTargetPos);
                        }
                        else
                        {
                            TutoHand.SetActive(false);
                        }
                    }));
                }));
            });
    }


    public void ResetTutoHand()
    {
        canShowTuto = false;
        TutoHand.transform.DOKill();
        TutoHand.SetActive(false);
    }


    public List<TaskInfo> activeTaskList = new List<TaskInfo>();

    public void SetTask(Task task)
    {
        if (task.traintype == TrainType.First)
        {
            var _task = Instantiate(taskPrefab);
            _task.SetTaskInfo(task);
            var Parent = TrainTasksList.Where(x => x.TrainType == TrainType.First).ToList();
            _task.transform.SetParent(Parent[0].taskGroup.transform);
            _task.transform.DOScale(Vector3.one, 0.3f);
            _task.targetTrain = TrainType.First;
            activeTaskList.Add(_task);
        }

        if (task.traintype == TrainType.Second)
        {
            var _task = Instantiate(taskPrefab);
            _task.SetTaskInfo(task);
            var Parent = TrainTasksList.Where(x => x.TrainType == TrainType.Second).ToList();
            _task.transform.SetParent(Parent[0].taskGroup.transform);
            _task.transform.DOScale(Vector3.one, 0.3f);
            _task.targetTrain = TrainType.Second;
            activeTaskList.Add(_task);
        }
    }


    public void CheckCollectedIsATask(CollectableItem collectedItem, TrainType targetType)
    {
        var matchedTasks = activeTaskList
            .Where(x => x.targetTrain == targetType && x.neededType == collectedItem.collectableItemType).ToList();
        if (matchedTasks.Count > 0)
        {
            matchedTasks[0].SetInfoAfterLoad(collectedItem);
            if (matchedTasks[0].neededAmount == matchedTasks[0].collectedAmount)
            {
                matchedTasks[0].isDone = true;
                // matchedTasks[0].taskIcon.
                // matchedTasks[0].taskText.color = Color.green;
                matchedTasks[0].checkMark.SetActive(true);
            }
        }
    }
}