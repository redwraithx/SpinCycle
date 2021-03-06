﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RepairToolZoneSpawn : MonoBehaviour
{
    public string networkItemToSpawn = "NetworkRepairTool";
    public GameObject repairTool;
    public GameObject spawnPoint;
    public Vector3 spawnPointPosition;
    public GameObject repairToolInstance;
    private float spawnTimer;
    public bool OnDock;
    public List<GameObject> repairTools;
    public static RepairToolZoneSpawn instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(this);
        }
        instance = this;
        spawnPointPosition = spawnPoint.transform.position;
        if (PhotonNetwork.IsMasterClient)
        {
            repairToolInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
            repairToolInstance.GetComponent<RepairToolUse>().spawner = transform.position;


            spawnTimer += 3;
            repairTools.Add(repairToolInstance);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (Vector3.Distance(repairToolInstance.transform.position, spawnPointPosition) >= 1)
        {
                if (PhotonNetwork.IsMasterClient)
                {
                    repairToolInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
                    repairToolInstance.GetComponent<RepairToolUse>().spawner = transform.position;
                    spawnTimer += 3;
                    repairTools.Add(repairToolInstance);
                }
        }
    }


    public void RemoveObject()
    {
        repairTools.Remove(repairToolInstance);
    }

}
