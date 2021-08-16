

using System;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class NetworkedPlayer : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public static GameObject LocalPlayerInstance = null; // this is your character when your playing

    public PlayerSphereCast playerSphereCastScript = null;
    public Grab grabScript = null;
    public PlayerMovementCC playerMovementCCScript = null;
    //public PlayerPoints playerPointsScript = null;
    public GameObject playerPointsGO = null;
    
    public Rigidbody rb = null;
    public Renderer playerMesh = null;

    public GameObject cameraBrainGO = null;
    public GameObject cameraBrainShouldGO = null;
    public GameObject camGO = null;

    public IntroCamera introCam = null;
    public GameObject dynamicCrossHair = null;
    
    //public CinemachineFreeLook cineFreeLook = null;

    public GameObject canvasGO = null;

    //public Transform targetPlayer = null;
    //public Text playerNameUI = null;


    private bool hasJoinedGame = false;
    bool introCamera = true;

    private void Start()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
            
            //playerMesh.material.color = Color.blue;
        }
        else
        {

            gameObject.name = "Network_Connected_Player";
            
            Debug.Log("Disabling player scripts & camera due to it not being the local players info");
            
            // this is a list of all the things the network players do not need to have only the local player.
            if (playerSphereCastScript)
                playerPointsGO.SetActive(false);
            else
                gameObject.GetComponent<PlayerSphereCast>().enabled = false;

            if (grabScript)
                grabScript.enabled = false;
            else
                gameObject.GetComponent<Grab>().enabled = false;

            if (playerMovementCCScript)
                playerMovementCCScript.enabled = false;
            else
                gameObject.GetComponent<PlayerMovementCC>().enabled = false;

            if (playerPointsGO)
                playerPointsGO.SetActive(false);
            else
                gameObject.GetComponent<PlayerPoints>().enabled = false;

            if (cameraBrainGO)
            {
                Debug.Log("cameraBrainGO reference found and disabling it");
                
                //cameraBrainGO.SetActive(false);
                Destroy(cameraBrainGO);
            }
            else
            {
                Debug.Log("no reference finding cameraBrainGO and disabling it");
                
                Destroy(gameObject.GetComponentInChildren<CinemachineFreeLook>().gameObject);
            }

            if (cameraBrainShouldGO)
                cameraBrainShouldGO.SetActive(false);

            if (camGO)
            {
                camGO.SetActive(false);

                //Destroy(camGO);
            }
            else
                gameObject.GetComponent<Camera>().gameObject.SetActive(false);

            if(canvasGO)
                canvasGO.SetActive(false);

            if (introCam)
                introCam.enabled = false;
            else
                gameObject.GetComponent<IntroCamera>().enabled = false;

            if (dynamicCrossHair)
                dynamicCrossHair.SetActive(false);
            else
                throw new Exception("Error! Network Player Character is missing dynamic cross hair reference in NetworkPlayer Script");


        }
    }



    
    // Photon callbacks
    
    
    // WARNING THERE IS TO BE ONLY ONE OF THIS PER INSTANCE OF THE GAME
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("trying to request ownership of object");
        
        // if item has a parent then its owned and can not be taken at this time.
        if (targetView != base.photonView)
            return;
        
        Debug.Log($"getting ownership of object for player: {requestingPlayer.NickName}");
        
        base.photonView.TransferOwnership(requestingPlayer);
    }

    // WARNING THERE IS TO BE ONLY ONE OF THIS PER INSTANCE OF THE GAME 
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("transferring ownership to prev player");
       // base.photonView.TransferOwnership(previousOwner);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log($"Error Transfering ownership has failed. From: {senderOfFailedRequest.NickName} <> To: {targetView.Controller.NickName}");
    }
}
