using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private LayerMask itemHolderLayer;

    public List<RailHistory> historyList = new List<RailHistory>();

    public bool canHistoryRecord;

    [SerializeField] private Indicator indicator;

    private Camera _mainCamera;
    private LevelController _levelController;

    public void Initialize(LevelController levelController)
    {
        _mainCamera = Camera.main;
        _levelController = levelController;

        GameManager.Instance.touchManager.OnTouchMoveScreen += OnTouchMove;
        GameManager.Instance.touchManager.OnTouchBegin += OnTouchBegin;
        GameManager.Instance.touchManager.OnTouchEnd += OnTouchEnd;
    }

    private void OnTouchEnd()
    {
        SetHistory();
    }

    private void OnTouchBegin(Vector2 touch)
    {
        activeTrainType = TrainType.None;
        var ray = _mainCamera.ScreenPointToRay(touch);

        if (Physics.Raycast(ray, out var hit, 500f, itemHolderLayer))
        {
            if (hit.transform.TryGetComponent(out ItemHolder holder))
            {
                var currentItem = holder.CurrentItem;

                if (currentItem is not Rail rail)
                    return;

                if (_levelController.isTutorialLevel)
                {
                }

                if (rail.IsSetted())
                {
                    return;
                }
            }
        }
    }

    public void SetHistory()
    {
        if (canHistoryRecord)
        {
            canHistoryRecord = false;

            var holderList = _levelController.gridManager.Grids.Where(x => x.CurrentItem is Rail).ToList();
            var railList = holderList.Select(x => x.CurrentItem as Rail).ToList();

            var history = new RailHistory
            {
                indicatorPosition = indicator.transform.position
            };

            foreach (var rail in railList)
            {
                var trainDirections = new TrainDirectionHistory();
                foreach (var direction in rail.directonList)
                {
                    trainDirections.dirList.Add(new TrainDirection
                    {
                        targetTrain = direction.targetTrain,
                        nextRail = direction.nextRail,
                        previousRail = direction.previousRail
                    });
                }

                history.dirList.Add(trainDirections);
            }

            historyList.Add(history);
        }
    }


    private void CheckIsRailMustSet(Rail lastTouchedRail)
    {
        if (!activeRail) return;

        if (Mathf.Abs(lastTouchedRail.Holder.GridPosition.x - activeRail.Holder.GridPosition.x) >= 2 &&
            Mathf.Abs(lastTouchedRail.Holder.GridPosition.x - activeRail.Holder.GridPosition.x) < 3)
        {
            if (lastTouchedRail.Holder.GridPosition.x > activeRail.Holder.GridPosition.x)
            {
                var holder = _levelController.gridManager.FindHolderByGridPos(new Vector2Int(
                    lastTouchedRail.Holder.GridPosition.x - 1, lastTouchedRail.Holder.GridPosition.y));

                if (holder.CurrentItem is not Rail _rail)
                    return;

                SetRail(_rail, holder);

            }
            else if (lastTouchedRail.Holder.GridPosition.x < activeRail.Holder.GridPosition.x)
            {
                var holder = _levelController.gridManager.FindHolderByGridPos(new Vector2Int(
                    lastTouchedRail.Holder.GridPosition.x + 1, lastTouchedRail.Holder.GridPosition.y));

                if (holder.CurrentItem is not Rail _rail)
                    return;

                SetRail(_rail, holder);

            }
        }

        if (Mathf.Abs(lastTouchedRail.Holder.GridPosition.y - activeRail.Holder.GridPosition.y) >= 2 &&
            Mathf.Abs(lastTouchedRail.Holder.GridPosition.y - activeRail.Holder.GridPosition.y) < 3)
        {
            if (lastTouchedRail.Holder.GridPosition.y > activeRail.Holder.GridPosition.y)
            {
                var holder = _levelController.gridManager.FindHolderByGridPos(new Vector2Int(
                    lastTouchedRail.Holder.GridPosition.x, lastTouchedRail.Holder.GridPosition.y - 1));

                if (holder.CurrentItem is not Rail _rail)
                    return;

                SetRail(_rail, holder);

            }
            else if (lastTouchedRail.Holder.GridPosition.y < activeRail.Holder.GridPosition.y)
            {
                var holder = _levelController.gridManager.FindHolderByGridPos(new Vector2Int(
                    lastTouchedRail.Holder.GridPosition.x, lastTouchedRail.Holder.GridPosition.y + 1));

                if (holder.CurrentItem is not Rail _rail)
                    return;

                SetRail(_rail, holder);
            }
        }
    }

    private void OnTouchMove(Vector2 touch)
    {
        var ray = _mainCamera.ScreenPointToRay(touch);

        if (Physics.Raycast(ray, out var hit, 500f, itemHolderLayer))
        {
            if (hit.transform.TryGetComponent(out ItemHolder holder))
            {
                var currentItem = holder.CurrentItem;

                if (currentItem is not Rail rail)
                    return;


                if (!rail.IsSetted())
                {
                    CheckIsRailMustSet(rail);
                }

                SetRail(rail, holder);

                foreach (var train in GameManager.Instance.levelManager.currentLevel.trains)
                {
                    var endRails = GameManager.Instance.levelManager.currentLevel.endRails
                        .Where(x => x.targetTrain == train.type)
                        .ToList();

                    foreach (var endRail in endRails)
                    {
                        if (!train.PathComplete)
                            train.PathComplete = CheckRail(endRail, endRail.Holder);
                    }
                }

                if (!GameManager.Instance.levelManager.currentLevel.trains.All(x => x.PathComplete))
                    return;

                LogManager.LogWarning("Rails Completed !", this);

                GameManager.Instance.touchManager.OnTouchMoveScreen -= OnTouchMove;
                TrainAction();
            }
        }
    }

    private bool SetRail(Rail rail, ItemHolder holder)
    {
        if (!rail.IsSetted())
            return CheckRail(rail, holder);

        CheckIntersection(rail);

        return false;
    }

    private TrainType activeTrainType;
    private Rail activeRail;

    private bool CheckRail(Rail rail, ItemHolder holder)
    {
        var availableHolders =
            GameManager.Instance.levelManager.currentLevel.gridManager.GetNeighbours(holder.GridPosition);

        foreach (var itemHolder in availableHolders)
        {
            var currentItem = itemHolder.CurrentItem;

            if (currentItem is not Rail currentRail)
                continue;

            if (!currentRail.IsSetted())
                continue;

            if (rail.targetTrain != TrainType.None && currentRail.targetTrain != TrainType.None)
            {
                if (rail.targetTrain != currentRail.targetTrain)
                    continue;
            }

            if (currentRail.GetPreviousRail(currentRail.targetTrain) == null)
                continue;

            if (currentRail.directonList.Count > 1)
            {
                foreach (var trainDirection in currentRail.directonList)
                {
                    if (currentRail.GetNextRail(trainDirection.targetTrain) == null)
                    {
                        activeTrainType = trainDirection.targetTrain;
                        rail.targetTrain = trainDirection.targetTrain;
                        rail.SetPreviousRail(rail.targetTrain, currentRail);
                        currentRail.SetNextRail(rail.targetTrain, rail);


                        rail.Initialize();
                        rail.UpdateModel();
                        SetIndicatorPosition(rail.transform.position);

                        currentRail.UpdateModel();
                        activeRail = rail;
                        foreach (var item in availableHolders)
                        {
                            if (item.CurrentItem is CollectableItem)
                            {
                                item.CurrentItem.SetMaterialForCollect();
                                (item.CurrentItem as CollectableItem).SetCollectable();
                                var spriteRen = item.indicator.GetComponent<SpriteRenderer>();
                                spriteRen.color = new Color(0.294f, 1f, 0.0627f, 1f);
                            }
                        }

                        return true;
                    }
                }
            }
            else
            {
                if (currentRail.GetNextRail(currentRail.targetTrain) == null)
                {
                    activeTrainType = currentRail.targetTrain;
                    rail.targetTrain = currentRail.targetTrain;
                    rail.SetPreviousRail(rail.targetTrain, currentRail);
                    currentRail.SetNextRail(rail.targetTrain, rail);


                    rail.Initialize();
                    rail.UpdateModel();
                    SetIndicatorPosition(rail.transform.position);

                    currentRail.UpdateModel();
                    activeRail = rail;
                    foreach (var item in availableHolders)
                    {
                        if (item.CurrentItem is CollectableItem)
                        {
                            item.CurrentItem.SetMaterialForCollect();
                            (item.CurrentItem as CollectableItem).SetCollectable();
                            var spriteRen = item.indicator.GetComponent<SpriteRenderer>();
                            spriteRen.color = new Color(0.294f, 1f, 0.0627f, 1f);
                        }
                    }

                    return true;
                }
            }
        }

        return false;
    }

    public void RewindRails()
    {
        var holderList = _levelController.gridManager.Grids.Where(x => x.CurrentItem is Rail).ToList();
        var railList = holderList.Select(x => x.CurrentItem as Rail).ToList();

        if (historyList.Count < 2)
        {
            return;
        }

        indicator.transform.position = historyList[^2].indicatorPosition;

        for (var i = 0; i < railList.Count; i++)
        {
            var trainDirections = new List<TrainDirection>();
            foreach (var direction in historyList[^2].dirList[i].dirList)
            {
                trainDirections.Add(new TrainDirection
                {
                    targetTrain = direction.targetTrain,
                    nextRail = direction.nextRail,
                    previousRail = direction.previousRail
                });
            }

            railList[i].directonList = trainDirections;
            railList[i].Undo();
        }

        if (historyList.Count > 1)
        {
            historyList.RemoveAt(historyList.Count - 1);
        }
    }

    public void CheckIntersection(Rail touchedRail)
    {
        if (touchedRail.targetTrain == TrainType.None || touchedRail.targetTrain == activeTrainType)
        {
            return;
        }

        var availableHolders =
            GameManager.Instance.levelManager.currentLevel.gridManager.GetNeighbours(touchedRail.Holder.GridPosition);

        var RailHolders = availableHolders.Where(x => x.CurrentItem is Rail).ToList();

        var anotherHolder = RailHolders.FirstOrDefault(x =>
            ((x.CurrentItem as Rail).targetTrain != TrainType.None &&
             (x.CurrentItem as Rail).targetTrain != touchedRail.targetTrain) &&
            !(x.CurrentItem as Rail).GetNextRail(touchedRail.targetTrain));


        if (anotherHolder != null)
        {
            var anotherRail = anotherHolder.CurrentItem as Rail;
            if (touchedRail.type != RailType.Straight || anotherRail.type != RailType.Straight)
            {
                return;
            }

            if (touchedRail.direction is RailDirection.Forward or RailDirection.Back &&
                anotherRail.direction is RailDirection.Forward or RailDirection.Back)
            {
                return;
            }

            if (touchedRail.direction is RailDirection.Right or RailDirection.Left &&
                anotherRail.direction is RailDirection.Right or RailDirection.Left)
            {
                return;
            }

            // if (!touchedRail.GetNextRail(touchedRail.targetTrain))
            // {
            //     Debug.Log(touchedRail.Holder.GridPosition);
            //     return;
            // }

            if (anotherRail.Holder.GridPosition.y == touchedRail.Holder.GridPosition.y &&
                touchedRail.Holder.GridPosition.x > anotherRail.Holder.GridPosition.x &&
                !GameManager.Instance.levelManager.currentLevel.gridManager.CheckPlaceableByPosition(
                    new Vector2Int(touchedRail.Holder.GridPosition.x + 1, touchedRail.Holder.GridPosition.y)))
            {
                return;
            }

            if (anotherRail.Holder.GridPosition.x == touchedRail.Holder.GridPosition.x &&
                anotherRail.Holder.GridPosition.y > touchedRail.Holder.GridPosition.y &&
                !GameManager.Instance.levelManager.currentLevel.gridManager.CheckPlaceableByPosition(
                    new Vector2Int(touchedRail.Holder.GridPosition.x, touchedRail.Holder.GridPosition.y - 1)))
            {
                return;
            }

            if (anotherRail.Holder.GridPosition.x == touchedRail.Holder.GridPosition.x &&
                anotherRail.Holder.GridPosition.y < touchedRail.Holder.GridPosition.y &&
                !GameManager.Instance.levelManager.currentLevel.gridManager.CheckPlaceableByPosition(
                    new Vector2Int(touchedRail.Holder.GridPosition.x, touchedRail.Holder.GridPosition.y + 1)))
            {
                return;
            }

            if (anotherRail.Holder.GridPosition.y == touchedRail.Holder.GridPosition.y &&
                touchedRail.Holder.GridPosition.x < anotherRail.Holder.GridPosition.x &&
                !GameManager.Instance.levelManager.currentLevel.gridManager.CheckPlaceableByPosition(
                    new Vector2Int(touchedRail.Holder.GridPosition.x - 1, touchedRail.Holder.GridPosition.y)))
            {
                return;
            }

            touchedRail.SetPreviousRail(anotherRail.targetTrain, anotherRail);
            anotherRail.SetNextRail(anotherRail.targetTrain, touchedRail);
        }
    }

    private void TrainAction()
    {
        GameManager.Instance.uiManager.overlay.rewindButton.interactable = false;
        HapticManager.GenerateHaptic(PresetType.MediumImpact);
        indicator.gameObject.SetActive(false);
        foreach (var trainObject in GameManager.Instance.levelManager.currentLevel.trainList)
        {
            trainObject.isTrainMove = true;
        }

        if (_levelController.isTutorialLevel)
        {
            GameManager.Instance.uiManager.gamePlay.ResetTutoHand();
        }
    }

    public void SetIndicatorPosition(Vector3 targetPos)
    {
        indicator.ResetTime();
        indicator.SetlastPos();
        indicator.transform.position = targetPos;
    }
}

[Serializable]
public class RailHistory
{
    public List<TrainDirectionHistory> dirList;
    public Vector3 indicatorPosition;

    public RailHistory()
    {
        dirList = new List<TrainDirectionHistory>();
    }
}

[Serializable]
public class TrainDirectionHistory
{
    public List<TrainDirection> dirList;

    public TrainDirectionHistory()
    {
        dirList = new List<TrainDirection>();
    }
}