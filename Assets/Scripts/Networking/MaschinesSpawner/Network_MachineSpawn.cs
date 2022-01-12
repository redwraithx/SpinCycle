using System.IO;
using UnityEngine;
using Photon.Pun;

public class Network_MachineSpawn : MonoBehaviour
{
    public string networkItemToSpawn = "";

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newObject = PhotonNetwork.Instantiate(Path.Combine("NetworkMachinePrefabs", networkItemToSpawn), transform.position, transform.rotation, 0);
            //newObject.transform.rotation = transform.rotation;
            newObject.name = newObject.name + "_" + newObject.GetComponentInChildren<PhotonView>().ViewID;

            Destroy(gameObject);
        }
        else
        {
            // dont keep this is your just a client, delete it as the master client is going to spawn all needed objects
            Destroy(gameObject);
        }
    }
}