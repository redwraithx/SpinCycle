
using GamePlaySystems.Utilities;
<<<<<<< HEAD
=======
using Photon.Pun;
>>>>>>> main
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Transform grabPoint = null;
<<<<<<< HEAD
    
=======
    public WeaponScript weapon = null;
    public GameObject weaponCamera;
>>>>>>> main
    [SerializeField] public bool canPickUpItem = false;
    [SerializeField] private bool hasItemInHand = false;
    [SerializeField] internal GameObject itemInHand = null;
    [SerializeField] internal GameObject itemToPickUp = null;
    [SerializeField] internal GameObject objectToInteractWith = null;
    [SerializeField] private ItemTypeForUsingItem machineInteractionObject = null;
    [SerializeField] private bool canUseHeldItem = false;
    [SerializeField] internal bool outOfRange = true;

    [SerializeField] private ItemTypeForUsingItem objectYouCanUse = null;

<<<<<<< HEAD
=======
    
    [SerializeField] private PhotonView _photonView;


>>>>>>> main
    public bool CanUseHeldItem
    {
        get => canUseHeldItem;
        set => canUseHeldItem = value;
    }

<<<<<<< HEAD
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
    
    
    
=======
    private void Start()
    {
        if (!weapon)
            weapon = GetComponent<WeaponScript>();
        

        
    }
