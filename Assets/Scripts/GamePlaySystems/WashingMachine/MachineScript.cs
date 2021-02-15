
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;
using EnumSpace;
public class MachineScript : MonoBehaviour
{
    public float cycleLength;
    public GameObject itemSpawnPoint;
    public float timer;
    public bool timerSabotage = false;
    public bool isEnabled = false;
    public Slider sliderTime;
    public LaundryType laundryType;
    public MachineType machineType;

    private void Start()
    {
        sliderTime.maxValue = cycleLength;
    }
    void Update()
    {
        if (timerSabotage == false)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0 && isEnabled == true)
            {
                SpawnFinishedProduct(laundryType);
                isEnabled = false;
            }
        }

        sliderTime.value = timer;

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
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemTypeForItem.ItemType.ClothingWet;
                break;
            case MachineType.dryer:
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemTypeForItem.ItemType.ClothingUnfolded;
                break;
            case MachineType.folder:
                laundry.GetComponent<ItemTypeForItem>().itemType = ItemTypeForItem.ItemType.ClothingDone;
                break;
        }

        laundry.SetActive(true);
    }

    public void ProcessItems()
    {
        timer = cycleLength;
        isEnabled = true;
        
    }

    public void UseMachine(GameObject other)
    {   

        //Alter this tag based on machine
        if (other.CompareTag("Item"))
        {
            Debug.Log($"we have a item {other.GetComponent<ItemTypeForItem>().itemType}");
            laundryType = other.GetComponent<ItemTypeForItem>().laundryType;
                
            //Once player is created, call to destroy the item in their hand here
            ProcessItems();

            // we may want to use a bool incase the machine is full we dont destroy or use the object
            other.transform.parent = null;
            other.gameObject.SetActive(false);
        }

    }
}
