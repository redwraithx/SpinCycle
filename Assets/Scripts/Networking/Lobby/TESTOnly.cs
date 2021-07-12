


using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TESTOnly : MonoBehaviourPun
{
    public GameObject gameOverContainer = null;
    public GameObject waitForItContainer = null;
    
    
    public bool isTheGameOver = false;

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
        }
    }

    public void GoToMainMenu()
    {
        GameObject networkObj = GameObject.FindWithTag("NetworkManager");

        if (networkObj)
            Destroy(networkObj);

        SceneManager.LoadScene(0); // 0 is title scene
    }
    

    public void QuitApp()
    {
        Application.Quit();
    }
    
    
}
