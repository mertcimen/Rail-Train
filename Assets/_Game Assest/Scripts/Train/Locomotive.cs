using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Locomotive : MonoBehaviour
{
    public Train train;
    public LocomotiveTask taskObject;
    public GameObject taskGroup;
    public Canvas trainCanvas;
    public List<LocomotiveTask> activeTasks;

    public GameObject cloudTrail;

    public void Initialize(Train _train)
    {
        train = _train;
        UpdateTasks();
        ActivateTrail();
    }


    public void ActivateTrail()
    {
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2f);
            cloudTrail.SetActive(true);
        }
    }

    private void UpdateTasks()
    {
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.3f);
            var tasks = GameManager.Instance.levelManager.currentLevel.taskList.Where(x => x.traintype == train.type)
                .ToList();
            if (tasks.Count > 0)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    var task = Instantiate(taskObject, taskGroup.transform);
                    task.transform.DOScale(Vector3.one, 0.3f);
                    task.SetTaskInfo(tasks[i]);
                    activeTasks.Add(task);
                }
            }
            else
            {
                trainCanvas.gameObject.SetActive(false);
            }
        }
    }


    public void CheckCollectedIsATask(CollectableItem collectedItem)
    {
        var matchedTasks = activeTasks.Where(x => x.neededType == collectedItem.collectableItemType).ToList();
        if (matchedTasks.Count > 0)
        {
            matchedTasks[0].SetInfoAfterLoad(collectedItem);
            if (matchedTasks[0].neededAmount == matchedTasks[0].collectedAmount)
            {
                matchedTasks[0].isDone = true;
               // matchedTasks[0].taskIcon.
                matchedTasks[0].taskText.color = Color.green;
            }
        }
    }
}