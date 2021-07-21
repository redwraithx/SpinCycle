using System;
<<<<<<< HEAD
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;
=======
using System.Linq;
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;
using Photon.Pun;
using Photon.Realtime;

>>>>>>> main

public class DirtyLaundrySpawner : MonoBehaviour
{
    public GameObject spawnPoint;
    public float timeSinceLastSpawn = 0f;
<<<<<<< HEAD
    public float spawnRate;
    public bool isSpawning;
    float randTime;
    int laundryRandomizer;
=======
    public float spawnRate = 0f;
    public bool isSpawning = true;
    float randTime = 0f;
    int laundryRandomizer = 0;
>>>>>>> main
    public LaundryType laundryType;
    
    

    void Update()
    {
<<<<<<< HEAD
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

    public void SpawnDirtyLaundry(LaundryType type)
=======
        if (PhotonNetwork.IsMasterClient)
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

    }

    private void SpawnDirtyLaundry(LaundryType type)
>>>>>>> main
    {
        GameObject newLaundry = LaundryPool.poolInstance.GetItem(type);
        newLaundry.transform.position = spawnPoint.transform.position;
        newLaundry.transform.rotation = spawnPoint.transform.rotation;
<<<<<<< HEAD
        newLaundry.SetActive(true);
=======
        newLaundry.GetComponent<ItemTypeForItem>().itemType = ItemType.ClothingDirty;
        newLaundry.GetComponent<Item>().EnableObject();
>>>>>>> main

    }
}