>>>>>>> main
    private void CheckForMouseDown()
    {
        if (canPickUpItem && itemToPickUp && outOfRange == false)
        {
            

            hasItemInHand = true;
            GetComponent<PlayerSphereCast>().itemInHand = true;
            itemInHand = itemToPickUp;
<<<<<<< HEAD
=======
            
            if(itemInHand.GetComponent<ItemTypeForItem>())
                itemInHand.GetComponent<ItemTypeForItem>().RequestOwnership();
>>>>>>> main

            foreach (var itemCollider in itemInHand.GetComponents<Collider>())
            {
                    itemCollider.enabled = false;
            }
            
            itemInHand.GetComponent<Rigidbody>().useGravity = false;
            itemInHand.transform.position = grabPoint.position;
            
            itemInHand.transform.parent = gameObject.transform;
<<<<<<< HEAD
=======
            itemInHand.GetComponent<Item>().UpdateObjectsRigidBody(true);
>>>>>>> main
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
<<<<<<< HEAD
=======
        itemInHand.GetComponent<Item>().UpdateObjectsRigidBody(false);
        
        if(itemInHand.GetComponent<ItemTypeForItem>())
            itemInHand.GetComponent<ItemTypeForItem>().RequestTransferOwnershipToHost();
        
>>>>>>> main
        itemInHand.transform.parent = null;

        hasItemInHand = false;
        GetComponent<PlayerSphereCast>().itemInHand = false;
        itemInHand = null;
    }

    private void Update()
    {
<<<<<<< HEAD
        if (Input.GetMouseButtonDown(1) )// || Input.GetButton("Fire2"))
=======
        if (Input.GetMouseButtonDown(1) )
>>>>>>> main
        {
            CheckForMouseDown();
            
        }
<<<<<<< HEAD
        else if (Input.GetMouseButtonUp(1) )//|| Input.GetButton("Fire2"))
=======
        else if (Input.GetMouseButtonUp(1) )
>>>>>>> main
        {
            CheckForMouseUp();
        }

        if (hasItemInHand)
        {
            
<<<<<<< HEAD
            if (Input.GetMouseButtonDown(0) )// || Input.GetButton("Fire1"))
            {
                Debug.Log("trying to use item in hand");

=======
            if (Input.GetMouseButtonDown(0) )
            {
>>>>>>> main
                var isValidItemObject = false;
                
                // is it an item? or weapon?
                if(itemInHand)
                    isValidItemObject = itemInHand.GetComponent<ItemTypeForItem>() ? true : false;

                if (isValidItemObject && canUseHeldItem)
                {
<<<<<<< HEAD
                    Debug.Log("Using item in hand now");

=======
>>>>>>> main
                    if (machineInteractionObject)
                    {
                       // use object action will only work on one event per object
                       machineInteractionObject.thisObjectEvent.Invoke(itemInHand);
<<<<<<< HEAD

                       itemInHand = null;
=======
                        //If you are getting an error that calls here, make sure the machine has the event set up properly
                       //itemInHand = null;
>>>>>>> main
                       
                       ClearGrabValues();
                       
                    }
<<<<<<< HEAD
                    
                }

            }
        }
=======
                    else if (itemInHand.GetComponent<RepairToolUse>())
                    {
                        itemInHand.GetComponent<RepairToolUse>().UseItem();

                        itemInHand = null;

                        ClearGrabValues();
                    }
                    else if (itemInHand.GetComponent<BombThrow>())
                    {
                        itemInHand.GetComponent<BombThrow>().Throw();

                        CheckForMouseUp();

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
                if (isValidItem.itemType == ItemType.SabotageWaterGun || isValidItem.itemType == ItemType.SabotageIceGun || isValidItem.itemType == ItemType.SabotageSoapGun)
                {
                    if (!weapon.enabled)
                    {
                        weapon.enabled = true;
                        weapon.itemType = isValidItem.itemType;
                        weapon.gun = itemInHand;
                        weapon.projectileSpawnPoint = itemInHand.GetComponentInChildren<Transform>();
                        weaponCamera.gameObject.SetActive(true);
                        itemInHand.gameObject.transform.rotation = transform.rotation;
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
                weaponCamera.gameObject.SetActive(false);
            }
            //if (!canUseHeldItem)
            //canUseHeldItem = false;
        }
>>>>>>> main
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

<<<<<<< HEAD
    //We should rewrite this function with the spherecast instead of the collider.  I have added private internal objectToInteractWith that has a value passed to it whenever the spherecast is on a machine.  
    //Further parameters of that function are available in the spherecast script but it should be working properly if we want to just rewrite the below functions using that variable instead.

=======
>>>>>>> main
    private void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
<<<<<<< HEAD
        
        // can only hold items in your hand not machines
=======
        RepairToolUse repairTool = null;
        

>>>>>>> main
        if (item || machine)
        {
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
<<<<<<< HEAD
=======

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
>>>>>>> main
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

<<<<<<< HEAD
            if (itemInHand)
            {
                var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
                if (isItemASabbotage)
                {
                    if (isItemASabbotage.itemType == ItemTypeForItem.ItemType.SabotageWaterGun)
                        canUseHeldItem = false;
                }
            }
=======
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
>>>>>>> main

        }
        
    }
<<<<<<< HEAD
=======
    
>>>>>>> main

    private void OnTriggerStay(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
<<<<<<< HEAD
        
=======
        RepairToolUse repairTool = null;

>>>>>>> main
        // can only hold items in your hand not machines
        if (item || machine)
        {
            Debug.Log("collided with " + other.gameObject.tag);
            
            
            // NEW VERSION
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
            }
<<<<<<< HEAD
            else
            {
=======
            else if ((repairTool = other.GetComponent<RepairToolUse>()) == true)
            {
                canUseHeldItem = true;
                Debug.Log("GrabScriptRepairToolDebug2");
            }
            else
            {
                Debug.Log("GrabScriptRepairToolDebugElse");
>>>>>>> main
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

<<<<<<< HEAD
            if (itemInHand)
            {
                var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
                if (isItemASabbotage)
                {
                    if (isItemASabbotage.itemType == ItemTypeForItem.ItemType.SabotageWaterGun)
                        canUseHeldItem = false;
                }
            }
=======
            //if (itemInHand)
            //{
            //    var isItemASabbotage = itemInHand?.GetComponent<ItemTypeForItem>();
            //    if (isItemASabbotage)
            //    {
            //        if (isItemASabbotage.itemType == ItemType.SabotageWaterGun)
            //            canUseHeldItem = false;
            //    }
                
            //}


>>>>>>> main
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
<<<<<<< HEAD
            Debug.Log("you have item in hand");
            
            var isItemASabbotage = itemInHand.GetComponent<ItemTypeForItem>().itemType;
            if (isItemASabbotage == ItemTypeForItem.ItemType.SabotageWaterGun)
            {
                canUseHeldItem = true;
                
                Debug.Log("you have sabotageWaterGun");
=======
            var isItemASabbotage = itemInHand.GetComponent<ItemTypeForItem>().itemType;
            if (isItemASabbotage == ItemType.SabotageWaterGun)
            {
                canUseHeldItem = true;
>>>>>>> main
            }
            
        }
        else
        {
<<<<<<< HEAD
            Debug.Log("you have NO item");
            
=======
>>>>>>> main
            itemInHand = null;
            canUseHeldItem = false;
            GetComponent<PlayerSphereCast>().itemInHand = false;
        }

        canPickUpItem = false;
        itemToPickUp = null;
        

        machineInteractionObject = null;

    }
    
}
