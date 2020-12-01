using System;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour, IVendingMachine
{
    public GameObject[] prefabOfItemsForSale;
    public GameObject itemSpawnPoint;
    public GameObject VendingUI;
    public Button sabotage1;
    public Button sabotage2;
    public Button sabotage3;
    public Button sabotage4;
    public Button sabotage5;
    public Button sabotage6;
    public Button closeButton;
    public float closeDistance = 2;
    //Button array is temp, will be replaced with specific buttons once we figure out what thats gonna be
    void Start()
    {
        if (closeButton)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
        if (sabotage1)
        {
           sabotage1.onClick.AddListener(buttonSpawn);
        }
    }
    void Update()
    {

    }

    public void DisplayItemsForSale()
    {
        Debug.Log("Displaying items for sale to user");
    }

    public void buttonSpawn()
    {
        SpawnSoldItem(-1);
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
                VendingUI.SetActive(true);

<<<<<<< HEAD
                //*******this needs to be updated for the UI is attached, hard coding first Object******
                SpawnSoldItem(0);
=======
               // SpawnSoldItem(-1);
>>>>>>> Evan-Branch
            }
        }
        
    }


    private void CloseUI()
    {
        VendingUI.SetActive(false);
    }
    
}

