using UnityEngine;

/// <summary>
/// Mono singleton Class. Extend this class to make singleton component.
/// Example: 
/// <code>
/// public class Foo : MonoSingleton<Foo>
/// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
/// Override <code>Init()</code> method instead of using <code>Awake()</code>
/// from this class.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _mInstance = null;

    public static T Instance
    {
        get
        {
            // Instance required for the first time, we look for it
            if (_mInstance != null)
                return _mInstance;

            _mInstance = FindObjectOfType(typeof(T)) as T;

            // Object not found, we create a temporary one
            if (_mInstance == null)
            {
                Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                isTemporaryInstance = true;
                _mInstance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T))
                    .GetComponent<T>();

                // Problem during the creation, this should not happen
                if (_mInstance == null)
                {
                    Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                }
            }

            return _mInstance;
        }
    }

    public static bool isTemporaryInstance { private set; get; }

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (_mInstance == null)
        {
            _mInstance = this as T;
        }
        else if (_mInstance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(gameObject);
            return;
        }

        if (_mInstance)
            _mInstance.Init();
    }


    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// This function is called when the instance is used the first time
    /// Put all the initializations you need here, as you would do in Awake
    /// </summary>
    protected virtual void Init()
    {
        
    }

    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
        _mInstance = null;
    }
}