using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PathCreation;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Rail : ItemBase
{
    public TrainType targetTrain;
    public RailType type;
    public RailDirection direction;
    public PositionType position;

    [ReadOnly] public ItemHolder Holder;

    [ReadOnly] public GameObject railObject;
    [ReadOnly] public PathCreator pathCreator;

    [ReadOnly] public List<TrainDirection> directonList;

    public ParticleSystem spawnParticle;

    private const int AdditionalRailLenght = 4;

    public bool _isSetted => directonList.Count > 0;


    // public bool isTrapTunnel;
    public void Initialize()
    {
        if (directonList == null)
        {
            directonList = new List<TrainDirection>();
        }

        if (Holder is null)
        {
            var holders = GetComponentsInParent<ItemHolder>();
            Holder = holders[0];
        }


        if (position == PositionType.None)
            return;

        Rail lastRail = null;

        for (var i = 0; i < (position == PositionType.End ? AdditionalRailLenght : 3); i++)
        {
            var gridManager = GameManager.Instance.levelManager.currentLevel.gridManager;
            var decoyRail = Instantiate(GameManager.Instance.levelManager.currentLevel.railObject, transform);

            decoyRail.targetTrain = targetTrain;
            decoyRail.type = type;
            decoyRail.direction = direction;
            decoyRail.position = PositionType.None;

            if (position == PositionType.End)
            {
                decoyRail.transform.position += direction switch
                {
                    RailDirection.None => Vector3.zero,
                    RailDirection.Right => Vector3.right * gridManager.xOffset * (i + 1),
                    RailDirection.Left => Vector3.left * gridManager.xOffset * (i + 1),
                    RailDirection.Forward => Vector3.forward * gridManager.zOffset * (i + 1),
                    RailDirection.Back => Vector3.back * gridManager.zOffset * (i + 1),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                decoyRail.transform.position += direction switch
                {
                    RailDirection.None => Vector3.zero,
                    RailDirection.Right => Vector3.right * gridManager.xOffset * -(i + 1),
                    RailDirection.Left => Vector3.left * gridManager.xOffset * -(i + 1),
                    RailDirection.Forward => Vector3.forward * gridManager.zOffset * -(i + 1),
                    RailDirection.Back => Vector3.back * gridManager.zOffset * -(i + 1),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (i == 0)
            {
                if (position == PositionType.End)
                {
                    SetNextRail(targetTrain, decoyRail);
                    decoyRail.SetPreviousRail(targetTrain, this);
                }
                else
                {
                    SetPreviousRail(targetTrain, decoyRail);
                    decoyRail.SetNextRail(targetTrain, this);
                }
            }
            else
            {
                if (position == PositionType.End)
                {
                    lastRail.SetNextRail(targetTrain, decoyRail);
                    decoyRail.SetPreviousRail(targetTrain, lastRail);
                }
                else
                {
                    lastRail.SetPreviousRail(targetTrain, decoyRail);
                    decoyRail.SetNextRail(targetTrain, lastRail);
                }
            }

            var decoyHolder = new GameObject("Decoy Holder").AddComponent<ItemHolder>();
            decoyHolder.transform.SetParent(decoyRail.transform);
            decoyHolder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            switch (position)
            {
                case PositionType.Start:
                    decoyHolder.GridPosition = direction switch
                    {
                        RailDirection.None => Vector2Int.zero,
                        RailDirection.Right => Vector2Int.left * (i + 1),
                        RailDirection.Left => Vector2Int.right * (i + 1),
                        RailDirection.Forward => Vector2Int.up * (i + 1),
                        RailDirection.Back => Vector2Int.down * (i + 1),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    decoyHolder.GridPosition += Holder.GridPosition;
                    break;
                case PositionType.End:
                    decoyHolder.GridPosition = direction switch
                    {
                        RailDirection.None => Vector2Int.zero,
                        RailDirection.Right => Vector2Int.right * (i + 1),
                        RailDirection.Left => Vector2Int.left * (i + 1),
                        RailDirection.Forward => Vector2Int.down * (i + 1),
                        RailDirection.Back => Vector2Int.up * (i + 1),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    decoyHolder.GridPosition += Holder.GridPosition;
                    break;
                case PositionType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            decoyRail.Holder = decoyHolder;
            decoyHolder.CurrentItem = decoyRail;

            lastRail = decoyRail;

            lastRail.Initialize();
            lastRail.UpdateModel();
        }

        lastRail.position = position;
        position = PositionType.None;

        UpdateModel();
    }

    private Rail previousRail => GetPreviousRail(targetTrain);
    private Rail nextRail => GetNextRail(targetTrain);

    public void Undo()
    {
        UpdateModel();
    }


    public void SetPreviousRail(TrainType trainType, Rail rail)
    {
        var trainDirection = directonList.FirstOrDefault(x => x.targetTrain == trainType);
        if (trainDirection != null)
        {
            trainDirection.previousRail = rail;
        }
        else
        {
            directonList.Add(new TrainDirection()
            {
                targetTrain = trainType,
                previousRail = rail
            });
        }
    }

    public void SetNextRail(TrainType trainType, Rail rail)
    {
        var trainDirection = directonList.FirstOrDefault(x => x.targetTrain == trainType);
        if (trainDirection != null)
        {
            trainDirection.nextRail = rail;
        }
        else
        {
            directonList.Add(new TrainDirection()
            {
                targetTrain = trainType,
                nextRail = rail
            });
        }

        if (rail == null && trainDirection != null)
        {
            directonList.Remove(trainDirection);
        }
    }

    public Rail GetPreviousRail(TrainType trainType)
    {
        var trainDirection = directonList.FirstOrDefault(x => x.targetTrain == trainType);
        return trainDirection?.previousRail;
    }

    public Rail GetNextRail(TrainType trainType)
    {
        var trainDirection = directonList.FirstOrDefault(x => x.targetTrain == trainType);
        return trainDirection?.nextRail;
    }


    /// <summary>
    /// Getchild I Intersection Scriptine Bağla!!!
    /// </summary>
    /// <param name="targetTrain"></param>
    /// <returns></returns>
    public PathCreator GetPathCreator(TrainType targetTrain)
    {
        if (type == RailType.Intersection || type == RailType.IntersectionReverse)
        {
            if (targetTrain == TrainType.First)
            {
                return transform.GetChild(0).GetChild(1).GetComponent<PathCreator>();
            }
            else
            {
                return transform.GetChild(0).GetChild(2).GetComponent<PathCreator>();
            }

            var pathCreators = GetComponentsInChildren<PathCreator>();
            return type == RailType.Intersection ? pathCreators[0] : pathCreators[1];
        }
        else
        {
            return pathCreator;
        }
    }

    public bool IsSetted()
    {
        return _isSetted;
    }

    [Button]
    public void UpdateModel()
    {
        // if (directionHistory.Count > 0 && directionHistory[^1] != directonList)
        // {
        //     directionHistory.Add(directonList);
        // }
        // else if (directionHistory.Count < 1)
        // {
        // }

        var lastDirection = direction;
        var lastType = type;

        CalculateRail();

        if (railObject)
            Destroy(railObject);

        if (type == RailType.None || direction == RailDirection.None)
            return;

        var levelController = GameManager.Instance.levelManager.currentLevel;
        var trainObject = levelController.trains.FirstOrDefault(x => x.type == targetTrain);

        if (trainObject is null)
        {
            LogManager.LogError("Missing Train Data !", this);

            return;
        }

        var rail = type switch
        {
            RailType.None => null,
            RailType.Straight => trainObject.straightObject,
            RailType.RightCorner => trainObject.rightCornerObject,
            RailType.LeftCorner => trainObject.leftCornerObject,
            RailType.Intersection => trainObject.intersectionObject,
            RailType.IntersectionReverse => trainObject.intersectionReverseObject,
            _ => throw new ArgumentOutOfRangeException()
        };

        railObject = Instantiate(rail, transform.position, quaternion.identity, transform);
        pathCreator = railObject.GetComponent<PathCreator>();


        railObject.transform.localRotation = direction switch
        {
            RailDirection.None => Quaternion.identity,
            RailDirection.Right => Quaternion.Euler(0, 90f, 0),
            RailDirection.Left => Quaternion.Euler(0, -90f, 0),
            RailDirection.Forward => Quaternion.identity,
            RailDirection.Back => Quaternion.Euler(0, 180f, 0),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (lastDirection == direction && lastType == type)
            return;

        railObject.transform.localScale = Vector3.zero;
        railObject.transform.DOScale(1f, .2f).SetEase(Ease.OutBack);
        var particle = Instantiate(spawnParticle, transform.position, quaternion.identity);
        particle.Play();
        HapticManager.GenerateHaptic(PresetType.MediumImpact);
        GameManager.Instance.levelManager.currentLevel.touchController.canHistoryRecord = true;
        Destroy(particle, 5f);
    }

    [Button]
    public void CalculateRail()
    {
        directonList.RemoveAll(x => x.previousRail == null && x.nextRail == null);

        if (directonList.Count > 1)
        {
            if (GetPreviousRail(TrainType.First).Holder.GridPosition.x <
                GetNextRail(TrainType.First).Holder.GridPosition.x)
            {
                direction = RailDirection.Back;
                if (GetPreviousRail(TrainType.Second).Holder.GridPosition.y >
                    GetNextRail(TrainType.Second).Holder.GridPosition.y)
                {
                    type = RailType.Intersection;
                }
                else
                {
                    type = RailType.IntersectionReverse;
                }
            }

            else if (GetPreviousRail(TrainType.First).Holder.GridPosition.x >
                     GetNextRail(TrainType.First).Holder.GridPosition.x)
            {
                direction = RailDirection.Forward;
                if (GetPreviousRail(TrainType.Second).Holder.GridPosition.y >
                    GetNextRail(TrainType.Second).Holder.GridPosition.y)
                {
                    type = RailType.IntersectionReverse;
                }
                else
                {
                    type = RailType.Intersection;
                }
            }
            else if (GetPreviousRail(TrainType.First).Holder.GridPosition.y >
                     GetNextRail(TrainType.First).Holder.GridPosition.y)
            {
                direction = RailDirection.Right;
                if (GetPreviousRail(TrainType.Second).Holder.GridPosition.x >
                    GetNextRail(TrainType.Second).Holder.GridPosition.x)
                {
                    type = RailType.Intersection;
                }
                else
                {
                    type = RailType.IntersectionReverse;
                }
            }
            else if (GetPreviousRail(TrainType.First).Holder.GridPosition.y <
                     GetNextRail(TrainType.First).Holder.GridPosition.y)
            {
                direction = RailDirection.Left;
                if (GetPreviousRail(TrainType.Second).Holder.GridPosition.x >
                    GetNextRail(TrainType.Second).Holder.GridPosition.x)
                {
                    type = RailType.IntersectionReverse;
                }
                else
                {
                    type = RailType.Intersection;
                }
            }

            return;
        }

        if (previousRail == null && nextRail == null)
        {
            targetTrain = TrainType.None;
            type = RailType.None;
            direction = RailDirection.None;
        }

        if (previousRail == null)
        {
            return;
        }

        if (nextRail == null)
        {
            type = RailType.Straight;

            if (Holder.GridPosition.x > previousRail.Holder.GridPosition.x) //Previous Rail is on Left
            {
                direction = RailDirection.Right;
            }
            else if (Holder.GridPosition.x < previousRail.Holder.GridPosition.x) //Previous Rail is on Right
            {
                direction = RailDirection.Left;
            }
            else if (Holder.GridPosition.y > previousRail.Holder.GridPosition.y) //Previous Rail is on Forward
            {
                direction = RailDirection.Back;
            }
            else if (Holder.GridPosition.y < previousRail.Holder.GridPosition.y) //Previous Rail is on Back
            {
                direction = RailDirection.Forward;
            }
        }
        else
        {
            type = RailType.Straight;
            if (Holder.GridPosition.x > previousRail.Holder.GridPosition.x) //Previous Rail is on Left
            {
                if (Holder.GridPosition.x < nextRail.Holder.GridPosition.x) //Next Rail is on Right
                {
                    direction = RailDirection.Right;
                }
                else if (Holder.GridPosition.y > nextRail.Holder.GridPosition.y) //Next Rail is on Forward
                {
                    type = RailType.LeftCorner;
                    direction = RailDirection.Forward;
                }
                else if (Holder.GridPosition.y < nextRail.Holder.GridPosition.y) //Next Rail is on Back
                {
                    type = RailType.RightCorner;
                    direction = RailDirection.Back;
                }
            }
            else if (Holder.GridPosition.x < previousRail.Holder.GridPosition.x) //Previous Rail is on Right
            {
                if (Holder.GridPosition.x > nextRail.Holder.GridPosition.x) //Next Rail is on Left
                {
                    direction = RailDirection.Left;
                }
                else if (Holder.GridPosition.y > nextRail.Holder.GridPosition.y) //Next Rail is on Forward
                {
                    type = RailType.RightCorner;
                    direction = RailDirection.Forward;
                }
                else if (Holder.GridPosition.y < nextRail.Holder.GridPosition.y) //Next Rail is on Back
                {
                    type = RailType.LeftCorner;
                    direction = RailDirection.Back;
                }
            }
            else if (Holder.GridPosition.y > previousRail.Holder.GridPosition.y) //Previous Rail is on Forward
            {
                if (Holder.GridPosition.x > nextRail.Holder.GridPosition.x) //Next Rail is on Left
                {
                    type = RailType.RightCorner;
                    direction = RailDirection.Left;
                }
                else if (Holder.GridPosition.x < nextRail.Holder.GridPosition.x) //Next Rail is on Right
                {
                    type = RailType.LeftCorner;
                    direction = RailDirection.Right;
                }
                else if (Holder.GridPosition.y < nextRail.Holder.GridPosition.y) //Next Rail is on Back
                {
                    direction = RailDirection.Back;
                }
            }
            else if (Holder.GridPosition.y < previousRail.Holder.GridPosition.y) //Previous Rail is on Back
            {
                if (Holder.GridPosition.x > nextRail.Holder.GridPosition.x) //Next Rail is on Left
                {
                    type = RailType.LeftCorner;
                    direction = RailDirection.Left;
                }
                else if (Holder.GridPosition.x < nextRail.Holder.GridPosition.x) //Next Rail is on Right
                {
                    type = RailType.RightCorner;
                    direction = RailDirection.Right;
                }
                else if (Holder.GridPosition.y > nextRail.Holder.GridPosition.y) //Next Rail is on Forward
                {
                    direction = RailDirection.Forward;
                }
            }
        }
    }
}

[Serializable]
public class TrainDirection
{
    public TrainType targetTrain;
    public Rail previousRail;
    public Rail nextRail;
}

public enum RailType
{
    None = 0,
    Straight = 1,
    RightCorner = 2,
    LeftCorner = 3,
    Intersection = 4,
    IntersectionReverse = 5,
}

public enum RailDirection
{
    None = 0,
    Right = 1,
    Left = 2,
    Forward = 3,
    Back = 4
}

public enum PositionType
{
    None = 0,
    Start = 1,
    End = 2
}