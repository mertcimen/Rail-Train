using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemHolderData", order = 1)]
public class ItemHolderScriptable : ScriptableObject
{
   
    public List<ItemBase> HolderObjects = new List<ItemBase>();

    
}
