using System;
using UnityEngine;

public class EnemyColorNameCollection : SerializableDictionary<EnemyTypes, EnemyEntry>
{

}

[Serializable]
public class EnemyEntry
{
    public string name;
    public Color color;
}

public enum EnemyTypes
{
    Bogdanoff,
    Gigachad,
    Chad,
    Other
}