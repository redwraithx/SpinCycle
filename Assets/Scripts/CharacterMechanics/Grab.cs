
using GamePlaySystems.Utilities;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(1) )
        {
            CheckForMouseDown();
            
        }
        else if (Input.GetMouseButtonUp(1) )
        {
            CheckForMouseUp();
        }

        if (hasItemInHand)
        {
            
            if (Input.GetMouseButtonDown(0) )
            {
                var isValidItemObject = false;
                
                // is it an item? or weapon?
                if(itemInHand)
                    isValidItemObject = itemInHand.GetComponent<ItemTypeForItem>() ? true : false;

                if (isValidItemObject && canUseHeldItem)
                {
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

    private void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
        

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
            var isItemASabbotage = itemInHand.GetComponent<ItemTypeForItem>().itemType;
            if (isItemASabbotage == ItemTypeForItem.ItemType.SabotageWaterGun)
            {
                canUseHeldItem = true;
            }
            
        }
        else
        {
            itemInHand = null;
            canUseHeldItem = false;
            GetComponent<PlayerSphereCast>().itemInHand = false;
        }

        canPickUpItem = false;
        itemToPickUp = null;
        

        machineInteractionObject = null;

    }
    
}
