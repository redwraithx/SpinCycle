using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Grab : MonoBehaviour
{
    public Transform grabPoint = null;
    [SerializeField] private bool canPickUpItem = false;
    [SerializeField] private bool hasItemInHand = false;
    

    [SerializeField] private GameObject itemInHand = null;
    [SerializeField] private GameObject itemToPickUp = null;


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
        if (canPickUpItem && itemToPickUp)
        {
            

            hasItemInHand = true;
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
        itemInHand = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckForMouseDown();
            
        }

        if (Input.GetMouseButtonUp(1))
        {
            CheckForMouseUp();
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

    private void OnTriggerEnter(Collider other)
    {
        // can only hold items in your hand not machines
        if (!other.gameObject.CompareTag("Item"))
            return;
        
        canPickUpItem = true;
        itemToPickUp = other.gameObject;
        
    }

    private void OnTriggerStay(Collider other)
    {
        // can only hold items in your hand not machines
        if (!other.gameObject.CompareTag("Item"))
            return;

        canPickUpItem = true;
        itemToPickUp = other.gameObject;
        
    }


    private void OnTriggerExit(Collider other)
    {
        canPickUpItem = false;
        itemToPickUp = null;
    }
}
