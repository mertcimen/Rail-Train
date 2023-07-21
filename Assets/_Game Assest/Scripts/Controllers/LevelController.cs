using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public TouchController touchController;
    public GridManager gridManager;

    public List<Rail> endRails;
    [Header("Train Objects")] public List<Train> trains;
    public List<Train> trainList;

    [Header("Rail Objects")] [AssetsOnly] public Rail railObject;

    public List<Task> taskList = new List<Task>();

    public bool isTutorialLevel;


    public List<WagonCountInfo> wagonInfoList = new List<WagonCountInfo>();

    public LevelController Initialize()
    {
        for (var i = 0; i < trains.Count; i++)
        {
            trains[i] = Instantiate(trains[i], transform);
        }

        gridManager.Initialize();
        touchController.Initialize(this);

        return this;
    }

    private void Start()
    {
        foreach (var VARIABLE in gridManager.Grids)
        {
            VARIABLE.SetCurrentItem();
        }

        FindRailTypesAndSpawnTrain();
        GameManager.Instance.levelManager.currentLevel.touchController.canHistoryRecord = true;
        GameManager.Instance.levelManager.currentLevel.touchController.SetHistory();

        if (isTutorialLevel)
        {
            var tutoRailHolders = gridManager.Grids.Where(x => x.tutorialRailObj != null).ToList();
            var tutoRails = tutoRailHolders.Where(x => x.tutorialRailObj.TryGetComponent(out TutorialRail tutorail))
                .ToList();

            StartCoroutine(Delay());

            IEnumerator Delay()
            {
                for (int i = tutoRails.Count - 1; i > 0; i--)
                {
                    var tutoRail = tutoRails[i].tutorialRailObj.GetComponent<TutorialRail>();
                    yield return new WaitForSeconds(0.3f);

                    tutoRail.SetColor();
                }

                // foreach (var holder in tutoRails)
                // {
                //     var tutoRail = holder.tutorialRailObj.GetComponent<TutorialRail>();
                //     yield return new WaitForSeconds(0.1f);
                //
                //     tutoRail.SetColor();
                // }
            }
        }

        var firstTrainTasks = taskList.Where(x => x.traintype == TrainType.First).ToList();
        var secondTrainTasks = taskList.Where(x => x.traintype == TrainType.Second).ToList();
        if (firstTrainTasks.Count > 0 || secondTrainTasks.Count > 0)
        {
            foreach (var task in taskList)
            {
                GameManager.Instance.uiManager.gamePlay.SetTask(task);
            }
        }

        if (firstTrainTasks.Count <= 0)
        {
            var firstTrainTaskGroup =
                GameManager.Instance.uiManager.gamePlay.TrainTasksList.First(x => x.TrainType == TrainType.First);
            firstTrainTaskGroup.transform.gameObject.SetActive(false);
        }

        if (secondTrainTasks.Count <= 0)
        {
            var secondTrainTaskGroup =
                GameManager.Instance.uiManager.gamePlay.TrainTasksList.First(x => x.TrainType == TrainType.Second);
            Debug.Log(secondTrainTaskGroup.name);
            secondTrainTaskGroup.transform.gameObject.SetActive(false);
        }
    }

    private void SpawnTrain()
    {
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            foreach (var train in trains)
            {
                trainList.Add(train);

                var matchedCountInfo = wagonInfoList.Where(x => x.targetTrain == train.type).ToList();
                if (matchedCountInfo.Count > 0)
                {
                    train.SetWagonCounts(matchedCountInfo[0].wagonCount, matchedCountInfo[0].passWagoncount);
                }

                StartCoroutine(train.Move());
                yield return null;

                train.isTrainMove = false;
            }
        }
    }

    private void FindRailTypesAndSpawnTrain()
    {
        var startRails = gridManager.Grids.Where(x =>
            x.CurrentItem != null && x.CurrentItem.TryGetComponent(out Rail rail) &&
            rail.position == PositionType.Start).ToList();

        var endHolders = gridManager.Grids.Where(x =>
            x.CurrentItem != null && x.CurrentItem.TryGetComponent(out Rail rail) &&
            rail.position == PositionType.End).ToList();

        endRails = endHolders.Select(x => x.CurrentItem.GetComponent<Rail>()).ToList();

        if (startRails.Count == 0 || endHolders.Count == 0)
        {
            LogManager.LogError("Start or End rails is Missing !", this);

            return;
        }

        foreach (var holder in startRails)
        {
            if (holder.CurrentItem.TryGetComponent(out Rail currentRail))
            {
                currentRail.Initialize();

                // while (currentRail.previousRail != null)
                // {
                //     currentRail = currentRail.previousRail;
                // }

                var train = trains.FirstOrDefault(x => x.type == currentRail.targetTrain);

                if (train is null)
                {
                    LogManager.LogError("Start Point Missing target Train !", this);
                    return;
                }

                train.startRail = currentRail;
                if (currentRail.targetTrain == TrainType.First)
                {
                    touchController.SetIndicatorPosition(currentRail.transform.position);
                    if (isTutorialLevel)
                    {
                        var targetEndRail = endHolders.Where(x =>
                                x.CurrentItem.TryGetComponent(out Rail rail) && rail.targetTrain == TrainType.First)
                            .ToList();

                        SetTutoHandOnRail(currentRail, (targetEndRail[0].CurrentItem as Rail));
                    }
                }

                if (currentRail.targetTrain == TrainType.Second)
                {
                    touchController.SetIndicatorPosition(currentRail.transform.position);
                }
            }
        }

        foreach (var holder in endHolders)
        {
            if (holder.CurrentItem.TryGetComponent(out Rail currentRail))
            {
                currentRail.Initialize();

                var train = trains.FirstOrDefault(x => x.type == currentRail.targetTrain);

                if (train is null)
                {
                    LogManager.LogError("End Point Missing target Train !", this);
                    return;
                }

               
            }
        }

        SpawnTrain();

        // CheckTasks();
    }

    private void SetTutoHandOnRail(Rail targetRail, Rail secondTargetRail)
    {
        GameManager.Instance.uiManager.gamePlay.SetTutoHand(targetRail.transform.position,
            secondTargetRail.transform.position);
    }


    private int CompletedTrainCount;

    public void CheckLevelFinish()
    {
        CompletedTrainCount++;

        Debug.Log(CompletedTrainCount);

        if (CompletedTrainCount == trains.Count)
        {
            var doneCount = GameManager.Instance.uiManager.gamePlay.activeTaskList.Where(x => x.isDone).ToList();

            if (doneCount.Count == GameManager.Instance.uiManager.gamePlay.activeTaskList.Count)
            {
                GameManager.Instance.LevelFinish(true);
            }
            else
            {
                GameManager.Instance.LevelFinish(false);
            }

            // if (trains.Count(x => x.isTrainArrive == true) == trains.Count)
            // {
            //     GameManager.Instance.LevelFinish(true);
            //     return;
            // }
            // else
            //     GameManager.Instance.LevelFinish(false);
        }
    }


    // public void EliminateTask(CollectableType type, Train train)
    // {
    //     var eliminatedTask = taskList.Where(x => x.TaskType == type && !x.isDone && train.locomotiveObj.activeTasks. ).ToList();
    //     if (eliminatedTask.Count > 0)
    //     {
    //         eliminatedTask[0].neededCount -= 1;
    //         if (eliminatedTask[0].neededCount <= 0)
    //         {
    //             eliminatedTask[0].isDone = true;
    //         }
    //     }
    //
    //     CheckTasks();
    // }

    // public void CheckTasks()
    // {
    //     var notDoneTasks = taskList.Where(x => !x.isDone).ToList();
    //
    //     Debug.Log(notDoneTasks.Count);
    // }
}

[Serializable]
public class Task
{
    public TrainType traintype;
    public CollectableType TaskType;
    public int neededCount;
}

[Serializable]
public class WagonCountInfo
{
    public TrainType targetTrain;
    public int wagonCount;
    public int passWagoncount;
}