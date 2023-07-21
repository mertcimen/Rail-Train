using UnityEngine;
public class Vector3Utils
{
    public static bool Approximately(Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;
        if (Mathf.Abs(dz) > allowedDifference)
            return false;

        return true;
    }
    public static Vector3 Add(params Vector3[] vectors)
    {
        Vector3 newVector = new Vector3(0, 0, 0);
        foreach (Vector3 vector in vectors)
        {
            float x = newVector.x + vector.x;
            float y = newVector.y + vector.y;
            float z = newVector.z + vector.z;
            newVector = new Vector3(x, y, z);
        }
        return newVector;
    }

    public static Vector3 Substract(params Vector3[] vectors)
    {
        Vector3 newVector = new Vector3(0, 0, 0);
        foreach (Vector3 vector in vectors)
        {
            float x = newVector.x - vector.x;
            float y = newVector.y - vector.y;
            float z = newVector.z - vector.z;
            newVector = new Vector3(x, y, z);
        }
        return newVector;
    }

    public static Vector3 Multiply(params Vector3[] vectors)
    {
        Vector3 newVector = new Vector3(1, 1, 1);
        foreach (Vector3 vector in vectors)
        {
            float x = newVector.x * vector.x;
            float y = newVector.y * vector.y;
            float z = newVector.z * vector.z;
            newVector = new Vector3(x, y, z);
        }
        return newVector;
    }
}