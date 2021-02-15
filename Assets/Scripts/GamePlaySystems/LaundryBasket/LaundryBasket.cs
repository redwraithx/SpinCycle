
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;

public class LaundryBasket : MonoBehaviour
{

    public Text pointsText = null;
    public int points;
    PlayerPoints playerPoints = null;
    public string pointsToText;
    
    void Start()
    {
        playerPoints = GameObject.Find("PlayerCC").GetComponent<PlayerPoints>();
        points += playerPoints.Points;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += other.gameObject.GetComponent<Item>().Price;
                Debug.Log(points);
                other.gameObject.SetActive(false);
            }
        }
    }
    
    public void AddClothing(GameObject other)
    {
        if (other.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += other.gameObject.GetComponent<Item>().Price;
                
                Debug.Log(points);
                
                other.gameObject.SetActive(false);

                //pointsToText = points.ToString();
                //Debug.Log(pointsToText);
                //pointsText.text = pointsToText;
                playerPoints.Points += points;
            }
        }
    }
}
