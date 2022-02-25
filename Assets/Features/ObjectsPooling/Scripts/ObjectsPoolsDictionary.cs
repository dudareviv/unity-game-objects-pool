using System;
using Core.Collections;
using Object = UnityEngine.Object;

namespace Features.ObjectsPooling
{
    [Serializable]
    public class ObjectsPoolsDictionary<T> : SerializableDictionary<Object, ObjectsPool<T>> where T : Object
    {
    }
}