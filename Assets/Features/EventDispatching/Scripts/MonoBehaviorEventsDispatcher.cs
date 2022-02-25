using UnityEngine;
using UnityEngine.Events;

namespace Features.EventDispatching
{
    public class MonoBehaviorEventsDispatcher : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onAwakened;

        [SerializeField]
        private UnityEvent onStarted;

        [SerializeField]
        private UnityEvent onOnEnabled;

        [SerializeField]
        private UnityEvent onOnDisabled;

        private void Awake()
        {
            Debug.Log("Awake dispatching...", gameObject);
            onAwakened?.Invoke();
        }

        private void Start()
        {
            Debug.Log("Start dispatching...", gameObject);
            onStarted?.Invoke();
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable dispatching...", gameObject);
            onOnEnabled?.Invoke();
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable dispatching...", gameObject);
            onOnDisabled?.Invoke();
        }
    }
}