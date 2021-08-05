
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NetworkLevelManager : MonoBehaviourPun
{
    public Transform[] playerStartingPositions;

    public List<GameObject> playersJoined;

    public GameObject timer;

    bool timerStarted = false;

    public Text player1;

    public Text player2;


    

    //public bool[] isPlayersDiveDelayEnabled = new bool[GameManager.networkManager.maxPlayersPerRoom];
    //public float initialDiveReuseDelay = 10f;

    private void Awake()
    {
        GameManager.networkLevelManager = this;

        Vector3 startingPosition = Vector3.zero;
        Quaternion startingRotation = Quaternion.identity;
        GameObject networkedPlayer = null;

        // if main camera is in scene disable it
        foreach (var cam in GameObject.FindGameObjectsWithTag("MainCamera"))
            Destroy(cam);


        // we can add character selection here if / when we get that.
        //PlayerPrefs.GetInt("PlayerSelection");  // something like this if we had a few characters we could have them set by an int


        if (PhotonNetwork.IsConnected)
        {
            //int newPlayerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            int newPlayerIndex = playersJoined.Count;

            startingPosition = playerStartingPositions[newPlayerIndex].position;
            startingRotation = playerStartingPositions[newPlayerIndex].rotation;

            if (NetworkedPlayer.LocalPlayerInstance == null)
            {
                // spawn networked Player
                networkedPlayer = PhotonNetwork.Instantiate(Path.Combine("NetworkingPrefabs", "NetworkedPlayerAvatar"), startingPosition, startingRotation, 0);

                networkedPlayer.GetComponent<PlayerMovementCC>().playerDiveIndex = newPlayerIndex; // what is this for?
            }

            if (PhotonNetwork.IsMasterClient)
            {
                // time before an empty room is destroyed in seconds
                PhotonNetwork.CurrentRoom.EmptyRoomTtl = 1;
            }

        }

    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (!timerStarted)
                    {
                        PhotonNetwork.CurrentRoom.IsOpen = false;
                        
                        timer.GetComponent<NetworkedTimerNew>().InitializeTimer();
                        timerStarted = true;
                    }
                }

                if (playersJoined.Count <= 2)
                {
                    foreach (var Player in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (!playersJoined.Contains(Player))
                        {
                            if(Player.GetComponent<PhotonView>().Owner.IsMasterClient == true && playersJoined.Count >= 0)
                            {
                                playersJoined.Add(Player);
                            }
                            else if(Player.GetComponent<PhotonView>().Owner.IsMasterClient == false && playersJoined.Count >= 1)
                            {
                                playersJoined.Add(Player);
                            }
                            else
                            {
                                Debug.Log("having trouble finding player 2");
                            }


                        }
                    }
                }
            }
        }
    }

}
