using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Text enemyName;
    [SerializeField] private Image enemyImage;
    [SerializeField] private EnemyColorNameCollection enemyColorName;
    [SerializeField] private EnemyTypes enemyTypes;

    private void Start()
    {
        Setup(enemyColorName.dictionary[enemyTypes].name, enemyColorName.dictionary[enemyTypes].color);
    }

    public void Setup(string name, Color color)
    {
        enemyName.text = name;
        enemyImage.color = color;
    }
}
