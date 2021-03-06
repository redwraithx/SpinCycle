﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairToolUse : MonoBehaviour, IRepairToolUse
{
    public int repairIndex;
    public float timer;
    public Vector3 spawner;
    public void UseItem()
    {
        Debug.Log("repair tool goes away");
        RepairToolZoneSpawn.instance.RemoveObject();
        PhotonNetwork.Destroy(gameObject);
    }

    void Start()
    {
        spawner = RepairToolZoneSpawn.instance.spawnPointPosition;

        timer = 60;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, spawner) >= 1)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                RepairToolZoneSpawn.instance.RemoveObject();
                //Destroy(this.gameObject);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
