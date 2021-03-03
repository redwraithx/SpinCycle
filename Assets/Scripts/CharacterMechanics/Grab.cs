
using GamePlaySystems.Utilities;
using Photon.Pun;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Transform grabPoint = null;
    public WeaponScript weapon = null;
    [SerializeField] public bool canPickUpItem = false;
    [SerializeField] private bool hasItemInHand = false;
    [SerializeField] private GameObject itemInHand = null;
    [SerializeField] internal GameObject itemToPickUp = null;
    [SerializeField] internal GameObject objectToInteractWith = null;
    [SerializeField] private ItemTypeForUsingItem machineInteractionObject = null;
    [SerializeField] private bool canUseHeldItem = false;
    [SerializeField] internal bool outOfRange = true;

    [SerializeField] private ItemTypeForUsingItem objectYouCanUse = null;

    
    [SerializeField] private PhotonView _photonView;


    public bool CanUseHeldItem
    {
        get => canUseHeldItem;
        set => canUseHeldItem = value;
    }

    private void Start()
    {
        if (!weapon)
            weapon = GetComponent<WeaponScript>();
        

        
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
                        //If you are getting an error that calls here, make sure the machine has the event set up properly
                       itemInHand = null;
                       
                       ClearGrabValues();
                       
                    }
                    else if (itemInHand.GetComponent<RepairToolUse>())
                    {
                        itemInHand.GetComponent<RepairToolUse>().UseItem();

                        itemInHand = null;

                        ClearGrabValues();
                    }

                    
                }
                else if (itemInHand.GetComponent<ItemTypeForItem>().itemType == ItemType.SabotageWaterGun)
                {
                    if(weapon.enabled)
                        weapon.fire();
                    //itemInHand.GetComponent<RepairToolUse>().UseItem();
                    Debug.Log("Gun");
                    //itemInHand = null;

                    //ClearGrabValues();
                }

            }
        }
        if (itemInHand)
        {
            var isValidItem = itemInHand?.GetComponent<ItemTypeForItem>();
            if (isValidItem)
            {
                Debug.Log(isValidItem.itemType);
                if (isValidItem.itemType == ItemType.SabotageWaterGun)
                {
                    if (!weapon.enabled)
                    {
                        weapon.enabled = true;
                        weapon.projectileSpawnPoint = itemInHand.GetComponentInChildren<Transform>();
                    }
                    if (!canUseHeldItem)
                        canUseHeldItem = true;
                    
                    Debug.Log("2");
                }
                
            }
            
        }
        else
        {
            if (weapon.enabled)
            {
                weapon.enabled = false;
                weapon.projectileSpawnPoint = null;
            }
            //if (!canUseHeldItem)
            //canUseHeldItem = false;
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
        RepairToolUse repairTool = null;
        

        if (item || machine)
        {
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;

                Item _item = other.gameObject.GetComponent<Item>();
                
                // // networking TEST
                // if (PickupObject(_item) == true)
                // {
                //     PickupObject(_item);
                // }
            }
            else if ((repairTool = other.GetComponent<RepairToolUse>()) == true)
            {
                canUseHeldItem = true;
                Debug.Log("GrabScriptRepairToolDebug");
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

            //if (itemInHand)
            //{
            //    var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
            //    if (isItemASabbotage)
            //    {
            //        Debug.Log(isItemASabbotage.itemType);
            //        if (isItemASabbotage.itemType == ItemType.SabotageWaterGun)
            //        {
            //            canUseHeldItem = true;
            //            weapon.enabled = true;
            //            Debug.Log("2");
            //        }
            //    }
            //}

        }
        
    }

    // [PunRPC]
    // bool PickupObject(Item itemObject)
    // {
    //     if (itemObject)
    //     {
    //         _photonView.RPC("RpcPickupItem", RpcTarget.AllBuffered, itemObject.gameObject.transform.position, itemObject.gameObject.transform.rotation);
    //         
    //         Debug.Log("updating position and rotation of object picked up");
    //         
    //
    //         return true;
    //     }
    //
    //     return false;
    // }
    //
    // [PunRPC]
    // protected void OnPickup(int viewId)
    // {
    //     PhotonView view = PhotonView.Find(viewId);
    //
    //     Debug.Log("id of object is from: " + view.gameObject.name);
    //     
    // }

    private void OnTriggerStay(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
        RepairToolUse repairTool = null;

        // can only hold items in your hand not machines
        if (item || machine)
        {
            Debug.Log("collided with " + other.gameObject.tag);
            
            
            // NEW VERSION
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
            }
            else if ((repairTool = other.GetComponent<RepairToolUse>()) == true)
            {
                canUseHeldItem = true;
                Debug.Log("GrabScriptRepairToolDebug2");
            }
            else
            {
                Debug.Log("GrabScriptRepairToolDebugElse");
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

            //if (itemInHand)
            //{
            //    var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
            //    if (isItemASabbotage)
            //    {
            //        if (isItemASabbotage.itemType == ItemType.SabotageWaterGun)
            //            canUseHeldItem = false;
            //    }
                
            //}


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
            if (isItemASabbotage == ItemType.SabotageWaterGun)
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
