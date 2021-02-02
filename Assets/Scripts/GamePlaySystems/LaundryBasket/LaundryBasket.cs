
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;

public class LaundryBasket : MonoBehaviour
{

    public Text pointsText = null;
    public int points;
    PlayerPoints playerPoints = null;
    public string pointsToText;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPoints = GameObject.Find("PlayerCC").GetComponent<PlayerPoints>();
        points += playerPoints.points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* public void DepositLaundry(GameObject other)
     {
         if (other.CompareTag("Item"))
         {
             Debug.Log($"we have a item {other.GetComponent<ItemTypeForItem>().itemType}");
             points += other.gameObject.GetComponent<TShirt>().Price;

         }
     }*/
    
    

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "Item")
    //     {
    //         if (collision.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
    //         {
    //             points += collision.gameObject.GetComponent<TShirt>().Price;
    //             Debug.Log(points);
    //             collision.gameObject.SetActive(false);
    //         }
    //     }
    // }
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
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
        if (other.tag == "Item")
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += other.gameObject.GetComponent<Item>().Price;
                
                Debug.Log(points);
                
                other.gameObject.SetActive(false);

                pointsToText = points.ToString();
                Debug.Log(pointsToText);
                pointsText.text = pointsToText;
                playerPoints.points += points;
            }
        }
    }
}
