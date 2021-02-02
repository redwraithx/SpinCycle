using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerPoints : MonoBehaviour
{
    public Text playerPointText;
    public int points;

    private void Awake()
    {

    }
    private void Update()
    {
        playerPointText.text = points.ToString();
    }
}
