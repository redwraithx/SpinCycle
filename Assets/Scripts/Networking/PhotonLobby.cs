

using UnityEngine;
using Random = UnityEngine.Random;

using Photon.Pun;
using Photon.Realtime;


public class PhotonLobby : MonoBehaviourPunCallbacks
{
    // CHANGE THIS TO BE A REFERENCE FROM THE GAMEMANAGER
    public static PhotonLobby lobby;

    
    private RoomInfo[] rooms;

    
    public GameObject battleButton;
    public GameObject cancelButton;

    private void Awake()
    {
        // part of the SINGLETON
        lobby = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // connects to the Master photon server.
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        
        // this makes the clients connected sync to the master client (host) of this room. if they load a scene all clients will load it as well.
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // Player is now connected to the server
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle Button was clicked");
        
        battleButton.SetActive(false);
        cancelButton.SetActive(true);

        
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");

        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        string roomName = "Room" + randomRoomName;
        
        // trying to create a room with the specified values
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are now in a room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room wit ht esame name");

        CreateRoom();
        
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button was clicked");
        
        battleButton.SetActive(true);
        cancelButton.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }
}
