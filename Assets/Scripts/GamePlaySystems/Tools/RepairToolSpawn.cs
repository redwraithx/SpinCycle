using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairToolSpawn : MonoBehaviour
{
    public GameObject repairTool;
    public GameObject spawnPoint;
    private Vector3 spawnPointPosition;
    public GameObject repairToolInstance;
    public float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        spawnPointPosition = spawnPoint.transform.position;
        Instantiate(repairTool, spawnPointPosition, Quaternion.identity);
        spawnTimer += 3;
    }

    // Update is called once per frame
    void Update()
    {
      if (GameObject.Find("RepairTool(Clone)") == null)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnPointPosition = spawnPoint.transform.position;
                Instantiate(repairTool, spawnPointPosition, Quaternion.identity);
                spawnTimer += 3;
            }

        }


    }


}
