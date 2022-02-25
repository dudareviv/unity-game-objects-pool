using UnityEngine;

namespace Features.RandomPosition
{
    public abstract class RandomPositionProvider : MonoBehaviour, IRandomPositionProvider
    {
        public abstract Vector3 GetPosition();
    }
}