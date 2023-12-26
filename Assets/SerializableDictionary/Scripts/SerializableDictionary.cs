using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [HelpBox(HelpBoxMessageType.Error)]
    [SerializeField] private List<KeyValueEntry> entries;
    private List<TKey> keys = new List<TKey>();

    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [Serializable]
    class KeyValueEntry
    {
        public TKey key;
        public TValue value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            dictionary.TryAdd(entries[i].key, entries[i].value);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
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
            var errorMessage = $"Warning {GetType().Name} keys has duplicates {duplicates}";
            Message.errorText = errorMessage;
            Message.showIf = true;
        }
        else
        {
            Message.showIf = false;
        }
    }

}

#if UNITY_EDITOR
public enum HelpBoxMessageType { None, Info, Warning, Error }

public static class Message
{
    public static string errorText;
    public static bool showIf;
}

public class HelpBoxAttribute : PropertyAttribute
{
    public HelpBoxMessageType messageType;

    public HelpBoxAttribute(HelpBoxMessageType messageType = HelpBoxMessageType.None)
    {
        this.messageType = messageType;
    }
}

[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxAttributeDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        var helpBoxAttribute = attribute as HelpBoxAttribute;
        if (helpBoxAttribute == null || !Message.showIf) return base.GetHeight();
        var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
        if (helpBoxStyle == null) return base.GetHeight();
        return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(Message.errorText), EditorGUIUtility.currentViewWidth) + 4);
    }

    public override void OnGUI(Rect position)
    {
        var helpBoxAttribute = attribute as HelpBoxAttribute;
        if (helpBoxAttribute == null || !Message.showIf) return;
        EditorGUI.HelpBox(position, Message.errorText, GetMessageType(helpBoxAttribute.messageType));
    }

    private MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType)
    {
        switch (helpBoxMessageType)
        {
            default:
            case HelpBoxMessageType.None: return MessageType.None;
            case HelpBoxMessageType.Info: return MessageType.Info;
            case HelpBoxMessageType.Warning: return MessageType.Warning;
            case HelpBoxMessageType.Error: return MessageType.Error;
        }
    }
}
#endif