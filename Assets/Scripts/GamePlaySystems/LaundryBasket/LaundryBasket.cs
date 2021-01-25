using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;
using EnumSpace;

public class LaundryBasket : MonoBehaviour
{

    public Text pointsText = null;
    public int points;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = points.ToString();
    }

    /* public void DepositLaundry(GameObject other)
     {
         if (other.CompareTag("Item"))
         {
             Debug.Log($"we have a item {other.GetComponent<ItemTypeForItem>().itemType}");
             points += other.gameObject.GetComponent<TShirt>().Price;

         }
     }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            if (collision.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += collision.gameObject.GetComponent<TShirt>().Price;
                Debug.Log(points);
                collision.gameObject.SetActive(false);
            }
        }
    }
}
