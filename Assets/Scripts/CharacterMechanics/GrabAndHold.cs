using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrabAndHold : MonoBehaviourPunCallbacks
{
    public GameObject targetPlayer = null;
    private Grab grab = null;
    private StrengthBarUI strengthBar = null;
    private PlayerMovementCC playerCC = null;
    public CapsuleCollider grabCollider = null;
    internal bool isBeingGrabbed = false;
    internal bool isHoldingOtherPlayer = false;
    float playerSpeedWhenBeingGrabed;
    private bool isGettingOwnership = false;

    PhotonView originalClientPhotonViewID = null;
    PhotonView myPhotonViewID = null;

    private void Start()
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

        //Ignores all other colliders, except the sphere collider.
        foreach (var collider in GetComponents<Collider>())
        {
            Physics.IgnoreCollision(grabCollider, collider, true);
        }

        
    }

    private void Update()
    {
        if (targetPlayer)
            CheckGrab();
    }

    private void CheckGrab() 
    {
        GrabAndHold otherPlayersGrabAndHoldScript = targetPlayer.GetComponent<GrabAndHold>();

        if (!otherPlayersGrabAndHoldScript)
            return;

        if (targetPlayer && Input.GetMouseButtonDown(0) && !isBeingGrabbed)
        {
            if (!targetPlayer)
                return;

            otherPlayersGrabAndHoldScript.isBeingGrabbed = true;



            

            //You become the other player's parent!
            otherPlayersGrabAndHoldScript.SetParent(transform);



            isHoldingOtherPlayer = true;

        }

        if(targetPlayer && Input.GetMouseButtonUp(0) && isHoldingOtherPlayer)
        {

            //Reverts the becoming of the other player's parent!
            otherPlayersGrabAndHoldScript.RemoveParent();
                    
            otherPlayersGrabAndHoldScript.isBeingGrabbed = false;

            isHoldingOtherPlayer = false;

            otherPlayersGrabAndHoldScript.RequestOwnerShip(photonView);

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
            if(grab.itemInHand)
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
        //This is where we set the potential target to null.
        other.GetComponent<GrabAndHold>()?.RemoveParent();

        targetPlayer = null;

        Debug.Log("Target player = true");

    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void RemoveParent()
    {
        transform.SetParent(null);
    }

    private void RequestOwnerShip(PhotonView requestingPlayerPhotonView)
    {
        originalClientPhotonViewID = requestingPlayerPhotonView;
        base.photonView.RequestOwnership();
        
    }

    private void RequestTransferOwnershipToOriginalHost()//PhotonView originalPlayerPhotonView)
    {
        //base.photonView.TransferOwnership(myPhotonViewID);
        photonView.TransferOwnership(myPhotonViewID.ViewID);
    }
}
