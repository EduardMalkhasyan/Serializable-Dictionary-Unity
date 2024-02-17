using ProjectTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    [SerializeField] private Text GeorgeLabel;
    [SerializeField] private Image GeorgeImage;

    [SerializeField] private Text ThemisLabel;
    [SerializeField] private Image ThemisImage;

    [SerializeField] private SerializableDictionary<int, string> intToStringDictionary;
    [SerializeField] private SerializableDictionary<string, Color> stringToColorDictionary;
    [SerializeField] private SerializableDictionary<string, int[]> stringToIntArrayDictionary;

    private const string GeorgeName = "George";
    private const string ThemisName = "Themis";

    private void Start()
    {
        TestSetup();
    }

    public void TestSetup()
    {
        GeorgeLabel.text = intToStringDictionary[0];
        ThemisLabel.text = intToStringDictionary[1];

        GeorgeImage.color = stringToColorDictionary[GeorgeName];
        ThemisImage.color = stringToColorDictionary[ThemisName];
    }
}
