using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public List<PosHolderByType> holderList = new List<PosHolderByType>();
    public bool isPassengerWagon;

    public void Load(CollectableItem item)
    {
        var holder2 = holderList.Find(x => x.ItemType == item.collectableItemType);
        Spawn(holder2, item);
    }

    public void Spawn(PosHolderByType holder, CollectableItem collectableItem)
    {
        StartCoroutine(SpawnDelay());

        IEnumerator SpawnDelay()
        {
            
            // for (int i = 0; i < collectableItem.itemAmount; i++)
            // {
            //     var model = Instantiate(holder.itemModel, collectableItem.transform.position,
            //         holder.posList[i].rotation,
            //         this.transform);
            //     collectableItem.transform.DOScale(Vector3.zero, 0.3f);
            //     model.transform.DOJump(holder.posList[i].position, 3f, 1, 0.3f);
            //     model.transform.localScale = Vector3.zero;
            //     model.transform.DOScale(Vector3.one * 1f, .5f).SetEase(Ease.OutBack);
            //     HapticManager.GenerateHaptic(PresetType.LightImpact);
            //     yield return new WaitForSeconds(0.2f);
            // }
            foreach (var transform1 in holder.posList)
            {
                var model = Instantiate(holder.itemModel, collectableItem.transform.position, transform1.rotation,
                    this.transform);
                collectableItem.transform.DOScale(Vector3.zero, 0.3f);
                model.transform.DOJump(transform1.position, 3f, 1, 0.1f);
                model.transform.localScale = Vector3.zero;
                model.transform.DOScale(Vector3.one * 1f, .5f).SetEase(Ease.OutBack);
                HapticManager.GenerateHaptic(PresetType.LightImpact);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

[Serializable]
public class PosHolderByType
{
    public CollectableType ItemType;
    public List<Transform> posList = new List<Transform>();
    public GameObject itemModel;
}