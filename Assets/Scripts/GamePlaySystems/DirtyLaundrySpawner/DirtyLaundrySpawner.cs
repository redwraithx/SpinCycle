using System;
using System.Linq;
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;


public class DirtyLaundrySpawner : MonoBehaviour
{
    public GameObject spawnPoint;
    public float timeSinceLastSpawn = 0f;
    public float spawnRate = 0f;
    public bool isSpawning = true;
    float randTime = 0f;
    int laundryRandomizer = 0;
    public LaundryType laundryType;
    
    

    void Update()
    {
        spawnRate = randTime;
        if (Time.time >= timeSinceLastSpawn + spawnRate)
        {
            randTime = UnityEngine.Random.Range(1f, 10f);
            timeSinceLastSpawn = Time.time;
            if (isSpawning == true)
            {
                laundryType = (LaundryType)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(LaundryType)).Cast<LaundryType>().Max());
                SpawnDirtyLaundry(laundryType);
            }
        }
    }

    private void SpawnDirtyLaundry(LaundryType type)
    {
        GameObject newLaundry = LaundryPool.poolInstance.GetItem(type);
        newLaundry.transform.position = spawnPoint.transform.position;
        newLaundry.transform.rotation = spawnPoint.transform.rotation;
        newLaundry.GetComponent<ItemTypeForItem>().itemType = ItemType.ClothingDirty;
        newLaundry.SetActive(true);

    }
}
