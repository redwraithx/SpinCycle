using System.IO;
using UnityEngine;
using Photon.Pun;

public class Network_ItemSpawn : MonoBehaviour
{
    public string networkItemToSpawn = "";

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newObject = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), transform.position, Quaternion.identity, 0);

            newObject.name = newObject.name + "_" + newObject.GetComponent<PhotonView>().ViewID;
            Debug.Log(newObject.name + ": " + newObject.GetComponent<PhotonView>().Owner);

            Destroy(gameObject);
        }
        else
        {
            // dont keep this is your just a client, delete it as the master client is going to spawn all needed objects
            Destroy(gameObject);
        }
    }

    private void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject.tag;
    }
}