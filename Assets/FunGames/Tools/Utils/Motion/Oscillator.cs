using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2F;

    public bool local = false;


    float movementFactor; Vector3 startingPos;
    // Use this for initialization
    void Start()
    {
        startingPos = local ? transform.localPosition : transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // set the movement factor
        //Todo protect against Naan
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to + 1
        movementFactor = rawSinWave = rawSinWave / 2f + 0.5f;


        Vector3 offset = movementVector * movementFactor;
        if (!local)
        {
            transform.position = startingPos + offset;
        }
        else
        {
            transform.localPosition = startingPos + offset;
        }
    }
}
