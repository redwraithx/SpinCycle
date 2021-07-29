﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BombDetonate : MonoBehaviourPun
{
    public GameObject Bomb;
    public GameObject Radius = null;
    public string radiusName;
    public bool detonated;
    public float timer;
    public float timerAdjust;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {

        if (CanDetonateObject(collision))
        {

            if (collision.gameObject.tag == "machine")
            {
                BroadcastMessage("SabotageMachine");
            }


             Radius = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", radiusName), transform.position, transform.rotation);
             detonated = true;


            timer = timerAdjust;


        }

    }
    private bool CanDetonateObject(Collision other)
    {


        if (detonated)
            return false;

        if (transform.parent != null)
            return false;

        //if (other.gameObject?.GetComponent<Grab>().itemInHand == null)
        //    return false;

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("ItemStand")) // may need to reverse this
            return false;

        if (GetComponent<Item>().OwnerID == 0)
            return false;

        return true;
    }
    private void Update()
    {
        Debug.Log(photonView.Owner.IsMasterClient + " for master client owning this soap bomb");

        if (detonated == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {

                if (photonView.Owner.IsMasterClient == true)
                {
                  PhotonNetwork.Destroy(gameObject);
                }

            }
        }

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(detonated);
        }
        if(stream.IsReading)
        {
            detonated = (bool) stream.ReceiveNext();
        }
    }
}
