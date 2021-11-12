


using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LobbyPlayerCounter : MonoBehaviourPunCallbacks
{
    public GameObject gameOverContainer = null;
    public GameObject waitForItContainer = null;
    public GameObject loadingContainer = null;

    public int LevelGameLevelToLoad = 0;

    public bool isTheGameOver = false;

    public bool hasLevelBeenLoaded = false;

    public float startTime;
    public bool timerGoing;
    public TMP_Text ticker;

    public int maxPlayers;
    public int playersReady;
    public bool clientReady = false;

    private void Start()
    {

        startTime = 5;
        timerGoing = false;
        Debug.Log("Photon player: " + PhotonNetwork.LocalPlayer.ActorNumber);

        gameOverContainer.SetActive(false);
        waitForItContainer.SetActive(true);


    }


    private void Update()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        //    timerGoing = true;

        if (timerGoing == true)
        {
            if(playersReady < maxPlayers)
            {
                ticker.gameObject.GetComponent<AudioSource>().Stop();
                ticker.gameObject.SetActive(false);
                startTime = 5;
                timerGoing = false;
            }

            ticker.gameObject.SetActive(true);
            ticker.text = Mathf.Round(startTime).ToString();
            startTime -= Time.deltaTime;
            if (startTime <= 0)
            {
                isTheGameOver = true;
                timerGoing = false;
            }
        }

        if(timerGoing == false && ticker.gameObject.activeInHierarchy == true)
        {
            ticker.gameObject.GetComponent<AudioSource>().Stop();
            ticker.gameObject.SetActive(false);
        }

        if (isTheGameOver)
        {
            //gameOverContainer.SetActive(true);
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
        loadingContainer.SetActive(true);
        PhotonNetwork.LoadLevel(LevelGameLevelToLoad);
    }

    public void ReadyButton()
    {
        Debug.Log("Ready Button Hit");

        if (clientReady == false)
        {
            playersReady += 1;
            clientReady = true;
        }
        else if (clientReady == true)
        {
            playersReady -= 1;
            clientReady = false;
        }


        if(playersReady >= maxPlayers)
        {
            timerGoing = true;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playersReady);
        }
        if (stream.IsReading)
        {
            float ReadyPlayers = (int) stream.ReceiveNext();

            if (ReadyPlayers != playersReady)
                playersReady = (int)ReadyPlayers;
        }
    }
}
