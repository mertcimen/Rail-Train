using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] private List<PoolObject> poolObjects;

    private static Transform _transform;

    public ObjectPoolingManager Initialize()
    {
        _transform = transform;
        return this;
    }

    public T GetItem<T>()
    {
        return (T)poolObjects.FirstOrDefault(x => x.ObjectType == typeof(T))?.Pool.Get();
    }

    public void Release<T>(IPoolable poolable)
    {
        poolObjects.FirstOrDefault(x => x.ObjectType == typeof(T))?.Pool.Release(poolable);
    }

    [Serializable]
    public class PoolObject
    {
        private Type _objectType;

        public Type ObjectType
        {
            get
            {
                if (poolObject is null || !poolObject)
                    return null;

                return _objectType ??= poolObject.GetComponent<IPoolable>().GetType();
            }
        }

        [ShowInInspector, LabelText("Object Type")]
        private string TypeName => ObjectType is not null ? ObjectType.Name : "Null";

        [SerializeField, Min(0)] private int defaultSize = 10;
        [SerializeField, Min(0)] private int maxPoolSize = 100;
        [SerializeField, Required] private GameObject poolObject;

        [ShowInInspector] private int ActiveCount => _pool?.CountActive ?? -1;
        [ShowInInspector] private int InactiveCount => _pool?.CountInactive ?? -1;

        private ObjectPool<IPoolable> _pool;

        public ObjectPool<IPoolable> Pool
        {
            get
            {
                // Collection checks will throw errors if we try to release an item that is already in the pool.
                return _pool ??= new ObjectPool<IPoolable>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                    OnDestroyPoolObject, true, defaultSize, maxPoolSize);
            }
        }

        private IPoolable CreatePooledItem()
        {
            var obj = Instantiate(poolObject, _transform);
            if (obj.TryGetComponent(out IPoolable poolable))
            {
                return poolable;
            }

            LogManager.LogError("Pool Object does not contain IPooling interface", obj);
            return null;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool(IPoolable poolable)
        {
            poolable.gameObject.SetActive(false);
            poolable.OnReturnToPool();
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(IPoolable poolable)
        {
            poolable.gameObject.SetActive(true);
            poolable.OnTakeFromPool(Pool);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(IPoolable poolable)
        {
            poolable.OnDestroyPoolObject();
        }
    }
}

public interface IPoolable
{
    GameObject gameObject { get; }

    public abstract void OnTakeFromPool(IObjectPool<IPoolable> pool);

    public abstract void OnReturnToPool();

    public abstract void OnDestroyPoolObject();
}