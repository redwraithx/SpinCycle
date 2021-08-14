﻿using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkedGrab : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject targetPlayer = null;
    public GameObject grabber;
    private Vector3 grabberPos;
    public Transform otherGrabber;
    private Grab grab = null;
    private StrengthBarUI strengthBar = null;
    private PlayerMovementCC playerCC = null;
    public bool isBeingGrabbed = false;
    public CapsuleCollider grabCollider = null;
    public bool isGrabbing = false;

    PhotonView myPhotonViewID = null;
    public const byte grabByte = 2;
    public const byte secondGrabByte = 3;
    //public const byte secondPlayerGrab = 3;

    RaycastHit hit;


    public string textString = "Sending";
    public string showTextString = "";
    public int counter = 0;

    public Text vectorText;
    public Text myVectorText;


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!myPhotonViewID)
            myPhotonViewID = GetComponent<PhotonView>();

        if (!grabCollider)
            grabCollider = GetComponentInChildren<CapsuleCollider>();

        if (!grab)
            grab = GetComponent<Grab>();

        if (!strengthBar)
            strengthBar = GetComponent<StrengthBarUI>();

        if (!playerCC)
            playerCC = GetComponent<PlayerMovementCC>();

        if (targetPlayer)
            targetPlayer = null;

        foreach (var collider in GetComponents<Collider>())
        {
            Physics.IgnoreCollision(grabCollider, collider, true);
        }


        Physics.SphereCast(transform.position + new Vector3(0f, 0.5f, 0f), 0.5f, transform.forward, out hit, 0.5f);

        if (hit.collider != null)
            Debug.Log("can hit: " + hit.collider.name);

        //otherGrabber.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //isBeingGrabbed = playerCC.isGrabbed;
        CheckGrab();
        if (isGrabbing == true)
        {
            playerCC.isGrabbing = true;
            if (PhotonNetwork.IsMasterClient)
                GrabMaster_S();
            else
                GrabSecondary_S();
        }
        else
        {
            playerCC.isGrabbing = false;
        }

        if (photonView.IsMine)
            grabberPos = grabber.transform.position;
        else
        {
            
           grabberPos = GetOtherGrabberPos();
        }
            

        //myVectorText.text = grabberPos.ToString();

        //vectorText.text = grabber.position.ToString();

    }

    private void CheckGrab()
    {

        if (targetPlayer && Input.GetMouseButtonDown(0) && !isBeingGrabbed)
        {
            if (!targetPlayer)
                return;

            isGrabbing = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
            if (PhotonNetwork.IsMasterClient)
                GrabMaster_S();
            else
                GrabSecondary_S();
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        GetTarget(other);
    }
    private void OnTriggerStay(Collider other)
    {
        GetTarget(other);
    }

    private void GetTarget(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (grab)
            if (grab.itemInHand)
                return;

        if (targetPlayer)
            return;

        if (!other.gameObject)
            return;

        targetPlayer = other.gameObject;
        Debug.Log("Target player = true");
    }

    private void OnTriggerExit(Collider other)
    {
        targetPlayer = null;
        Debug.Log("Target lost");
    }

    public void GrabMaster_S()
    {
        
        object[] package = new object[] {  grabberPos, isGrabbing };

        PhotonNetwork.RaiseEvent(
            (byte)grabByte,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }

    public void GrabSecondary_S()
    {
        
        object[] package = new object[] { this.grabberPos, isGrabbing };

        PhotonNetwork.RaiseEvent(
            (byte)secondGrabByte,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }

    public void OnEvent (EventData photonEvent)
    {
        if (photonEvent.Code == 2)
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                object[] data = (object[])photonEvent.CustomData;
                Vector3 grabMovement = (Vector3)data[0];
                playerCC.enemyGrab = grabMovement;            
                //Debug.Log(data[0]);
                playerCC.isGrabbed = (bool)data[1];

                //vectorText.text = grabMovement.ToString();
            }

        }
        
        if (photonEvent.Code == 3)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                object[] data = (object[])photonEvent.CustomData;
                Vector3 grabMovement = (Vector3)data[0];
                playerCC.enemyGrab = grabMovement;
                //Debug.Log(data[0]);
                playerCC.isGrabbed = (bool)data[1];

                //vectorText.text = grabMovement.ToString();
            }
        }
    }

    Vector3 GetOtherGrabberPos()
    {
        foreach (GameObject player in GameManager.networkLevelManager.playersJoined)
        {
            if (player.GetComponent<PhotonView>().ViewID != myPhotonViewID.ViewID)
            {
                return player.GetComponent<NetworkedGrab>().grabber.transform.position;
            }
        }
        return Vector3.zero;
    }

}
