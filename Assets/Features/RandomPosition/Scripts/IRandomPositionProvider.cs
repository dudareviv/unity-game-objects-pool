using UnityEngine;

namespace Features.RandomPosition
{
    public interface IRandomPositionProvider
    {
        Vector3 GetPosition();
    }
}