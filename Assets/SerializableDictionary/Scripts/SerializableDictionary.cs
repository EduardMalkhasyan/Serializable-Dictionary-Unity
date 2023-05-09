using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerializableDictionarySO<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValueEntry> entries;
    private List<TKey> keys = new List<TKey>();

    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [Serializable]
    class KeyValueEntry
    {
        public TKey key;
        public TValue value;
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            dictionary.Add(entries[i].key, entries[i].value);
        }
    }

    public void OnBeforeSerialize()
    {
        if (entries == null)
        {
            return;
        }

        keys.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            keys.Add(entries[i].key);
        }

        var result = keys.GroupBy(x => x)
                         .Where(g => g.Count() > 1)
                         .Select(x => new { Element = x.Key, Count = x.Count() })
                         .ToList();

        if (result.Count > 0)
        {
            var duplicates = string.Join(", ", result);
            Debug.LogError($"Warning {GetType().FullName} keys has duplicates {duplicates}");
        }
    }
}

public class SerializableDictionary<TKey, TValue> : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValueEntry> entries;
    private List<TKey> keys = new List<TKey>();

    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [Serializable]
    class KeyValueEntry
    {
        public TKey key;
        public TValue value;
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            dictionary.Add(entries[i].key, entries[i].value);
        }
    }

    public void OnBeforeSerialize()
    {
        if (entries == null)
        {
            return;
        }

        keys.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            keys.Add(entries[i].key);
        }

        var result = keys.GroupBy(x => x)
                         .Where(g => g.Count() > 1)
                         .Select(x => new { Element = x.Key, Count = x.Count() })
                         .ToList();

        if (result.Count > 0)
        {
            var duplicates = string.Join(", ", result);
            Debug.LogError($"Warning {GetType().FullName} keys has duplicates {duplicates}");
        }
    }
}
