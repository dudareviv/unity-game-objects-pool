using Features.ObjectsPooling;
using Features.RandomPosition;
using UnityEngine;

namespace Features.Spawn
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObjectsPool pool;

        [SerializeField]
        private RandomPositionProvider positionProvider;

        public void Spawn(int count)
        {
            Debug.Log($"Trying to spawn {count} objects...", gameObject);

            for (var i = 0; i < count; i++)
            {
                var position = positionProvider.GetPosition();

                var pooledGameObject = pool.Get();
                pooledGameObject.transform.position = position;
                pooledGameObject.transform.parent = null;
                pooledGameObject.SetActive(true);
            }
        }
    }
}