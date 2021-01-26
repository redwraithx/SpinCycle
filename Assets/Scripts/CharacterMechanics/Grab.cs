using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using GamePlaySystems.Utilities;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.UIElements;

public class Grab : MonoBehaviour
{
    public Transform grabPoint = null;
    
    [SerializeField] public bool canPickUpItem = false;
    [SerializeField] private bool hasItemInHand = false;
    [SerializeField] private GameObject itemInHand = null;
    [SerializeField] internal GameObject itemToPickUp = null;
    [SerializeField] internal GameObject objectToInteractWith = null;
    [SerializeField] private ItemTypeForUsingItem machineInteractionObject = null;
    [SerializeField] private bool canUseHeldItem = false;
    [SerializeField] internal bool outOfRange = true;

    [SerializeField] private ItemTypeForUsingItem objectYouCanUse = null;

    public bool CanUseHeldItem
    {
        get => canUseHeldItem;
        set => canUseHeldItem = value;
    }

    private void OnMouseDown()
    {
        // orifinal code
        // GetComponent<Collider>().enabled = false; // update from BoxCollider to Collider to work with all colliders
        // GetComponent<Rigidbody>().useGravity = false;
        // this.transform.position = grabPoint.position;
        //
        // // making the player the child of the item they pick up?
        // this.transform.parent = GameObject.Find("Item").transform; // this is expensive, also updated "Other" to "Item as that is the tag for all items thus far
    }

    private void OnMouseUp()
    {
        // original
        // this.transform.parent = null;
        // GetComponent<Rigidbody>().useGravity = true;
        // GetComponent<Collider>().enabled = true; // update from BoxCollider to Collider to work with all colliders
    }
    
    
    
    private void CheckForMouseDown()
    {
        if (canPickUpItem && itemToPickUp && outOfRange == false)
        {
            

            hasItemInHand = true;
            GetComponent<PlayerSphereCast>().itemInHand = true;
            itemInHand = itemToPickUp;

            foreach (var itemCollider in itemInHand.GetComponents<Collider>())
            {
                    itemCollider.enabled = false;
            }
            
            itemInHand.GetComponent<Rigidbody>().useGravity = false;
            itemInHand.transform.position = grabPoint.position;
            
            itemInHand.transform.parent = gameObject.transform;
        }
    }

    private void CheckForMouseUp()
    {
        
        


        if (!itemInHand)
            return;
        
        canPickUpItem = false;

        foreach (var itemCollider in itemInHand.GetComponents<Collider>())
        {
                itemCollider.enabled = true;
        }
        
        itemInHand.GetComponent<Rigidbody>().useGravity = true;
        itemInHand.transform.parent = null;

        hasItemInHand = false;
        GetComponent<PlayerSphereCast>().itemInHand = false;
        itemInHand = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) )// || Input.GetButton("Fire2"))
        {
            CheckForMouseDown();
            
        }
        else if (Input.GetMouseButtonUp(1) )//|| Input.GetButton("Fire2"))
        {
            CheckForMouseUp();
        }

        if (hasItemInHand)
        {
            
            if (Input.GetMouseButtonDown(0))// || Input.GetButton("Fire1"))
            {
                Debug.Log("trying to use item in hand");

                var isValidItemObject = false;
                
                // is it an item? or weapon?
                if(itemInHand)
                    isValidItemObject = itemInHand.GetComponent<ItemTypeForItem>() ? true : false;

                if (isValidItemObject && canUseHeldItem)
                {
                    Debug.Log("Using item in hand now");

                    if (machineInteractionObject)
                    {
                       // use object action will only work on one event per object
                       machineInteractionObject.thisObjectEvent.Invoke(itemInHand);

                       itemInHand = null;
                       
                       ClearGrabValues();
                       
                    }
                    
                }

            }
        }
    }

    
    private void DropItemInHand()
    {
        if (!itemInHand)
            return;
    
        canPickUpItem = false;

        foreach (var itemCollider in itemInHand.GetComponents<Collider>())
        {
            itemCollider.enabled = true;
        }
        
        itemInHand.GetComponent<Rigidbody>().useGravity = true;
        itemInHand.transform.SetParent(null);

        itemInHand = null;
    }

    //We should rewrite this function with the spherecast instead of the collider.  I have added private internal objectToInteractWith that has a value passed to it whenever the spherecast is on a machine.  
    //Further parameters of that function are available in the spherecast script but it should be working properly if we want to just rewrite the below functions using that variable instead.

    private void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
        
        // can only hold items in your hand not machines
        if (item || machine)
        {
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
            }
            else
            {
                canUseHeldItem = false;
                machineInteractionObject = null;
            
            }
            
            
            if(item)
            {
                canPickUpItem = true;
                itemToPickUp = other.gameObject;
            
            }
            else
            {
                canPickUpItem = false;
                itemToPickUp = null;
            
            }

            if (itemInHand)
            {
                var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
                if (isItemASabbotage)
                {
                    if (isItemASabbotage.itemType == ItemTypeForItem.ItemType.SabotageWaterGun)
                        canUseHeldItem = false;
                }
            }

        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
        
        // can only hold items in your hand not machines
        if (item || machine)
        {
            Debug.Log("collided with " + other.gameObject.tag);
            
            
            // NEW VERSION
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
            }
            else
            {
                canUseHeldItem = false;
                machineInteractionObject = null;
            }
            
            
            if(item)
            {
                canPickUpItem = true;
                itemToPickUp = other.gameObject;
            }
            else
            {
                canPickUpItem = false;
                itemToPickUp = null;
            }

            if (itemInHand)
            {
                var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
                if (isItemASabbotage)
                {
                    if (isItemASabbotage.itemType == ItemTypeForItem.ItemType.SabotageWaterGun)
                        canUseHeldItem = false;
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        ClearGrabValues();
    }
    

    private void ClearGrabValues()
    {
        if (itemInHand)
        {
            Debug.Log("you have item in hand");
            
            var isItemASabbotage = itemInHand.GetComponent<ItemTypeForItem>().itemType;
            if (isItemASabbotage == ItemTypeForItem.ItemType.SabotageWaterGun)
            {
                canUseHeldItem = true;
                
                Debug.Log("you have sabotageWaterGun");
            }
            
        }
        else
        {
            Debug.Log("you have NO item");
            
            itemInHand = null;
            canUseHeldItem = false;
            GetComponent<PlayerSphereCast>().itemInHand = false;
        }

        canPickUpItem = false;
        itemToPickUp = null;
        

        machineInteractionObject = null;

    }
    
}
