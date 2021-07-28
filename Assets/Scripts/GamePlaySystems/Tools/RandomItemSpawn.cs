using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RandomItemSpawn : MonoBehaviour
{
    public GameObject spawnObjectGameObject;
    public Item spawnObject;
    public GameObject spawnPoint;
    public Vector3 spawnPointPosition;
    public GameObject objectInstance;
    public string networkItemToSpawn = "";
    public VendingIndex VendingIndex;

    [Header("Randomiser stuff")]

    public GameObject[] allSpawnableObjects;
    public int randomNum;

    [Header("SpeedBoostSpawnStuff")]

    public int itemsSpawned;
    public int randomSpeed1;
    public int randomSpeed2;
    public GameObject speedBoost;
        


    // Start is called before the first frame update
    void Start()
    {
        RandomNumber();
        spawnPointPosition = spawnPoint.transform.position;
        spawnObject = spawnObjectGameObject.GetComponent<Item>();
        VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
        networkItemToSpawn = VendingIndex.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
        }
        SpeedBoostRandomSetup();
        RandomNumber();
    }

    private void Update()
    {

    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit item spawner" + objectInstance);
        RespawnItem();

    }


    public void RespawnItem()
    {
        if (objectInstance == null)
        {
            Debug.Log("trigger exit item spawner Null");
            if (PhotonNetwork.IsMasterClient)
            {
                spawnObject = spawnObjectGameObject.GetComponent<Item>();
                VendingIndex = this.GetComponent<VendingIndex>();
                VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                networkItemToSpawn = VendingIndex.Name;
                CheckForSpeedBoost();
                objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
            }
        }
        else if (objectInstance != null)
        {
            Debug.Log("trigger exit item spawner NotNull");
            if (Vector3.Distance(objectInstance.transform.position, spawnPointPosition) >= 1)
            {
                if (PhotonNetwork.IsMasterClient)
                { 
                    spawnObject = spawnObjectGameObject.GetComponent<Item>();
                    VendingIndex = this.GetComponent<VendingIndex>();
                    VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                    networkItemToSpawn = VendingIndex.Name;
                    CheckForSpeedBoost();
                    objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("Item Exit Trigger not null or null");
        }
    }

    public void SpeedBoostRandomSetup()
    {
        randomSpeed1 = Random.Range(1, 15);
        randomSpeed2 = Random.Range(1, 20);

        if(randomSpeed1 == randomSpeed2)
        {
            SpeedBoostRandomSetup();
        }
    }
    public void CheckForSpeedBoost()
    {
        itemsSpawned += 1;

        if(itemsSpawned == randomSpeed1 || itemsSpawned == randomSpeed2)
        {
            spawnObjectGameObject = speedBoost;
        }
        else
        {
            RandomNumber();
        }
    }

    public void RandomNumber()
    {
        randomNum = Random.Range(0, allSpawnableObjects.Length - 1);
        if (allSpawnableObjects.Length > 0)
        {
            spawnObjectGameObject = allSpawnableObjects[randomNum];
        }
    }

}

