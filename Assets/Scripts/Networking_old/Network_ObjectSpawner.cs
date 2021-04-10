
using System.IO;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;



public class Network_ObjectSpawner : MonoBehaviourPun
{
    // public LaundryType itemObjectTypeToSpawn = LaundryType.None;
    // public ItemType itemToSpawn = ItemType.None;
    //
    //
    private void Start()
    {
    //     if (itemObjectTypeToSpawn != LaundryType.None && itemToSpawn != ItemType.None)
    //     {
            // each item in game (not many) should be listed here in if else or switch


            if (PhotonNetwork.IsMasterClient)
            {
                GameObject newObject = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "NetworkTShirt"), transform.position, Quaternion.identity, 0);

                newObject.name = newObject.name + "_" + newObject.GetComponent<PhotonView>().ViewID;
                Debug.Log(newObject.name + ": " + newObject.GetComponent<PhotonView>().Owner);
                
                //newObject.AddComponent<PhotonView>();
                //newObject.AddComponent<PhotonTransformView>();
                
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }


            //newObject.GetComponent<PhotonView>().ViewID = PhotonNetwork.PlayerList.Length + 1;

            //     }
            //     
            //     
    }


    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject.tag;
    }

    
}
