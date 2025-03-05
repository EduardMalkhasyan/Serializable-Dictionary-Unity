#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Project.Tools.DictionaryHelp
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            var indentedRect = EditorGUI.IndentedRect(rect);

            void Head()
            {
                var headerRect = indentedRect;
                headerRect.height = EditorGUIUtility.singleLineHeight;

                void ExpandablePanel()
                {
                    var fullHeaderRect = new Rect(headerRect);
                    fullHeaderRect.x -= 17;
                    fullHeaderRect.width += 34;

                    if (Event.current != null && fullHeaderRect.Contains(Event.current.mousePosition))
                    {
                        Color transparentGrey = new Color(0.4f, 0.4f, 0.4f, 0.4f);
                        EditorGUI.DrawRect(fullHeaderRect, transparentGrey);
                    }

                    GUI.color = Color.clear;

                    if (GUI.Button(new Rect(fullHeaderRect.x, fullHeaderRect.y, fullHeaderRect.width - 40,
                                            fullHeaderRect.height), ""))
                    {
                        prop.isExpanded = !prop.isExpanded;
                    }

                    GUI.color = Color.white;

                    var triangleRect = rect;
                    triangleRect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUI.Foldout(triangleRect, prop.isExpanded, "");
                }

                void DisplayName()
                {
                    GUI.color = Color.white;

#if UNITY_2022_1_OR_NEWER
                    var labelRect = headerRect;
                    labelRect.x += 12;
                    GUI.Label(labelRect, prop.displayName);
#else
                    GUI.Label(headerRect, prop.displayName);
#endif

                    GUI.color = Color.white;
                    GUI.skin.label.fontSize = 12;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                }

                void DuplicatedKeysWarning()
                {
                    if (Event.current != null && Event.current.type != EventType.Repaint)
                    {
                        return;
                    }

                    var hasRepeated = false;
                    var repeatedKeys = new List<string>();

                    for (int i = 0; i < dictionaryList.arraySize; i++)
                    {
                        SerializedProperty isKeyRepeatedProperty = dictionaryList.GetArrayElementAtIndex(i)
                                                                           .FindPropertyRelative("isKeyDuplicated");

                        if (isKeyRepeatedProperty.boolValue)
                        {
                            hasRepeated = true;
                            SerializedProperty keyProperty = dictionaryList.GetArrayElementAtIndex(i).FindPropertyRelative("Key");
                            string keyString = GetSerializedPropertyValueAsString(keyProperty);
                            repeatedKeys.Add(keyString);
                        }
                    }

                    if (!hasRepeated)
                    {
                        return;
                    }

                    float with = GUI.skin.label.CalcSize(new GUIContent(prop.displayName)).x;
                    headerRect.x += with + 35f;
                    var warningRect = headerRect;
                    Rect warningRectIcon = new Rect(headerRect.x - 18, headerRect.y, headerRect.width, headerRect.height);
                    GUI.color = Color.white;
                    GUI.Label(warningRectIcon, EditorGUIUtility.IconContent("console.erroricon"));
                    GUI.color = new Color(1.0f, 0.443f, 0.443f);
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.Label(warningRect, "Duplicated keys: " + string.Join(", ", repeatedKeys));
                    GUI.color = Color.white;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                }

                string GetSerializedPropertyValueAsString(SerializedProperty property)
                {
                    switch (property.propertyType)
                    {
                        case SerializedPropertyType.Integer:
                            return property.intValue.ToString();
                        case SerializedPropertyType.Boolean:
                            return property.boolValue.ToString();
                        case SerializedPropertyType.Float:
                            return property.floatValue.ToString();
                        case SerializedPropertyType.String:
                            return property.stringValue;
                        default:
                            return "(Unsupported Type)";
                    }
                }

                ExpandablePanel();
                DisplayName();
                DuplicatedKeysWarning();
            }

            void List()
            {
                if (!prop.isExpanded)
                {
                    return;
                }

                SetupList(prop);

                float newHeight = indentedRect.height - EditorGUIUtility.singleLineHeight - 3;
                indentedRect.y += indentedRect.height - newHeight;
                indentedRect.height = newHeight;

                reorderableList.DoList(indentedRect);
            }

            SetupProps(prop);

            Head();
            List();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            SetupProps(prop);

            var height = EditorGUIUtility.singleLineHeight;

            if (prop.isExpanded)
            {
                SetupList(prop);
                height += reorderableList.GetHeight() + 5;
            }

            return height;
        }

        private float GetListElementHeight(int index)
        {
            var kvpProp = dictionaryList.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            float GetPropertyHeight(SerializedProperty prop)
            {
                if (IsSingleLine(prop))
                {
                    return EditorGUI.GetPropertyHeight(prop);
                }

                var height = 1f;

                foreach (var childProp in GetChildren(prop, false))
                {
                    height += EditorGUI.GetPropertyHeight(childProp) + 1;
                }

                height += 10;

                return height;
            }

            return Mathf.Max(GetPropertyHeight(keyProp), GetPropertyHeight(valueProp));
        }

        void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect keyRect;
            Rect valueRect;
            Rect dividerRect;

            var kvpProp = dictionaryList.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            void Draw(Rect rect, SerializedProperty prop)
            {
                if (IsSingleLine(prop))
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, prop, GUIContent.none);
                }
                else
                {
                    foreach (var childProp in GetChildren(prop, false))
                    {
                        var childPropHeight = EditorGUI.GetPropertyHeight(childProp);
                        rect.height = childPropHeight;
                        EditorGUI.PropertyField(rect, childProp, true);
                        rect.y += childPropHeight + 2;
                    }
                }
            }

            void DrawRects()
            {
                var dividerWidh = IsSingleLine(valueProp) ? 6 : 16f;
                var dividerPosition = 0.25f;

                var fullRect = rect;
                fullRect.width -= 1;
                fullRect.height -= 2;

                keyRect = fullRect;
                keyRect.width *= dividerPosition;
                keyRect.width -= dividerWidh / 2;

                valueRect = fullRect;
                valueRect.x += fullRect.width * dividerPosition;
                valueRect.width *= (1 - dividerPosition);
                valueRect.width -= dividerWidh / 2;

                dividerRect = fullRect;
                dividerRect.x += fullRect.width * dividerPosition - dividerWidh / 2;
                dividerRect.width = dividerWidh;
            }

            void Key()
            {
                Draw(keyRect, keyProp);

                if (kvpProp.FindPropertyRelative("isKeyDuplicated").boolValue)
                {
                    GUI.Label(new Rect(keyRect.x + keyRect.width - 20, keyRect.y - 1, 20, 20),
                              EditorGUIUtility.IconContent("console.erroricon"));
                }
            }

            void Value()
            {
                Draw(valueRect, valueProp);

#if !ODIN_INSPECTOR
                if (valueProp.type.StartsWith("InterfaceHolder"))
                {
                    var interfaceValue = valueProp.FindPropertyRelative("value");
                    MonoBehaviour newValue = (MonoBehaviour)EditorGUI.ObjectField(valueRect,
                                              interfaceValue.objectReferenceValue, typeof(MonoBehaviour), true);

                    if (interfaceValue.objectReferenceValue != newValue)
                    {
                        if (newValue == null || newValue.GetComponent(
                            fieldInfo.FieldType.GenericTypeArguments[1].GenericTypeArguments[0]) != null)
                        {
                            interfaceValue.objectReferenceValue = newValue;
                        }
                        else
                        {
                            Debug.LogWarning($"Assigned object must implement interface " +
                                             $"{fieldInfo.FieldType.GenericTypeArguments[1].GenericTypeArguments[0].Name}");
                        }
                    }
                }
#endif
            }

            void Divider()
            {
                EditorGUIUtility.AddCursorRect(dividerRect, MouseCursor.ResizeHorizontal);

                if (Event.current == null || rect.Contains(Event.current.mousePosition) == false)
                {
                    return;
                }

                if (Event.current != null && dividerRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        //isDividerDragged = true;
                    }
                    else if (Event.current.type == EventType.MouseUp
                             || Event.current.type == EventType.MouseMove
                             || Event.current.type == EventType.MouseLeaveWindow)
                    {
                        isDividerDragged = false;
                    }
                }

                if (isDividerDragged && Event.current != null && Event.current.type == EventType.MouseDrag)
                {
                    dividerPosProp.floatValue = Mathf.Clamp(dividerPosProp.floatValue + Event.current.delta.x / rect.width, .2f, .8f);
                }
            }

            DrawRects();
            Key();
            Value();
            Divider();
        }

        private void ShowDictIsEmptyMessage(Rect rect)
        {
            GUI.Label(rect, "Empty");
        }

        private IEnumerable<SerializedProperty> GetChildren(SerializedProperty prop, bool enterVisibleGrandchildren)
        {
            prop = prop.Copy();

            var startPath = prop.propertyPath;

            var enterVisibleChildren = true;

            while (prop.NextVisible(enterVisibleChildren) && prop.propertyPath.StartsWith(startPath))
            {
                yield return prop;
                enterVisibleChildren = enterVisibleGrandchildren;
            }
        }

        private bool IsSingleLine(SerializedProperty prop)
        {
            return prop.propertyType != SerializedPropertyType.Generic || prop.hasVisibleChildren == false;
        }

        private void SetupList(SerializedProperty prop)
        {
            if (reorderableList != null)
            {
                return;
            }

            SetupProps(prop);

            this.reorderableList = new ReorderableList(dictionaryList.serializedObject, dictionaryList, true, false, true, true);
            this.reorderableList.drawElementCallback = DrawListElement;
            this.reorderableList.elementHeightCallback = GetListElementHeight;
            this.reorderableList.drawNoneElementCallback = ShowDictIsEmptyMessage;
        }

        private ReorderableList reorderableList;
        private bool isDividerDragged;

        public void SetupProps(SerializedProperty prop)
        {
            if (this.property != null)
            {
                return;
            }

            this.property = prop;
            this.dictionaryList = prop.FindPropertyRelative("dictionaryList");
            this.dividerPosProp = prop.FindPropertyRelative("dividerPos");
        }

        private SerializedProperty property;
        private SerializedProperty dictionaryList;
        private SerializedProperty dividerPosProp;
    }
}
#endif
