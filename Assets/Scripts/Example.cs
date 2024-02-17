using ProjectTools;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<int, string> intToStringDictionary;
    [SerializeField] private SerializedDictionary<string, Color> stringToColorDictionary;
    [SerializeField] private SerializedDictionary<string, int[]> stringToIntArrayDictionary;
    [SerializeField] private SerializedDictionary<string, List<int>> stringToIntListDictionary;
}
