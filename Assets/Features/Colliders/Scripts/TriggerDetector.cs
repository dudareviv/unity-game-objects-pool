using UnityEngine;
using UnityEngine.Events;

namespace Features.Colliders
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask availableLayer;

        [SerializeField]
        private UnityEvent<GameObject> triggerEnterEvent;

        [SerializeField]
        private UnityEvent<GameObject> triggerExitEvent;

        private void OnTriggerEnter(Collider other)
        {
            var otherGameObject = other.gameObject;

            if (!availableLayer.Includes(otherGameObject.layer))
            {
                return;
            }

            triggerEnterEvent?.Invoke(otherGameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            var otherGameObject = other.gameObject;

            if (!availableLayer.Includes(otherGameObject.layer))
            {
                return;
            }

            triggerExitEvent?.Invoke(otherGameObject);
        }
    }
}