#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Project.Tools.InterfaceHelp
{
    [System.Serializable]
    public class InterfaceHolder<T> where T : class
    {
        [SerializeField] private MonoBehaviour value;

        public T Value
        {
            get
            {
                if (value == null)
                {
                    Debug.LogError("value is null");
                    return null;
                }

                T castValue = value as T;
                if (castValue == null)
                {
                    Debug.LogError($"value cannot be cast to {typeof(T)}. It is of type {value.GetType()}");
                }

                return castValue;
            }
        }

        public InterfaceHolder(MonoBehaviour value)
        {
            this.value = value;
        }
    }

#if UNITY_EDITOR && !ODIN_INSPECTOR
    [CustomPropertyDrawer(typeof(InterfaceHolder<>))]
    public class InterfaceHolderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty valueProperty = property.FindPropertyRelative("value");

            EditorGUI.BeginChangeCheck();
            MonoBehaviour newValue = (MonoBehaviour)EditorGUI
                                     .ObjectField(position, label, valueProperty.objectReferenceValue,
                                      typeof(MonoBehaviour), true);

            if (EditorGUI.EndChangeCheck())
            {
                if (newValue == null || newValue.GetComponent(fieldInfo.FieldType.GenericTypeArguments[0]) != null)
                {
                    valueProperty.objectReferenceValue = newValue;
                }
                else
                {
                    Debug.LogWarning($"Assigned object must implement interface {fieldInfo.FieldType.GenericTypeArguments[0].Name}");
                }
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}
