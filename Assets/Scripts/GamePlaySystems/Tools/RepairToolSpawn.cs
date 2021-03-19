using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairToolSpawn : MonoBehaviour
{
    public GameObject repairTool;
    public GameObject spawnPoint;
    public Vector3 spawnPointPosition;
    public GameObject repairToolInstance;
    public float spawnTimer;
    public List<GameObject> repairTools;
    public static RepairToolSpawn instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if(instance)
        {
            Destroy(this);
        }
        instance = this;
        spawnPointPosition = spawnPoint.transform.position;
        repairToolInstance = Instantiate(repairTool, spawnPointPosition, Quaternion.identity);
        repairToolInstance.GetComponent<RepairToolUse>().spawner = transform.position;
        spawnTimer += 3;
        repairTools.Add(repairToolInstance);
    }

    // Update is called once per frame
    void Update()
    {

        repairToolInstance = GameObject.Find("RepairTool(Clone)");

        if (repairToolInstance == null)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnPointPosition = spawnPoint.transform.position;
                Instantiate(repairTool, spawnPointPosition, Quaternion.identity);
                spawnTimer = 3;
                repairTools.Add(repairToolInstance);
            }

        }
        else if (Vector3.Distance(repairToolInstance.transform.position, spawnPointPosition) >= 1)
        {
            if (repairTools.Count <= 1)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    spawnPointPosition = spawnPoint.transform.position;
                    Instantiate(repairTool, spawnPointPosition, Quaternion.identity);
                    spawnTimer = 3;
                    repairTools.Add(repairToolInstance);
                }
            }
        }
        else
        {
            //Debug.Log(repairTools.Count);
        }
    }

    public void RemoveObject()
    {
       repairTools.Remove(repairToolInstance);
    }

}
