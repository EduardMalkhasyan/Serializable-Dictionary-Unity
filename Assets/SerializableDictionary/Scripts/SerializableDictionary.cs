using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectTools
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField, SDHelpBox] private List<KeyValueEntry> entries;
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
                var errorMessage = $"Warning founded duplicates in: {duplicates}";
                SDMessage.errorText = errorMessage;
                SDMessage.showIf = true;
            }
            else
            {
                SDMessage.showIf = false;
            }
        }
    }

    #region Editor
#if UNITY_EDITOR
    public enum SDHelpBoxMessageType { None, Info, Warning, Error }

    public static class SDMessage
    {
        public static string errorText;
        public static bool showIf;
    }

    public class SDHelpBoxAttribute : PropertyAttribute
    {
        public SDHelpBoxMessageType messageType;

        public SDHelpBoxAttribute(SDHelpBoxMessageType messageType = SDHelpBoxMessageType.Error)
        {
            this.messageType = messageType;
        }
    }

    [CustomPropertyDrawer(typeof(SDHelpBoxAttribute))]
    public class SDHelpBoxAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            var helpBoxAttribute = attribute as SDHelpBoxAttribute;
            if (helpBoxAttribute == null || SDMessage.showIf == false) return base.GetHeight();
            var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
            if (helpBoxStyle == null) return base.GetHeight();
            return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(SDMessage.errorText), EditorGUIUtility.currentViewWidth) + 4);
        }

        public override void OnGUI(Rect position)
        {
            var helpBoxAttribute = attribute as SDHelpBoxAttribute;
            if (helpBoxAttribute == null || SDMessage.showIf == false) return;
            EditorGUI.HelpBox(position, SDMessage.errorText, GetMessageType(helpBoxAttribute.messageType));
        }

        private MessageType GetMessageType(SDHelpBoxMessageType helpBoxMessageType)
        {
            switch (helpBoxMessageType)
            {
                default:
                case SDHelpBoxMessageType.None: return MessageType.None;
                case SDHelpBoxMessageType.Info: return MessageType.Info;
                case SDHelpBoxMessageType.Warning: return MessageType.Warning;
                case SDHelpBoxMessageType.Error: return MessageType.Error;
            }
        }
    }
#endif
    #endregion
}
