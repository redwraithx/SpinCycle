using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LaundrySpawn : MonoBehaviour
{
    public GameObject spawnObjectGameObject;
    public Item spawnObject;
    public GameObject spawnPoint;
    public Vector3 spawnPointPosition;
    public GameObject objectInstance;
    public string networkItemToSpawn = "";
    public VendingIndex VendingIndex;
    // Start is called before the first frame update
    void Start()
    {
        spawnPointPosition = spawnPoint.transform.position;
        spawnObject = spawnObjectGameObject.GetComponent<Item>();
        VendingIndex = new VendingIndex(spawnObject.name, spawnObject.Description, spawnObject.Price.ToString(), spawnObject.sprite);
        networkItemToSpawn = VendingIndex.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("trigger exit item spawner");
            //if not working make sure there is a collider set as a trigger on the object
            if (Vector3.Distance(objectInstance.transform.position, spawnPointPosition) >= 1 || objectInstance == null)
            {
               objectInstance = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), spawnPointPosition, Quaternion.identity);
            }
        }
    }

}
