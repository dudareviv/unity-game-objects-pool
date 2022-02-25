using UnityEngine;
using UnityEngine.Events;

namespace Features.RandomValues
{
    public class RandomIntegerProvider : MonoBehaviour
    {
        [SerializeField]
        private int min;

        [SerializeField]
        private int max;

        [SerializeField]
        private UnityEvent<int> onValueRaised;

        private void OnValidate()
        {
            if (min > max)
            {
                max = min;
                Debug.LogError("Min can not be greater than max", gameObject);
                return;
            }
        }

        public void RaiseValue()
        {
            var value = Random.Range(min, max);

            Debug.Log($"Value {value} raising...", gameObject);
            onValueRaised?.Invoke(value);
        }
    }
}