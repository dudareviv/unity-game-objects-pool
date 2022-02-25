using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.ObjectsPooling
{
    [CreateAssetMenu(fileName = "New Game Objects Pool", menuName = "Pools/Game Object")]
    public class GameObjectsPool : ObjectsPool<GameObject>
    {
        protected override GameObject CreateElementInternal()
        {
            var element = Instantiate(prefab, objectsPoolTransform, true);
            element.SetActive(false);
            element.name = $"{prefab.name} ({objectsPool.CountAll})";

            if (element.TryGetComponent(out PooledGameObject pooledGameObject))
            {
                pooledGameObject.pool = this;
            }
            else
            {
                var pooledObject = element.AddComponent<PooledGameObject>();
                pooledObject.pool = this;
            }

            return element;
        }

        protected override void OnElementGet(GameObject element)
        {
            base.OnElementGet(element);

            element.transform.parent = null;
            element.GetComponent<PooledGameObject>().OnGet();
        }

        protected override void OnElementRelease(GameObject element)
        {
            element.transform.parent = objectsPoolTransform;
            element.transform.localPosition = Vector3.zero;
            element.SetActive(false);

            element.GetComponent<PooledGameObject>().OnRelease();

            base.OnElementRelease(element);
        }

        public override void ReleaseAll()
        {
            var activeGameObjects = activeObjects.ToList();

            foreach (var activeObject in activeGameObjects)
            {
                Release(activeObject);
            }
        }
    }
}