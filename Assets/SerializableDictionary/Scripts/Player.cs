using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Image playerImage;
    [SerializeField] private PlayerColorNameCollection playerColorName;
    [SerializeField] private PlayerTypes playerTypes;

    private void Start()
    {
        Setup(playerColorName.dictionary[playerTypes].name, playerColorName.dictionary[playerTypes].color);
    }

    public void Setup(string name, Color color)
    {
        playerName.text = name;
        playerImage.color = color;
    }
}
