using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyLaundrySpawner : MonoBehaviour
{

    public GameObject[] dirtyLaundryPrefabs;
    public GameObject spawnPoint;
    public float timeSinceLastSpawn = 0f;
    public float spawnRate;
    public bool isSpawning;
    float randTime;
    int laundryRandomizer;
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
            randTime = Random.Range(1f, 10f);
            timeSinceLastSpawn = Time.time;
            if (isSpawning == true)
            {
                laundryRandomizer = Random.Range(0, dirtyLaundryPrefabs.Length);
                SpawnDirtyLaundry(laundryRandomizer);
            }
        }
    }

    public void SpawnDirtyLaundry(int i)
    {
        GameObject newLaundry = Instantiate(dirtyLaundryPrefabs[i], spawnPoint.transform.position, Quaternion.identity);

    }
}
