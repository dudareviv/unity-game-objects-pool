using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Features.ObjectsPooling
{
    public abstract class ObjectsPool<T> : ScriptableObject,
        IDisposable, IObjectPool<T> where T : Object
    {
        [SerializeField]
        protected T prefab;

        [SerializeField]
        private int defaultCapacity = 4;

        [SerializeField]
        private int maxSize = 256;

        [SerializeField]
        private bool collectionCheck = true;

        [SerializeField]
        private UnityEvent<T> onGet;

        [SerializeField]
        private UnityEvent<T> onRelease;

        [SerializeField]
        private UnityEvent<T> onDestroy;

        protected List<T> activeObjects;

        protected ObjectPool<T> objectsPool;

        [NonSerialized]
        public Transform objectsPoolTransform;

        public void OnEnable()
        {
            activeObjects = new List<T>();

            objectsPool = new ObjectPool<T>(
                CreateElement,
                OnElementGet,
                OnElementRelease,
                OnElementDestroy,
                collectionCheck,
                defaultCapacity,
                maxSize
            );
        }

        public void Dispose()
        {
            Clear();
        }

        public T Get()
        {
            return objectsPool.Get();
        }

        public PooledObject<T> Get(out T v)
        {
            return objectsPool.Get(out v);
        }

        public void Release(T element)
        {
            objectsPool.Release(element);
        }

        public void Clear()
        {
            objectsPool.Clear();
        }

        public int CountInactive => objectsPool.CountInactive;

        private T CreateElement()
        {
            if (objectsPoolTransform == null)
            {
                var gameObject = new GameObject(name);
                gameObject.SetActive(false);
                objectsPoolTransform = gameObject.transform;
                DontDestroyOnLoad(objectsPoolTransform);
            }

            var element = CreateElementInternal();

            return element;
        }

        protected abstract T CreateElementInternal();

        protected virtual void OnElementGet(T element)
        {
            activeObjects.Add(element);
            onGet?.Invoke(element);
        }

        protected virtual void OnElementRelease(T element)
        {
            activeObjects.Remove(element);
            onRelease?.Invoke(element);
        }

        protected virtual void OnElementDestroy(T element)
        {
            onDestroy?.Invoke(element);
        }

        public virtual void ReleaseAll()
        {
            var activePooledObjects = activeObjects.ToList();

            foreach (var activeObject in activePooledObjects) Release(activeObject);
        }
    }
}