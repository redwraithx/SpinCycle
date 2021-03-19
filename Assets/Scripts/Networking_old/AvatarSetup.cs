using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MAKE SURE TO SIDABLE THE MESH RENDERER OF THE PLAYER IF YOUR REPLACING THE DEFAULT CHARACTER WITH THE NEW ONE.
public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
        }
        
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        // if we use changable characters
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
        
    }
}
