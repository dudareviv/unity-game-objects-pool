using UnityEngine;

public static class LayerMaskExtension
{
    public static bool Includes(this LayerMask source, int layer)
    {
        return (source.value & 1 << layer) > 0;
    }
}