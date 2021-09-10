using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class IntroCamera : MonoBehaviourPun
{
    public GameObject freeLook;
    public float initTime = 0f;
    //public PhotonView photonView = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "LobbyWaitingRoomScene" || scene.name == "TutorialLevel")
        {
            freeLook.SetActive(true);
        }
        else
        {
            freeLook.SetActive(false);
        }

        // if(!photonView)
        //     photonView =  
    }

    // Update is called once per frame
    void Update()
    {

        //use this function once networking is active again
        /*        if(!freeLook.activeSelf)
                {
                    if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime < 180)
                    {
                        freeLook.SetActive(true);
                    }
                }*/

        initTime += Time.deltaTime;
        if (initTime >= 5f)
        {
            if(photonView.IsMine)
                freeLook.SetActive(true);
        }

    }
}

