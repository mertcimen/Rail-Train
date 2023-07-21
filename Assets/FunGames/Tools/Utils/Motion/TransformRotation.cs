using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotation : MonoBehaviour
{

    public float RotationSpeed;
    public bool x;
    public bool y;
    public bool z;


    // Update is called once per frame
    void Update()
    {
        int intX = x ? 1 : 0;
        int intY = y ? 1 : 0;
        int intZ = z ? 1 : 0;

        Vector3 RotationVector = new Vector3(intX, intY, intZ);
        transform.Rotate(RotationVector, RotationSpeed * Time.deltaTime);
    }
}
