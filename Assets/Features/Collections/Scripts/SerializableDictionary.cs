using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Collections
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> :
        DrawableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Properties

        [field: NonSerialized]
        public IEqualityComparer<TKey> Comparer { get; }

        #endregion

        #region Fields

        [NonSerialized]
        private Dictionary<TKey, TValue> m_dict;

        #endregion

        #region CONSTRUCTOR

        public SerializableDictionary()
        {
        }

        public SerializableDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        #endregion

        #region IDictionary Interface

        public int Count => m_dict?.Count ?? 0;

        public void Add(TKey key, TValue value)
        {
            m_dict ??= new Dictionary<TKey, TValue>(Comparer);
            m_dict.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            if (m_dict == null) return false;
            return m_dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(Comparer);
                return m_dict.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null) return false;
            return m_dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_dict == null)
            {
                value = default;
                return false;
            }

            return m_dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(Comparer);
                return m_dict.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (m_dict == null) throw new KeyNotFoundException();
                return m_dict[key];
            }
            set
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(Comparer);
                m_dict[key] = value;
            }
        }

        public void Clear()
        {
            if (m_dict != null) m_dict.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(Comparer);
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (m_dict == null) return;
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return m_dict?.GetEnumerator() ?? default;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_dict?.GetEnumerator() ?? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return m_dict?.GetEnumerator() ?? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
        }

        #endregion

        #region ISerializationCallbackReceiver

        [SerializeField]
        private TKey[] m_keys;

        [SerializeField]
        private TValue[] m_values;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_keys != null && m_values != null)
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(m_keys.Length, Comparer);
                else m_dict.Clear();
                for (var i = 0; i < m_keys.Length; i++)
                    if (i < m_values.Length)
                        m_dict[m_keys[i]] = m_values[i];
                    else
                        m_dict[m_keys[i]] = default;
            }

            m_keys = null;
            m_values = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (m_dict == null || m_dict.Count == 0)
            {
                m_keys = null;
                m_values = null;
            }
            else
            {
                var cnt = m_dict.Count;
                m_keys = new TKey[cnt];
                m_values = new TValue[cnt];
                var i = 0;
                using var e = m_dict.GetEnumerator();
                while (e.MoveNext())
                {
                    m_keys[i] = e.Current.Key;
                    m_values[i] = e.Current.Value;
                    i++;
                }
            }
        }

        #endregion
    }
}