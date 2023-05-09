using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class PlayerColorNameCollection : SerializableDictionarySO<PlayerTypes, PlayerEntry>, ISerializationCallbackReceiver
{

}

[Serializable]
public class PlayerEntry
{
    public string name;
    public Color color;
}

public enum PlayerTypes
{
    Wojak,
    Doomer,
    Zoomer,
    Other
}