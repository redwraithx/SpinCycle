using System;
using UnityEngine;


public class VendingMachine : MonoBehaviour, IVendingMachine
{
    public GameObject[] prefabOfItemsForSale;
    //[SerializeField] private GameObject usersInteractableUI;
    public GameObject itemSpawnPoint;


    public void DisplayItemsForSale()
    {
        Debug.Log("Displaying items for sale to user");
    }

    public void SpawnSoldItem(string itemName)
    {
       // Debug.Log("Instantiate objects that were told to the user");

        if (prefabOfItemsForSale.Length > 0 && itemSpawnPoint) 
            Instantiate(prefabOfItemsForSale[0], itemSpawnPoint.transform.position, Quaternion.identity);
        // else
        // {
        //     Debug.Log("error spawning item, itemSpawnPoint or prefab is null");
        // }
        
        Debug.Log("FAKE SOAP WAS PURCHASED!");
    
    }

    public bool CanBuyItem(int itemForSalesValue, int usersCurrentCashAmount)
    {
        if (itemForSalesValue < usersCurrentCashAmount)
        {
            Debug.Log("user has enough cash to buy this item");

            return true;
        }
    
        Debug.Log("user does not have enough cash to buy this item");
    
        return false;
    }

    public void EnableDisableDisplayUI(bool value)
    {
    
        if(value)
            Debug.Log("enable the vending machine ui");
        else
            Debug.Log("disable the vending machine ui");
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("press E to spawn item");
    //
    //     if (other.CompareTag("Player"))
    //     {
    //         if (Input.GetKeyDown(KeyCode.E))
    //         {
    //             Debug.Log("User using vending machine");
    //
    //             SpawnSoldItem("test");
    //         }
    //     }
    //     
    // }

    //rivate void OnTriggerStay(Collider other)
    // private void OnControllerColliderHit(ControllerColliderHit other)
    // {
    //     Debug.Log("press E to spawn item");
    //
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         if (Input.GetKeyDown(KeyCode.E))
    //         {
    //             Debug.Log("User using vending machine");
    //
    //
    //             SpawnSoldItem("test");
    //         }
    //     }
    //     
    // }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("press E to spawn item trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("User using vending machine trigger");


                SpawnSoldItem("test");
            }
        }
        
    }
    
}

