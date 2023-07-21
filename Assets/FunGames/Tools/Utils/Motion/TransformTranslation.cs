using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTranslation : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float speed = 1F;

    public bool local = false;


    float movementFactor; Vector3 startingPos; Vector3 movemenFactor; Vector3 offset;
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
        // if (period <= Mathf.Epsilon) { return; }
        // float cycles = Time.time / period; // grows continually from 0
        // const float tau = Mathf.PI * 2; // about 6.28
        // float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to + 1
        // movementFactor = rawSinWave = rawSinWave / 2f + 0.5f;

        // movementFactor = speed;
        movemenFactor = movementVector * speed * 0.01f;
        offset = Vector3Utils.Add(movemenFactor, offset);


        if (Mathf.Abs(offset.x) >= Mathf.Abs(movementVector.x) && Mathf.Abs(offset.y) >= Mathf.Abs(movementVector.y) && Mathf.Abs(offset.z) >= Mathf.Abs(movementVector.z))
        {
            offset = Vector3.zero;
        }

        if (!local)
        {
            transform.position = startingPos + offset;
        }
        else
        {
            transform.localPosition = startingPos + offset;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 destination = Vector3.zero;
        if (!Application.isPlaying)
        {
            destination = Vector3Utils.Add(movementVector, transform.position);
            Gizmos.DrawLine(transform.position, destination);
        }
    }

}
