using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public static class MyExtensions
{
    public static T GetComponent<T>(this GameObject gameObject) where T : Component
    {
        T toGet = gameObject.GetComponent<T>();
        if (toGet != null)
        {
            return toGet;
        }
        else
        {
            throw new Exception($"Missing Component {typeof(T).FullName} Exception On {gameObject.name} ! ");
        }
    }

    public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(InternalOperation());

        IEnumerator InternalOperation()
        {
            yield return new WaitForSeconds(time);

            action?.Invoke();
        }
    }
    
    public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, float count = -1)
    {
        return monoBehaviour.StartCoroutine(InternalOperation());

        IEnumerator InternalOperation()
        {
            while (monoBehaviour.enabled)
            {
                yield return new WaitForSeconds(time);
                
                action?.Invoke();

                if (count-- == 0)
                    yield break;
            }
        }
    }
    
    public static IEnumerator DoBlendShapeWeightCoroutine(this SkinnedMeshRenderer meshRenderer,int index , float target, float time)
    {
        float currentTime = 0;
        var startValue = meshRenderer.GetBlendShapeWeight(index);

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            meshRenderer.SetBlendShapeWeight(index, Mathf.Lerp(startValue, target, currentTime / time));

            yield return null;
        }
    }
    
    public static void DoLayersWeight(this MonoBehaviour monoBehaviour, Animator animator, float targetWeight, float time)
    {
        monoBehaviour.StartCoroutine(SetLayersWeightInTime(animator, targetWeight, time));
    }

    private static IEnumerator SetLayersWeightInTime(Animator animator, float targetWeight, float time)
    {
        float currentTime = 0;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            animator.SetLayerWeight(1, Mathf.Lerp(0, targetWeight, currentTime / time));

            yield return null;
        }
    }
    
    public static Vector3 SamplePosition(this Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out var hit, 2.0f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            Debug.LogWarning("UnReachable Position Created On NavMesh");
        }

        return position;
    }

    public static int GetRandom(this Vector2Int vector2Int)
    {
        return Random.Range(vector2Int.x, vector2Int.y);
    }
    
    public static Transform SampleTransformPosition(this Transform transform)
    {
        if (NavMesh.SamplePosition(transform.position, out var hit, 2.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("UnReachable Position Created On NavMesh");
        }

        return transform;
    }
}