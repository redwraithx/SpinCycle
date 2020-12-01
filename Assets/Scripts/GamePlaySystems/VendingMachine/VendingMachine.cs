﻿using System;
using UnityEngine;


public class VendingMachine : MonoBehaviour, IVendingMachine
{
    public GameObject[] prefabOfItemsForSale;
    public GameObject itemSpawnPoint;
    
    
    public void DisplayItemsForSale()
    {
        Debug.Log("Displaying items for sale to user");
    }

    public void SpawnSoldItem(int itemPrefabID)
    {
        // if (prefabOfItemsForSale.Length <= 0 || itemSpawnPoint) 
        //     return;
        
        
        GameObject newItem = Instantiate(prefabOfItemsForSale[itemPrefabID], itemSpawnPoint.transform.position, Quaternion.identity);
        Destroy(newItem, 12f);    
        
        
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


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("press E to spawn item trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("User using vending machine trigger");

                //*******this needs to be updated for the UI is attached, hard coding first Object******
                SpawnSoldItem(0);
            }
        }
        
    }
    
}

