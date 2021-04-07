
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;
using EnumSpace;


public class MachineScript : MonoBehaviour
{
    public float cycleLength;
    public GameObject itemSpawnPoint;
    public float laundryTimer;
    public float sabotageTimer;
    public bool isSabotaged = false;
    public bool isEnabled = false;
    public bool isBoosted = false;
    public Slider sliderTime;
    public LaundryType laundryType;
    public MachineType machineType;
    public ParticleSystem part;

    private void Start()
    {
        cycleLength = 5;
        sliderTime.maxValue = cycleLength;

        if (isSabotaged == true)
        {
            part.Play();
        }
    }

    void Update()
    {
        if (isSabotaged == false)
        {
            if (laundryTimer > 0)
            {
                laundryTimer -= Time.deltaTime;
            }
            if (laundryTimer <= 0 && isEnabled == true)
            {
                SpawnFinishedProduct(laundryType);
                isEnabled = false;
            }
        }

        if (isSabotaged == true)
        {
            if (sabotageTimer > 0)
            {
                sabotageTimer -= Time.deltaTime;
            }
            if (sabotageTimer <= 0)
            {
                part.Stop();
                isSabotaged = false;
            }
        }

        sliderTime.value = laundryTimer;


    }

    public void SpawnFinishedProduct(LaundryType type)
    {
        GameObject laundry = LaundryPool.poolInstance.GetItem(type);
        laundry.transform.position = itemSpawnPoint.transform.position;
        laundry.transform.rotation = itemSpawnPoint.transform.rotation;

        //Determine laundry finished state by machine
        switch(machineType)
        {
            case MachineType.washer:
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemType.ClothingWet;
                break;
            case MachineType.dryer:
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemType.ClothingUnfolded;
                break;
            case MachineType.folder:
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemType.ClothingDone;
                break;
        }

        laundry.SetActive(true);
    }

    public void ProcessItems()
    {
        sliderTime.maxValue = cycleLength;
        laundryTimer = cycleLength;
        isEnabled = true;
    }

    public void UseMachine(GameObject other)
    {
        //Alter this tag based on machine
        if (other.CompareTag("Item"))
        {
            Debug.Log($"we have a item {other.GetComponent<ItemTypeForItem>().itemType}");
            laundryType = other.GetComponent<ItemTypeForItem>().laundryType;

            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.WasherBoost)
            {
                BoostMachine();
                other.gameObject.SetActive(false);
            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.RepairTool)
            {
                FixMachine();
                RepairToolSpawn.instance.RemoveObject();
                other.gameObject.SetActive(false);
            }
            else if (other.GetComponent<ItemTypeForItem>().itemType == this.gameObject.GetComponent<ItemTypeForUsingItem>().itemType[0])
            {
                //Once player is created, call to destroy the item in their hand here
                ProcessItems();
                // we may want to use a bool in case the machine is full we dont destroy or use the object
                other.transform.parent = null;
                other.gameObject.SetActive(false);
            }

        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "SoapBomb(Clone)")
        {
            SabotageMachine(60);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "EMPSphere")
        {
            if (isSabotaged == true)
            {
                Destroy(other.gameObject);
            }
            else
            {
                SabotageMachine(20);
                Destroy(other.gameObject);
            }
        }
    }

    public void SabotageMachine(float time)
    {
        sabotageTimer = time;
        isSabotaged = true;
        part.Play();
    }
    public void FixMachine()
    {
        isSabotaged = false;
        sabotageTimer = 0;
        part.Stop();
    }

    public void BoostMachine()
    {
        isBoosted = true;
    }
}
