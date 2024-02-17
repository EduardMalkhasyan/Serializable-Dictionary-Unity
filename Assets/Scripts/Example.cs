using ProjectTools;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<int, string> intToStringDictionary;
    [SerializeField] private SerializableDictionary<string, Color> stringToColorDictionary;
    [SerializeField] private SerializableDictionary<string, int[]> stringToIntArrayDictionary;

    [ContextMenu("Test")]
    private void Test()
    {
        Debug.Log(intToStringDictionary[1]);
    }
}
