using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Train : MonoBehaviour
{
    public TrainType type;

    [AssetsOnly] public GameObject straightObject;
    [AssetsOnly] public GameObject rightCornerObject;
    [AssetsOnly] public GameObject leftCornerObject;
    [AssetsOnly] public GameObject intersectionObject;
    [AssetsOnly] public GameObject intersectionReverseObject;

    [AssetsOnly, SerializeField] private Locomotive locomotivePrefab;
    public Locomotive locomotiveObj;
    [AssetsOnly, SerializeField] private Wagon vagonObj;
    [AssetsOnly, SerializeField] private Wagon passengerVagonObj;
    [SerializeField] private float speed = 5;

    [SerializeField] private int wagonCount = 1;

    [SerializeField] private int passengerWagonCount = 1;

    [SerializeField] private float firstVagonDistance;
    [SerializeField] private float nextVagonDistance;
    private float vagonDistance;

    [ReadOnly] public Rail startRail;
  

    private bool _isCrasHed;
    public bool PathComplete { get; set; } = false;

    public bool isTrainMove = true;

    private void Start()
    {
        PathComplete = false;
    }

    public ParticleSystem crashParticle;


    public void SetWagonCounts(int _wagonCount, int _passengerWagonCount)
    {
        wagonCount = _wagonCount;
        passengerWagonCount = _passengerWagonCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveableObj"))
        {
            var train = other.GetComponentInParent<Train>();
            if (train.GetCrushState())
            {
                return;
            }

            if (train.type != type)
            {
                var particle = Instantiate(crashParticle);
                particle.transform.position = other.transform.position;

                train.isTrainMove = false;
                isTrainMove = false;
                train.SetCrashState(true);
                SetCrashState(true);
                HapticManager.GenerateHaptic(PresetType.Failure);
                // var cam = Camera.main.transform.DOShakePosition(0.8f, Vector3.one * 1.2f, 3, 90f, true);
                var cam = Camera.main.transform.DOShakeRotation(0.8f, Vector3.one, 10, 90f, true);

                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    for (int i = 0; i < 15; i++)
                    {
                        HapticManager.GenerateHapticWithInterval(PresetType.HeavyImpact, 0.1f);
                        yield return new WaitForSeconds(0.05f);
                    }

                    GameManager.Instance.LevelFinish(false);
                }
            }
        }

        if (other.CompareTag("TrapTunnel"))
        {
            var particle = Instantiate(crashParticle);
            particle.transform.position = other.transform.position;

            isTrainMove = false;
            SetCrashState(true);
            HapticManager.GenerateHaptic(PresetType.Failure);
            // var cam = Camera.main.transform.DOShakePosition(0.8f, Vector3.one * 1.2f, 3, 90f, true);
            var cam = Camera.main.transform.DOShakeRotation(0.8f, Vector3.one, 10, 90f, true);

            StartCoroutine(Delay());

            IEnumerator Delay()
            {
                for (int i = 0; i < 60; i++)
                {
                    HapticManager.GenerateHapticWithInterval(PresetType.HeavyImpact, 0.1f);
                    yield return new WaitForSeconds(0.05f);
                }

                GameManager.Instance.LevelFinish(false);
            }
        }
        
        
    }

    public void SetCrashState(bool state)
    {
        _isCrasHed = state;
    }

    public bool GetCrushState()
    {
        return _isCrasHed;
    }


    public void Crash()
    {
        isTrainMove = false;
    }

    public List<MovingObject> movingObjects = new List<MovingObject>();

    public IEnumerator Move()
    {
        var locomotive = new MovingObject(this.locomotivePrefab.gameObject, startRail, this);
        locomotive.isLoaded = true;
        locomotiveObj = locomotive.model.GetComponent<Locomotive>();
        locomotiveObj.Initialize(this);
        movingObjects.Add(locomotive);
        vagonDistance = firstVagonDistance;

        var firstVagon = true;

        locomotive.addVagonAction = () =>
        {
            if (firstVagon)
            {
                vagonDistance = nextVagonDistance;
                firstVagon = false;
            }

            if (wagonCount > 0)
            {
                wagonCount--;

                var vagon = new MovingObject(vagonObj.gameObject, startRail, this);
                movingObjects.Add(vagon);
            }

            else if (passengerWagonCount > 0)
            {
                passengerWagonCount--;

                var vagon = new MovingObject(passengerVagonObj.gameObject, startRail, this);
                movingObjects.Add(vagon);
            }
            else
            {
                locomotive.addVagonAction = null;
            }
        };

        while (movingObjects.All(x => x.enabled))
        {
            yield return null;

            if (!isTrainMove)
            {
                continue;
            }

            for (var i = 0; i < movingObjects.Count; i++)
            {
                movingObjects[i].Move();
            }
        }
    }

    public class MovingObject
    {
        public bool isLoaded;
        public GameObject model;
        public Rail currentRail;
        public bool enabled = true;
        public float distance = 0;
        public Action addVagonAction;

        private Train train;
        private float totalDistance = 0;

        public MovingObject(GameObject modelObject, Rail startRail, Train train)
        {
            this.train = train;
            currentRail = startRail;
            distance = 0;

            var gameObject = Instantiate(modelObject, train.transform.position, train.transform.rotation,
                train.transform);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(1f, .2f).SetEase(Ease.OutBack);
            model = gameObject;
        }

        public void Move()
        {
            if (!enabled)
                return;

            distance += Time.deltaTime * train.speed;
            totalDistance += Time.deltaTime * train.speed;

            var path = currentRail.GetPathCreator(train.type).path;
            if (distance < path.length)
            {
                model.transform.position = path.GetPointAtDistance(distance);
                model.transform.rotation = path.GetRotationAtDistance(distance);
            }
            else
            {
                distance = distance - path.length + .001f;

                if (currentRail.GetNextRail(train.type) != null)
                {
                    if (currentRail.Holder.GetId() > 0 && !isLoaded)
                    {
                        var availableHolders =
                            GameManager.Instance.levelManager.currentLevel.gridManager.GetNeighbours(currentRail.Holder
                                .GridPosition);
                        foreach (var holder in availableHolders)
                        {
                            if (holder.CurrentItem is CollectableItem &&
                                !(((holder.CurrentItem) as CollectableItem).collectableItemType ==
                                  CollectableType.Passenger) &&
                                !(holder.CurrentItem as CollectableItem).isCollected)
                            {
                                var wagon = model.GetComponent<Wagon>();

                                if (!wagon.isPassengerWagon)
                                {
                                    ((CollectableItem)holder.CurrentItem).isCollected = true;
                                    train.isTrainMove = false;
                                    isLoaded = true;
                                    train.CollectSeq(wagon, holder);
                                }
                            }

                            if (holder.CurrentItem is CollectableItem &&
                                (((holder.CurrentItem) as CollectableItem).collectableItemType ==
                                 CollectableType.Passenger) &&
                                !(holder.CurrentItem as CollectableItem).isCollected)
                            {
                                var wagon = model.GetComponent<Wagon>();

                                if (wagon.isPassengerWagon)
                                {
                                    ((CollectableItem)holder.CurrentItem).isCollected = true;
                                    train.isTrainMove = false;
                                    isLoaded = true;
                                    train.CollectSeq(wagon, holder);
                                }
                            }
                        }
                    }

                    currentRail = currentRail.GetNextRail(train.type);
                }
                else
                {
                    train.CheckAllTasksDone();
                    train.isTrainMove = false;
                    enabled = false;
                }
            }

            if (totalDistance > train.vagonDistance)
            {
                addVagonAction?.Invoke();
                totalDistance = 0;
            }
        }
    }

    public void CollectSeq(Wagon wagon, ItemHolder holder)
    {
        wagon.Load((CollectableItem)holder.CurrentItem);
        // GameManager.Instance.levelManager.currentLevel.EliminateTask(collectable.collectableItemType, this);

        holder.indicator.SetActive(false);
        StartCoroutine(WaitForCollect());
        GameManager.Instance.uiManager.gamePlay.CheckCollectedIsATask((CollectableItem)holder.CurrentItem, type);

        IEnumerator WaitForCollect()
        {
            yield return new WaitForSeconds(0.9f);
            isTrainMove = true;
        }
    }

    public bool isTrainArrive;

    private void CheckAllTasksDone()
    {
        var doneCount = locomotiveObj.activeTasks.Count(x => x.isDone);
        if (locomotiveObj.activeTasks.Count == doneCount)
        {
            isTrainArrive = true;
            GameManager.Instance.levelManager.currentLevel.CheckLevelFinish();
        }
        else
        {
            GameManager.Instance.levelManager.currentLevel.CheckLevelFinish();
        }
    }
}

public enum TrainType
{
    None = 0,
    First = 1,
    Second = 2,
    Both = 3
}