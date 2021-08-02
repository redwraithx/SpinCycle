using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AOE : MonoBehaviourPun
{
    public float timer;
    public bool transferring;
    // Start is called before the first frame update
    private void Start()
    {

    }
    public void OnTriggerStay(Collider other)
    {
       
        if(other.gameObject.CompareTag("Machine"))
        {
            other.gameObject.BroadcastMessage("SabotageMachine");

            if (transferring == false)
            {
                GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
                transferring = true;
            }
        }
    }

    public void Update()
    {
        if (transferring == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                TransferOwner();
            }
        }
    }


    public void TransferOwner()
    {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
    }
}
