

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;




public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room info
    public static PhotonRoom room;
    private PhotonView _photonView;

    //public bool isGameLoaded;
    public int currentScene;
    public int multiplayerScene;
    
    // Player Info
    //private Player[] photonPlayers;
    //public int playersInRoom;
    //public int myNumberInRoom;

    //public int playersInGame;
    
    // Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private const int MAX_PLAYERS_PER_GAME = 2;
    private float timeToStart;



    private void Awake()
    {
        // SINGLETON AGAIN THIS SHOULD BE ALL CALLED FROM GAME MAANGER ONCE THINGS ARE SETUP
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);

                PhotonRoom.room = this;
            }
        }
        
        DontDestroyOnLoad(this.gameObject);
        _photonView = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        // subscribe to functions
        base.OnEnable();

        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

        //
        //PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnDisable()
    {
        // unsubscribe to functions
        base.OnDisable();
        
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        
        
    }
    
    // Initialization
    private void Start()
    {
        // set private variables
        _photonView = GetComponent<PhotonView>();

        // readyToCount = false;
        // readyToStart = false;
        // lessThanMaxPlayers = startingTime;
        // atMaxPlayers = MAX_PLAYERS_PER_GAME;
        // timeToStart = startingTime;
        
    }

    private void Update()
    {
        // delays start only
        // if (MultiplayerSetting.multiplayerSetting.delayStart)
        // {
        //     if (playersInRoom == 1)
        //     {
        //         RestartTimer();
        //     }
        //
        //     if (!isGameLoaded)
        //     {
        //         if (readyToStart)
        //         {
        //             atMaxPlayers -= Time.deltaTime;
        //             lessThanMaxPlayers = atMaxPlayers;
        //             timeToStart = atMaxPlayers;
        //         }
        //         else if (readyToCount)
        //         {
        //             lessThanMaxPlayers -= Time.deltaTime;
        //             timeToStart = lessThanMaxPlayers;
        //         }
        //         
        //         Debug.Log("Display time to start to the players " + timeToStart);
        //
        //         if (timeToStart <= 0f)
        //         {
        //             StartGame();
        //         }
        //         
        //         
        //     }
        //     
        // }


    }

    public override void OnJoinedRoom()
    {
        // sets player data when we join the room
        base.OnJoinedRoom();
        
        Debug.Log("We are now in a room");

        // optional lines of code
        //photonPlayers = PhotonNetwork.PlayerList;
        //playersInRoom = photonPlayers.Length;
        //myNumberInRoom = playersInRoom;
        //PhotonNetwork.NickName = myNumberInRoom.ToString(); // this will be changed to the players name after concept is working
        
        // delayed start stuff
        // if (MultiplayerSetting.multiplayerSetting.delayStart)
        // {
        //     Debug.Log("display players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.delayStart);
        //
        //     if (playersInRoom > 1)
        //     {
        //         readyToCount = true;
        //     }
        //
        //     if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
        //     {
        //         readyToStart = true;
        //
        //         if (!PhotonNetwork.IsMasterClient)
        //         {
        //             return;
        //         }
        //
        //         PhotonNetwork.CurrentRoom.IsOpen = false;
        //     }
        //     else
        //     {
                 StartGame();
        //     }
        // }
        
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // updates player data when a new player joins
        base.OnPlayerEnteredRoom(newPlayer);
        
        Debug.Log("A new player has joined the room");

        //photonPlayers = PhotonNetwork.PlayerList;
        //playersInRoom++;
        
        // delayed start
        // if (MultiplayerSetting.multiplayerSetting.delayStart)
        // {
        //     Debug.Log("Display players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.delayStart);
        //
        //     if (playersInRoom > 1)
        //     {
        //         readyToCount = true;
        //     }
        //
        //     if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
        //     {
        //         readyToStart = true;
        //
        //         if (!PhotonNetwork.IsMasterClient)
        //             return;
        //
        //         PhotonNetwork.CurrentRoom.IsOpen = false;
        //     }
        //     
        // }


    }

    private void StartGame()
    {
        // loads the multiplayer scene for all players
        //isGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        /*if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        */

        // NEED TO ADD THE ACTUAL LEVEL TO LOAD 1 will not be valid
        PhotonNetwork.LoadLevel(multiplayerScene);

        

    }

    private void RestartTimer()
    {
        // restarts the tim for when players leave the room during the Delayed start of a game
        // lessThanMaxPlayers = startingTime;
        // timeToStart = startingTime;
        // atMaxPlayers = MAX_PLAYERS_PER_GAME;
        // readyToCount = false;
        // readyToStart = false;
        
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // called when multiplayer scene is loaded

        currentScene = scene.buildIndex;

        if (currentScene == multiplayerScene)
        {
            //isGameLoaded = true;
            
            // for delayed start game
            // if (MultiplayerSetting.multiplayerSetting.delayStart)
            // {
            //     _photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            //     
            // }
            // else // non delayed start of game
            // {
                CreatePlayer();
            //}
 
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        //playersInGame++;

        //if (playersInGame == PhotonNetwork.PlayerList.Length)
        //{
            _photonView.RPC("RPC_CreatePlayer", RpcTarget.All);

        //}
    }

    [PunRPC]
    private void CreatePlayer()
    {
        // creates player network controller, but not the player as well as the datagram group
        PhotonNetwork.Instantiate(Path.Combine("PhotonPlayerPrefabs", "PhotonNetworkPlayer"), new Vector3(17.5f, 12f, 7f), Quaternion.identity, 0);
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        
        Debug.Log(otherPlayer.NickName + " has left the game");

        //playersInRoom--;
    }
    

}
