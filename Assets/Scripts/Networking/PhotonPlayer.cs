using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView _photonView;
    private static int count = 0;

    public GameObject myAvatar;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();


        if (_photonView.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPlayerPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[count].position, GameSetup.GS.spawnPoints[count].rotation, 0);
            
            
        }
        
        
    }
    
    
}
