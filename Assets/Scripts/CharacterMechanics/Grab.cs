
using GamePlaySystems.Utilities;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Animator characterAnimator;
    public Transform grabPoint = null;
    public Transform target = null;
    public WeaponScript weapon = null;
    public GameObject weaponCamera;
    public bool justPutIn;
    public bool throwBomb;

    [SerializeField] public bool canPickUpItem = false;
    [SerializeField] private bool hasItemInHand = false;
    [SerializeField] internal GameObject itemInHand = null;
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
            if (!itemInHand)
            {
                AudioClip grabItem = Resources.Load<AudioClip>("AudioFiles/SoundFX/Player/GrabItem/Magnetic_Grab_SFX_Magnetic Grab2SFX-St");
                GameManager.audioManager.PlaySfx(grabItem);
            }

            characterAnimator.ResetTrigger("Idle2");
            hasItemInHand = true;
            GetComponent<PlayerSphereCast>().itemInHand = true;
            itemInHand = itemToPickUp;
            if (itemInHand)
            {
                var isValidItem = itemInHand?.GetComponent<ItemTypeForItem>();
                if (isValidItem.itemType == ItemType.SabotageWaterGun || isValidItem.itemType == ItemType.SabotageIceGun)
                    characterAnimator.SetBool("Shooting", true);
            }
            // characterAnimator.SetBool("PickUp", true);
            characterAnimator.SetTrigger("PickUp2");

            if (itemInHand.GetComponent<ItemTypeForItem>())
                itemInHand.GetComponent<ItemTypeForItem>().RequestOwnership();

            foreach (var itemCollider in itemInHand.GetComponents<Collider>())
            {
                itemCollider.enabled = false;
            }

            itemInHand.GetComponent<Item>().OwnerID = this.gameObject.GetComponent<PhotonView>().ViewID;
            itemInHand.GetComponent<Rigidbody>().useGravity = false;
            itemInHand.transform.position = grabPoint.position;

            itemInHand.transform.parent = gameObject.transform;
            itemInHand.GetComponent<Item>().UpdateObjectsRigidBody(true);

        }
    }



    private IEnumerator WaitCoroutine()
    {


        // Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        characterAnimator.SetBool("PutIn", true);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        characterAnimator.SetBool("PutIn", false);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);

    }

    private IEnumerator ThrowCoroutine()
    {

        Debug.Log("Start Couroutine Throwing Bomb");
        // Print the time of when the function is first called.
        Debug.Log("Started Throw at timestamp : " + Time.time);
        // characterAnimator.SetBool("Throw", true);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.6f);

        //After we have waited 5 seconds print the time again.
        // characterAnimator.SetBool("Throw", false);
        characterAnimator.SetTrigger("Idle2");
        Debug.Log("Finished Throw at timestamp : " + Time.time);
        // characterAnimator.SetBool("PickUp", false);
    }



    public void CheckForMouseUp()
    {
        characterAnimator.ResetTrigger("PickUp2");
        //make sure this part only play when bomb is not throw
        if (itemInHand)
        {


            var isValidItem = itemInHand?.GetComponent<ItemTypeForItem>();
            if (isValidItem.itemType == ItemType.SabotageWaterGun || isValidItem.itemType == ItemType.SabotageIceGun || isValidItem.itemType == ItemType.SabotageSoapGun)
                characterAnimator.SetBool("Shooting", false);
        }
        if (!throwBomb)
        {
            characterAnimator.SetTrigger("Idle2");
            // characterAnimator.SetBool("PickUp", false);
        }
        if (itemInHand)
        {

            if (itemInHand.GetComponent<DrawProjection>() != null)
                itemInHand.GetComponent<DrawProjection>().weaponScript = null;

            canPickUpItem = false;

            foreach (var itemCollider in itemInHand.GetComponents<Collider>())
            {
                itemCollider.enabled = true;
            }

            itemInHand.GetComponent<Rigidbody>().useGravity = true;
            itemInHand.GetComponent<Item>().UpdateObjectsRigidBody(false);



            if (itemInHand.GetComponent<ItemTypeForItem>())
                itemInHand.GetComponent<ItemTypeForItem>().RequestTransferOwnershipToHost();

            itemInHand.transform.parent = null;

            hasItemInHand = false;
            GetComponent<PlayerSphereCast>().itemInHand = false;
            itemInHand = null;
        }
    }

 


    private void Update()
    {
        if (justPutIn && !itemInHand)
        {

            justPutIn = false;
            characterAnimator.SetBool("PickUp", false);
            //characterAnimator.SetTrigger("PickUp2");

            StartCoroutine(WaitCoroutine());

        }
        else if (throwBomb && !itemInHand)
        {

            throwBomb = false;
            characterAnimator.SetBool("Throw", false);

            // StartCoroutine(ThrowCoroutine());

        }
        if (Input.GetMouseButtonDown(1))
        {
            CheckForMouseDown();

        }

        if (Input.GetMouseButtonUp(1))
        {
            CheckForMouseUp();
        }

        if (hasItemInHand)
        {
            if (itemInHand)
                itemInHand.transform.position = grabPoint.position;
            if (Input.GetMouseButtonDown(0))
            {

                var isValidItemObject = false;

                // is it an item? or weapon?
                if (itemInHand)
                    isValidItemObject = itemInHand.GetComponent<ItemTypeForItem>() ? true : false;

                if (isValidItemObject && canUseHeldItem)
                {

                    // characterAnimator.SetTrigger("PutOn");
                    Debug.Log("ItemInHand: " + itemInHand.gameObject.name);
                    if (machineInteractionObject)
                    {
                        Debug.Log("ItemInHand: " + itemInHand.gameObject.name + ", Using Machine");

                        justPutIn = true;
                        // use object action will only work on one event per object
                        machineInteractionObject.thisObjectEvent.Invoke(itemInHand);
                        //If you are getting an error that calls here, make sure the machine has the event set up properly
                        //itemInHand = null;

                        ClearGrabValues();

                    }
                    else if (itemInHand.GetComponent<RepairToolUse>())
                    {
                        Debug.Log("ItemInHand: " + itemInHand.gameObject.name + ", Using Repair");
                        characterAnimator.SetTrigger("RepairOpened");
                        itemInHand.GetComponent<RepairToolUse>().UseItem();

                        itemInHand = null;

                        ClearGrabValues();
                    }
                    else if (itemInHand.GetComponent<BombThrow>())
                    {
                        Debug.Log("ItemInHand: " + itemInHand.gameObject.name + ", Using Bomb");
                        //      characterAnimator.SetBool("Throw", true);
                        characterAnimator.SetTrigger("Throw2");
                        //soapBombThrow();
                    }


                }



            }
            if (Input.GetMouseButtonUp(0))
            {

            }
        }
        if (itemInHand)
        {
            var isValidItem = itemInHand?.GetComponent<ItemTypeForItem>();
            if (isValidItem)
            {

                if (isValidItem.itemType == ItemType.SabotageWaterGun || isValidItem.itemType == ItemType.SabotageIceGun || isValidItem.itemType == ItemType.SabotageSoapGun)
                {
                    if (!weapon.enabled)
                    {

                        weapon.itemType = isValidItem.itemType;
                        weapon.gun = itemInHand;
                        weapon.projectileSpawnPoint = itemInHand.GetComponentInChildren<Transform>();
                        weapon.destroyGun = weapon.gun.GetComponent<WeaponDestroyScript>();
                        weaponCamera.gameObject.SetActive(true);
                        itemInHand.gameObject.transform.rotation = transform.rotation;
                        if (isValidItem.itemType == ItemType.SabotageIceGun)
                            itemInHand.GetComponent<DrawProjection>().weaponScript = weapon;
                        weapon.enabled = true;


                    }
                    if (!canUseHeldItem)
                        canUseHeldItem = true;

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

        }
    }


    public void soapBombThrow()
    {
        Debug.Log("Before if Throwing Bomb");
        if (!itemInHand)
            return;
        Debug.Log("Throwing Bomb");
        throwBomb = true;
        itemInHand.GetComponent<BombThrow>().Throw();
        ThrowCoroutine();

        characterAnimator.SetTrigger("Idle2");
        // characterAnimator.SetBool("PickUp", false);

        CheckForMouseUp();

        itemInHand = null;

        ClearGrabValues();
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


            }
            else if ((repairTool = other.GetComponent<RepairToolUse>()) == true)
            {
                canUseHeldItem = true;
            }
            else
            {
                canUseHeldItem = false;
                machineInteractionObject = null;

            }


            if (item)
            {
                canPickUpItem = true;
                itemToPickUp = other.gameObject;

            }
            else
            {
                canPickUpItem = false;
                itemToPickUp = null;

            }


        }

    }


    private void OnTriggerStay(Collider other)
    {
        var item = other.gameObject.CompareTag("Item");
        var machine = other.gameObject.CompareTag("Machine");
        RepairToolUse repairTool = null;

        // can only hold items in your hand not machines
        if (item || machine)
        {


            // NEW VERSION
            if ((machineInteractionObject = other.GetComponent<ItemTypeForUsingItem>()) == true && itemInHand)
            {
                CanUseHeldItem = true;
            }
            else if ((repairTool = other.GetComponent<RepairToolUse>()) == true)
            {
                canUseHeldItem = true;
            }
            else
            {
                canUseHeldItem = false;
                machineInteractionObject = null;
            }


            if (item)
            {
                canPickUpItem = true;
                itemToPickUp = other.gameObject;
            }
            else
            {
                canPickUpItem = false;
                itemToPickUp = null;
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

            if (isItemASabbotage == ItemType.SabotageWaterGun)
            {
                canUseHeldItem = true;
            }


        }
        else
        {

            hasItemInHand = false;
            itemInHand = null;
            canUseHeldItem = false;

            GetComponent<PlayerSphereCast>().itemInHand = false;

        }

        canPickUpItem = false;
        itemToPickUp = null;


        machineInteractionObject = null;

    }

}
