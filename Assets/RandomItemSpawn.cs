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
    public int maxSpeedBoosts;
    public int speedBoostsUsed;
    public int randomNum;



    // Start is called before the first frame update
    void Start()
    {

        randomNum = Random.Range(0, allSpawnableObjects.Length - 1);
        if (allSpawnableObjects.Length > 0)
        {
            spawnObjectGameObject = allSpawnableObjects[randomNum];
        }

        spawnPointPosition = spawnPoint.transform.position;
        spawnObject = spawnObjectGameObject.GetComponent<Item>();
        VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
        networkItemToSpawn = VendingIndex.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
        }
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
                randomNum = Random.Range(0, allSpawnableObjects.Length - 1);
                if (allSpawnableObjects.Length > 0)
                {
                    spawnObjectGameObject = allSpawnableObjects[randomNum];
                }
                spawnObject = spawnObjectGameObject.GetComponent<Item>();
                VendingIndex = this.GetComponent<VendingIndex>();
                VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                networkItemToSpawn = VendingIndex.Name;
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
                    randomNum = Random.Range(0, allSpawnableObjects.Length - 1);
                    if (allSpawnableObjects.Length > 0)
                    {
                        spawnObjectGameObject = allSpawnableObjects[randomNum];
                    }
                    spawnObject = spawnObjectGameObject.GetComponent<Item>();
                    VendingIndex = this.GetComponent<VendingIndex>();
                    VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                    networkItemToSpawn = VendingIndex.Name;
                    objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("Item Exit Trigger not null or null");
        }
    }

}

