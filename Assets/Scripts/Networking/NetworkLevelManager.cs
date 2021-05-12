
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class NetworkLevelManager : MonoBehaviourPunCallbacks
{
    public Transform[] playerStartingPositions;

    public List<GameObject> playersJoined;

    private bool playersFound = false;

    //public bool[] isPlayersDiveDelayEnabled = new bool[GameManager.networkManager.maxPlayersPerRoom];
    //public float initialDiveReuseDelay = 10f;

    private void Awake()
    {
        GameManager.networkLevelManager = this;

        Vector3 startingPosition = Vector3.zero;
        Quaternion startingRotation = Quaternion.identity;
        GameObject networkedPlayer = null;

        // if main camera is in scene disable it
        //if (GameObject.FindWithTag("MainCamera"))
        foreach (var cam in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
            //GameObject.FindWithTag("MainCamera").gameObject.SetActive(false);
            cam.SetActive(false);
        }

        // we can add character selection here if / when we get that.
        //PlayerPrefs.GetInt("PlayerSelection");  // something like this if we had a few characters we could have them set by an int


        if (PhotonNetwork.IsConnected)
        {
            int newPlayerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;

            startingPosition = playerStartingPositions[newPlayerIndex].position;
            startingRotation = playerStartingPositions[newPlayerIndex].rotation;

            if (NetworkedPlayer.LocalPlayerInstance == null)
            {


                // spawn networked Player
                networkedPlayer = PhotonNetwork.Instantiate(Path.Combine("NetworkingPrefabs", "NetworkedPlayerAvatar"), startingPosition, startingRotation, 0);

                networkedPlayer.GetComponent<PlayerMovementCC>().playerDiveIndex = newPlayerIndex;

                networkedPlayer.name = PhotonNetwork.NickName;


                Debug.Log($"spawning player {networkedPlayer.name}");


                playersJoined.Add(networkedPlayer);


            }

            if (PhotonNetwork.IsMasterClient)
            {
                // if we need the host to setup the level it will happen here
                // game timers etc...
                // we may want to wait for other players till the room lobby has been setup
                // if (PhotonNetwork.CurrentRoom.MaxPlayers == 2)
                // {
                //     // join room and force all clients to join as well
                // }
            }


        }
        else
        {
            // if we had a single player game it would go here.
            // set rotation and starting position


        }

        //networkedPlayer.GetComponent<PlayerMovement>().enabled = true;
        if (playersJoined.Count < 2)
        {
            StartCoroutine(FindPlayers());
        }

    }

    IEnumerator FindPlayers()
    {
        yield return new WaitForSeconds(10f);
        while (playersFound == false)
        {

            foreach (var Player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (!playersJoined.Contains(Player))
                {
                    playersJoined.Add(Player);
                }
                else
                {
                    playersFound = true;
                }
            }
            Debug.Log("Routine is running");
        }
    }

}
