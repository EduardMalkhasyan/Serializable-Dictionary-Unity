# Serializable Dictionary in Unity

## Download Unity Package:
[Download](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/releases)

## How to Use:
Just simpy import Unity Package it contains only two scripts and it ready to use!

This SerializableDictionary provides functionality similar to Unity's Dictionary<TKey, TValue>, allowing it to be displayed in the Unity Inspector.
Works also with [Odin Inspector](https://odininspector.com/) and without. 

Example script
```csharp
public SerializableDictionary<int, string> intToStringDictionary;
```

![all right](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/e0ae59f1-1b72-4d33-bb18-c2a1e1c802d4)
![can hold inside array](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/0c4a7b6e-2323-47b4-9fd9-f5f2d45c8ec6)

## Error Detections:

It can automatically detect duplicates and display them in the Inspector
![mutiple duplicated keys](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/99fc3aef-d764-436a-8b48-0bab0983700e)

Also in Debugger when it will be called
![aaaaaaaaa](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/fd9ba251-1389-4a5f-8d21-018d87e718f3)

## Extra Serialize Interfaces
Serialize Interfaces From Version 2.01 and above
![image](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/9750342a-19fd-4b5d-8462-566972f41d0a)
![image](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/558baa41-43be-4fd3-a4b0-2fd50ce60720)

## Simple Example in Use
You can simply clone the project and check the results. Here is a very simple example for use

```csharp
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
```

![image](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity/assets/78969017/6de88d75-b586-4ae4-a8bd-371670e250a5)
