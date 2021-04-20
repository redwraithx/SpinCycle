
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    internal byte maxPlayersPerRoom = 4;  // THIS MUST BE CHANGED FOR RELEASE
    private bool isConnecting = false;

    public TMP_Text feedbackText = null;
    
    public TMP_InputField playerName = null;

    public Button connectButton = null;
    private bool hasClickedJoinButton = false;

    private string gameVersion = "1";


    private void Awake()
    {
        // does the networking manager already exist on the GameManager?
        if (GameManager.networkManager)
        {
            DestroyImmediate(gameObject);

            return;
        }
        
        
        GameManager.networkManager = this;
            
        DontDestroyOnLoad(this);

        
        // photon async loading of scenes for all who join
        PhotonNetwork.AutomaticallySyncScene = true;


    }
    

    // this will happen on button press
    public void ConnectNetwork()
    {
        if (hasClickedJoinButton)
            return;

        hasClickedJoinButton = true;
        
        Debug.Log("called connect Network func");
        
        feedbackText.text = "";
        isConnecting = true;

        // sets player nickname to name selected.
        if (string.IsNullOrEmpty(playerName.text) || string.IsNullOrWhiteSpace(playerName.text))
        {
            hasClickedJoinButton = false;
            
            feedbackText.text = "You need to Enter a player name to continue";
            
            return;
        }
            
        
        if (playerName.text != PlayerPrefs.GetString("PlayerName"))
        {
            feedbackText.text += "\nSaving Player Name";
            SetName(playerName.text);
        }
        
        
        PhotonNetwork.NickName = playerName.text;

        if (PhotonNetwork.IsConnected)
        {
            feedbackText.text += "\nJoining Room...";
            PhotonNetwork.JoinRandomRoom();
            
        }
        else
        {
            feedbackText.text += "\nConnecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            
        }
        
        // if we have a player name set it. else we need to get and set it
        if (PlayerPrefs.HasKey("PlayerName"))
            playerName.text = PlayerPrefs.GetString("PlayerName");
        
    }

    
    private void FindJoinGameObjectsInLobby()
    {
        if (SceneManager.GetActiveScene().buildIndex != 5) //"BasicLobbyRoom")
            return;

        if (!connectButton)
        {
            hasClickedJoinButton = false;
            
            Debug.Log("found button");
            connectButton = GameObject.FindWithTag("JoinThisGameButton").GetComponent<Button>();

            connectButton.onClick.AddListener(ConnectNetwork);
        }
        

        if(!feedbackText)
            feedbackText = GameObject.FindWithTag("ConnectionFeedbackInfoText").GetComponent<TMP_Text>();

        if (!playerName)
        {
            playerName = GameObject.FindWithTag("YourPlayersNameInputBox").GetComponent<TMP_InputField>();

            if(playerName)
                playerName.text = GetName();
        }
        
    }
    
    
    

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public string GetName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    private void Update()
    {
        FindJoinGameObjectsInLobby();
        
        
        
    }


    #region Network_CallBacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (isConnecting )
        {
            Debug.Log("connected to master server");
            
            feedbackText.text += "\nOnConnectedToMaster...";
            PhotonNetwork.JoinRandomRoom();
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("failed to join random room, creating new room");
        
        feedbackText.text += "\nFailed to join random room, Creating new Room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }
    
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        //
        
        
        
        if(feedbackText)
            feedbackText.text += $"\n{PhotonNetwork.NickName} has Disconnected because: " + cause;
        else
        {
            Debug.Log($"{PhotonNetwork.NickName} has disconnected from the game");
        }

        isConnecting = false;
    }


    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("Joined room");
        
        feedbackText.text += "\nJoined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players.";
        
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(6); 
        
    }

    #endregion
    
}
