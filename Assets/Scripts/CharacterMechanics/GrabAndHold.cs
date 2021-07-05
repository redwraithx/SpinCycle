using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrabAndHold : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject targetPlayer = null;
    public GameObject grabber;
    private Grab grab = null;
    private StrengthBarUI strengthBar = null;
    private PlayerMovementCC playerCC = null;
    public CapsuleCollider grabCollider = null;
    internal bool isBeingGrabbed = false;
    internal bool isHoldingOtherPlayer = false;
    float playerSpeedWhenBeingGrabed;
    private bool isGettingOwnership = false;
    public float holdTimeDuration = 5f;
    public float currentHoldTimer = 0f;
    public bool isHoldTimerEnabled = false;
    public bool hasLostGripOfPlayer = false;

    PhotonView originalClientPhotonViewID = null;
    PhotonView myPhotonViewID = null;
    PhotonView originalTargetView = null;
    Player originalPlayer = null;
    GrabAndHold otherPlayersGrabAndHoldScript = null;
    RaycastHit hit;

    public string textString = "Sending";
    public string showTextString = "";
    public int counter = 0;



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

        currentHoldTimer = holdTimeDuration;

        //Physics.IgnoreCollision(grabCollider, GetComponent<CharacterController>());


        
        Physics.SphereCast(transform.position + new Vector3(0f, 0.5f, 0f), 0.5f, transform.forward, out hit, 0.5f);

        if(hit.collider != null)
           Debug.Log("can hit: " + hit.collider.name);
        
    }

    private void Update()
    {
        if (targetPlayer && !hasLostGripOfPlayer)
        {
            CheckGrab();
        }
        else if(targetPlayer && hasLostGripOfPlayer)
        {
            RelasedHeldPlayer();
        }
            

        if(isHoldTimerEnabled)
        {
            currentHoldTimer -= Time.deltaTime;

            if(currentHoldTimer <= 0f)
            {
                isHoldTimerEnabled = false;

                currentHoldTimer = holdTimeDuration;

                isBeingGrabbed = false;

                hasLostGripOfPlayer = true;
            }
        }

    }

    private void CheckGrab() 
    {
        otherPlayersGrabAndHoldScript = targetPlayer.GetComponent<GrabAndHold>();


        if (!otherPlayersGrabAndHoldScript)
            return;


        if (targetPlayer && Input.GetMouseButtonDown(0) && !isBeingGrabbed)
        {
            if (!targetPlayer)
                return;

            //isGettingOwnership = true;

            //otherPlayersGrabAndHoldScript.isBeingGrabbed = true;

            otherPlayersGrabAndHoldScript.BeingGrabbed();
            targetPlayer.GetComponent<PlayerMovementCC>().enemyGrab = grabber;

            //targetPlayer.GetComponent<PlayerMovementCC>().isGrabbed = true;

            //You become the other player's parent!
            //otherPlayersGrabAndHoldScript.SetParent(transform);

            isHoldingOtherPlayer = true;

        }

        if(targetPlayer && Input.GetMouseButtonUp(0) && isHoldingOtherPlayer)
        {

            RelasedHeldPlayer();
            targetPlayer.GetComponent<PlayerMovementCC>().enemyGrab = null;

            //otherPlayersGrabAndHoldScript.BeingReleased();

        }



    }

    public void BeingGrabbed()
    {
        isBeingGrabbed = true;

        GetComponent<PlayerMovementCC>().SlowDown();


        isHoldingOtherPlayer = true;

        currentHoldTimer = holdTimeDuration;

        isHoldTimerEnabled = true;

        
    }

    public void BeingReleased()
    {
        isHoldTimerEnabled = false;

        isHoldingOtherPlayer = false;

        //GetComponent<PlayerMovementCC>().SpeedUp();

        isBeingGrabbed = false;


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

    //private void RequestOwnerShip(PhotonView requestingPlayerPhotonView)
    //{
    //    originalClientPhotonViewID = requestingPlayerPhotonView;
    //    base.photonView.RequestOwnership();

    //}

    private void RequestTransferOwnershipToOriginalHost()//PhotonView originalPlayerPhotonView)
    {
        base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
        //photonView.TransferOwnership(originalClientPhotonViewID);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log($"Requestingownership by: {requestingPlayer.NickName} of PlayerID: {targetView.ViewID}");

        if (targetView != base.photonView)
            return;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if(targetView.OwnerActorNr == player.ActorNumber)
            {
                originalTargetView = targetView;
                originalPlayer = player;
            }
        }

        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView called");

        if (stream.IsWriting)
        {
            
            Debug.Log("Stream.IsWritting");

            stream.SendNext(textString);
            stream.SendNext(counter);
            showTextString = $"Sending: {counter}";

            counter++;

            stream.SendNext(isBeingGrabbed); 
            stream.SendNext(GetComponent<PlayerMovementCC>().isGrabbed);
            stream.SendNext(isHoldingOtherPlayer);
            stream.SendNext(currentHoldTimer);
            stream.SendNext(isHoldTimerEnabled);
            
        }
        else if (stream.IsReading)
        {
            Debug.Log("stream.IsReading");

            string newTextString = (string)stream.ReceiveNext();
            int newCounter = (int)stream.ReceiveNext();
            showTextString = $"Receiving: {newCounter}";

            bool beingGrabbed = (bool)stream.ReceiveNext();
            //if (beingGrabbed != isBeingGrabbed)
            //{
                isBeingGrabbed = beingGrabbed;
            //}

            bool grabbed = (bool)stream.ReceiveNext();
            //if (grabbed != GetComponent<PlayerMovementCC>().isGrabbed)
            //{
                GetComponent<PlayerMovementCC>().isGrabbed = grabbed;
            //}

            bool holdingOtherPlayer = (bool)stream.ReceiveNext();
            //if(holdingOtherPlayer != isHoldingOtherPlayer)
            //{
                isHoldingOtherPlayer = holdingOtherPlayer;
           //}

            currentHoldTimer = (float)stream.ReceiveNext();

            bool holdingTimerEnabled = (bool)stream.ReceiveNext();
            //if(holdingTimerEnabled != isHoldTimerEnabled)
            //{
                isHoldTimerEnabled = holdingTimerEnabled;
            //}


        }
    }

    public void RelasedHeldPlayer()
    {
        if (!targetPlayer)
            return; 
        //Reverts the becoming of the other player's parent!
        //otherPlayersGrabAndHoldScript.RemoveParent();

       //otherPlayersGrabAndHoldScript.isBeingGrabbed = false;

        //targetPlayer.GetComponent<PlayerMovementCC>().isGrabbed = false;


        isHoldingOtherPlayer = false;

        BeingReleased();


        //otherPlayersGrabAndHoldScript.OnOwnershipRequest(targetPlayer.GetPhotonView(), PhotonNetwork.LocalPlayer);
        //otherPlayersGrabAndHoldScript.RequestOwnerShip(photonView);


        //isGettingOwnership = false;

        hasLostGripOfPlayer = false;

        targetPlayer = null;
    }
}
