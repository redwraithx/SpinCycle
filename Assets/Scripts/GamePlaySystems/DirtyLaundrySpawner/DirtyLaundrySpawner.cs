using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;

public class DirtyLaundrySpawner : MonoBehaviour
{

    public GameObject[] dirtyLaundryPrefabs;
    public GameObject spawnPoint;
    public float timeSinceLastSpawn = 0f;
    public float spawnRate;
    public bool isSpawning;
    float randTime;
    int laundryRandomizer;
    public LaundryType laundryType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void SpawnDirtyLaundry(LaundryType type)
    {
        GameObject newLaundry = LaundryPool.poolInstance.GetItem(type);
        newLaundry.transform.position = spawnPoint.transform.position;
        newLaundry.transform.rotation = spawnPoint.transform.rotation;
        newLaundry.SetActive(true);

    }
}
