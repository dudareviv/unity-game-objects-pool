using Features.ObjectsPooling;
using UnityEngine;

namespace Core.Pool
{
    [CreateAssetMenu(fileName = "New Master Game Objects Pool", menuName = "Pools/Master Game Objects")]
    public class MasterGameObjectsPool : ScriptableObject
    {
        [SerializeField]
        private ObjectsPoolsDictionary<GameObject> dictionary = new ObjectsPoolsDictionary<GameObject>();

        public bool HasKey(Object key)
        {
            return dictionary.ContainsKey(key);
        }

        public GameObject Get(Object key)
        {
            return dictionary[key].Get();
        }

        public GameObject Get(Object key, bool active)
        {
            var pooledGameObject = dictionary[key].Get();
            pooledGameObject.SetActive(active);

            return pooledGameObject;
        }

        public void Release(Object key, GameObject element)
        {
            dictionary[key].Release(element);
        }

        public void ReleaseAll()
        {
            foreach (var objectsPool in dictionary)
            {
                objectsPool.Value.ReleaseAll();
            }
        }
    }
}