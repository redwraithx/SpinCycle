using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
public class RandomItemSpawn : MonoBehaviourPun, IPunObservable
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
    public TMP_Text itemName;

    [Header("SpeedBoostSpawnStuff")]

    public int itemsSpawned;
    public int randomSpeed1;
    public int randomSpeed2;
    public GameObject speedBoost;

    [Header("Time")]

    public float timer;
    public float timeTotal;
    public TMP_Text countdown;
    public bool timeRunning;



    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RandomNumber();
            spawnPointPosition = spawnPoint.transform.position;
            spawnObject = spawnObjectGameObject.GetComponent<Item>();
            VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
            networkItemToSpawn = VendingIndex.Name;
            itemName.text = networkItemToSpawn;

            objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);

            //SpeedBoostRandomSetup();
            RandomNumber();
        }
    }

    private void Update()
    {
        
        if(itemName.text != networkItemToSpawn)
        {
            itemName.text = networkItemToSpawn;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            countdown.text = Mathf.Round(timer).ToString();
        }
        if (timer <= 0 && timeRunning == true)
        {
            RespawnItem();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit item spawner" + objectInstance);
        if (timer <= 0)
        {
            if (objectInstance == null || Vector3.Distance(objectInstance.transform.position, spawnPointPosition) >= 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    timer += timeTotal;
                    timeRunning = true;
                }
            }
        }

    }


    public void RespawnItem()
    {
        if (objectInstance == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                RandomNumber();
                spawnObject = spawnObjectGameObject.GetComponent<Item>();
                VendingIndex = this.GetComponent<VendingIndex>();
                VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                networkItemToSpawn = VendingIndex.Name;
                //CheckForSpeedBoost();
                timeRunning = false;
                objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);

            }
        }
        else if (objectInstance != null)
        {
            if (Vector3.Distance(objectInstance.transform.position, spawnPointPosition) >= 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    RandomNumber();
                    spawnObject = spawnObjectGameObject.GetComponent<Item>();
                    VendingIndex = this.GetComponent<VendingIndex>();
                    VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
                    networkItemToSpawn = VendingIndex.Name;
                    //CheckForSpeedBoost();
                    timeRunning = false;
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

        if (randomSpeed1 == randomSpeed2)
        {
            SpeedBoostRandomSetup();
        }
    }
    public void CheckForSpeedBoost()
    {
        itemsSpawned += 1;

        if (itemsSpawned == randomSpeed1 || itemsSpawned == randomSpeed2)
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
        randomNum = Random.Range(0, allSpawnableObjects.Length);
        if (allSpawnableObjects.Length > 0)
        {
            spawnObjectGameObject = allSpawnableObjects[randomNum];
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeRunning);
            stream.SendNext(timer);
            stream.SendNext(networkItemToSpawn);
        }

        if(stream.IsReading)
        {
            bool isrunning = (bool)stream.ReceiveNext();
            float timing = (float)stream.ReceiveNext();
            string named = (string)stream.ReceiveNext();

            if (isrunning != timeRunning)
                timeRunning = isrunning;

            if (timing != timer)
                timer = timing;

            if (named != networkItemToSpawn)
                networkItemToSpawn = named;
        }
    }
}

