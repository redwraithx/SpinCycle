


using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TESTOnly : MonoBehaviourPunCallbacks
{
    public GameObject gameOverContainer = null;
    public GameObject waitForItContainer = null;

    public int LevelGameLevelToLoad = 0;
    
    public bool isTheGameOver = false;

    public bool hasLevelBeenLoaded = false; 

    private void Start()
    {
        Debug.Log("Photon player: " + PhotonNetwork.LocalPlayer.ActorNumber);

        gameOverContainer.SetActive(false);
        waitForItContainer.SetActive(true);
        
        
    }


    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            isTheGameOver = true;

        if (isTheGameOver)
        {
            gameOverContainer.SetActive(true);
            waitForItContainer.SetActive(false);

            if (!hasLevelBeenLoaded && PhotonNetwork.IsMasterClient)
            {
                hasLevelBeenLoaded = true;
                
                LoadGameLevel();
            }    
        }
    }

    // public void GoToMainMenu()
    // {
    //     GameObject networkObj = GameObject.FindWithTag("NetworkManager");
    //
    //     if (networkObj)
    //         Destroy(networkObj);
    //
    //     //SceneManager.LoadScene(0); // 0 is title scene
    //     
    //     if(PhotonNetwork.IsMasterClient)
    //         PhotonNetwork.LoadLevel(0);
    // }
    

    public void QuitApp()
    {
        Application.Quit();
    }

    public void LoadGameLevel()
    {
        PhotonNetwork.LoadLevel(LevelGameLevelToLoad);
    }
}
