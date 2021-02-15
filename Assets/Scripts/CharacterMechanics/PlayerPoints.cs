using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayerPoints : MonoBehaviour
{
    public Text playerPointText = null;
    public int points = 0;

    public int Points
    {
        get => points;
        set
        {
            points += value;
            
            playerPointText.text = points.ToString();
        }
    }

    private void Update()
    {
        
    }
}
