using UnityEngine;
using UnityEngine.Events;

namespace Features.ObjectsPooling
{
    public class PooledGameObject : MonoBehaviour
    {
        public GameObjectsPool pool;

        [SerializeField]
        private UnityEvent onRelease;

        [SerializeField]
        private UnityEvent onGet;

        public void Release()
        {
            pool.Release(gameObject);
        }

        public void OnGet()
        {
            onGet?.Invoke();
        }

        public void OnRelease()
        {
            onRelease?.Invoke();
        }
    }
}