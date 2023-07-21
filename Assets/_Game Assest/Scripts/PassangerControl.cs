using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassangerControl : MonoBehaviour
{
    public List<GameObject> graphList;


    private void OnEnable()
    {
        var random = Random.Range(0, graphList.Count);
        foreach (var graph in graphList)
        {
            graph.SetActive(false);
        }

        graphList[random].SetActive(true);
    }
}