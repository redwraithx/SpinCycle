using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using NetworkLobbyGameSettings;
using NetworkProfile;
using NetworkMaps;
using PlayerProfileData;

using Photon.Pun;
using Photon.Realtime;
//using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;




public class NetworkLobby : MonoBehaviourPunCallbacks
{
    internal static NetworkLobby networkManager = null;
    
    [Min(-1)]public int networkLobbySceneIndex = -1;
    
    public bool perpetual = false;
    public float waitTimeTillReturningToMainMenu = 6f;
    
    public TMP_InputField userNameField;
    public TMP_InputField roomNameField;
    public TMP_Text mapValue;
    public TMP_Text modeValue;
    public TMP_Dropdown maxPlayersDropDown;
    //public Text maxPlayersValue;
    public static ProfileData myProfile = new ProfileData();
    
    public GameObject loadingLobby = null;

    public GameObject tabMain;
    public GameObject tabRooms;
    public GameObject tabCreate;

    public GameObject buttonRoom;

    public MapData[] maps;
    public int currentMap = 0;

    [SerializeField] public List<RoomInfo> roomList;

    [SerializeField] private bool hasLobbyLoaded = false;

    [SerializeField] private bool hasSuccessfullyLeftCurrentRoom = true;
    
    public void Awake()
    {
        
        
        //if (GameManager.networkManager)
        if(networkManager)
        {
            Debug.Log("network lobby instance exists, destroying this copy");
            
            DestroyImmediate(gameObject);

            return;
        }
        else
        {
            Debug.Log("Setting network lobby instance");
            
            DontDestroyOnLoad(this);

            networkManager = this;
        
            GameManager.networkManager = networkManager;
        }
        
        
        
        
        
        if (networkLobbySceneIndex == -1)
        {
            networkLobbySceneIndex = SceneManager.GetActiveScene().buildIndex;

            //throw new Exception("Error! networkLobbySceneIndex for NetworkLobby script has not been set. This must be the buildIndex of the lobby Scene Index in build settings");
        }
        
        
        PhotonNetwork.AutomaticallySyncScene = true;
        

        //
        // myProfile = DataClass.LoadProfile();
        // if (!string.IsNullOrEmpty(myProfile.userName))
        // {
        //     userNameField.text = myProfile.userName;
        //     
        // }
        //
        // update current map index
        currentMap = SceneManager.GetActiveScene().buildIndex;

        //Connect();
    }

    private void Update()
    {
        if (CheckForLobbyHasLoaded())
        {
            //StopCoroutine(End(1f));
            
            if (Cursor.lockState == CursorLockMode.Locked || Cursor.visible == false)
            {
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
    
    
            if (!userNameField)
            {

                
                userNameField = LocalLobbyManager.localInstance.userNameField;
            }
    
            if (!roomNameField)
            {

                
                roomNameField = LocalLobbyManager.localInstance.roomNameField;
            }
    
            if (!mapValue)
            {

                
                mapValue = LocalLobbyManager.localInstance.mapValueText;
            }
    
            if (!modeValue)
            {

                
                modeValue = LocalLobbyManager.localInstance.modeValueText;
            }
    
            if (!maxPlayersDropDown)
            {

                
                maxPlayersDropDown = LocalLobbyManager.localInstance.maxPlayersDropDown;
            }
            
            
            if (!loadingLobby)
            {

                
                loadingLobby = LocalLobbyManager.localInstance.loadingLobbyUI;
    
                HideLoadingLobby();
            }
    
            if (!tabMain)
            {

                
                tabMain = LocalLobbyManager.localInstance.tabMainUI;
                
                tabMain.SetActive(false);
            }
    
            if (!tabRooms)
            {

                
                tabRooms = LocalLobbyManager.localInstance.tabRoomsUI;
                
                tabRooms.SetActive(false);
            }
    
            if (!tabCreate)
            {

                
                tabCreate = LocalLobbyManager.localInstance.tabCreateUI;
                
                tabCreate.SetActive(false);
            }
    
            if (loadingLobby && tabMain && tabCreate && tabRooms)
            {

                
                
                
                hasLobbyLoaded = true;
                
                ResetTabsForLobby();
                
                // load player data if it exists
                myProfile = DataClass.LoadProfile();
                if (myProfile != null && !string.IsNullOrEmpty(myProfile.userName))
                {
                    userNameField.text = myProfile.userName;

                    PhotonNetwork.LocalPlayer.NickName = myProfile.userName;
                }
                
                if(!PhotonNetwork.IsConnected)
                    Connect();
            }
        }
    }

    private bool CheckForLobbyHasLoaded() => !hasLobbyLoaded && SceneManager.GetActiveScene().buildIndex == networkLobbySceneIndex;

    
    
    
    public override void OnConnectedToMaster()
    {
        
        
        JoinLobby();
        
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        RemoveOldLobbyReferences();
        
        while(!hasSuccessfullyLeftCurrentRoom)
            Debug.Log("waiting on last room left");
        
        //PhotonNetwork.LoadLevel(networkLobbySceneIndex);

        JoinLobby();
        
        SceneManager.LoadScene(networkLobbySceneIndex);
    }

    public void Connect()
    {
        Debug.Log("connect to master server...");

        PhotonNetwork.GameVersion = "0.0.0"; // we need to set this up
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        // is the name empty or have less then 3 characters?
        if (string.IsNullOrEmpty(roomNameField.text) || roomNameField.text.Length < 3)
            return;
        
        
        RoomOptions options = new RoomOptions();
    
        // Assign max players here
        int currentGamesMaxPlayers = 1;
        int playersSelection = maxPlayersDropDown.value;
        
        if (playersSelection == 0)
        {
            currentGamesMaxPlayers = 2;
        }
        else// if (playersSelection == 1)
        {
            currentGamesMaxPlayers = 1;
        }
        
        options.MaxPlayers = (byte) currentGamesMaxPlayers;
    
        options.CustomRoomPropertiesForLobby = new string[]
        {
            "map", 
            "mode"
        };
    
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("map", currentMap);
        properties.Add("mode", (int) GameSettings.GameMode);
        options.CustomRoomProperties = properties;
    
        PhotonNetwork.CreateRoom(roomNameField.text, options);
    }

    // public void ChangeMap()
    // {
    //     currentMap++;
    //
    //     if (currentMap >= maps.Length)
    //         currentMap = 0;
    //
    //     mapValue.text = "Map: " + maps[currentMap].name.ToUpper();
    // }

    // public void ChangeMode()
    // {
    //     int newMode = (int) GameSettings.GameMode + 1;
    //
    //     if (newMode >= System.Enum.GetValues(typeof(GameModeSelections)).Length) 
    //         newMode = 0;
    //
    //     GameSettings.GameMode = (GameModeSelections) newMode;
    //
    //     modeValue.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), newMode);
    // }

    // public void ChangeMaxPlayersSlider(float _value)
    // {
    //     maxPlayersValue.text = Mathf.RoundToInt(_value).ToString();
    // }
    
    private void ResetTabsForLobby()
    {
        
        TabCloseAll();
        
        ShowLoadingLobby();
    }
    
    private void ShowLoadingLobby()
    {
        
        loadingLobby.SetActive(true);
    }

    private void HideLoadingLobby()
    {
        
        loadingLobby.SetActive(false);
    }
    
    private void RemoveOldLobbyReferences()
    {


        userNameField = null;
        roomNameField = null;
        mapValue = null;
        modeValue = null;
        maxPlayersDropDown = null;
        
        loadingLobby = null;
        tabMain = null;
        tabCreate = null;
        tabRooms = null;

        hasLobbyLoaded = false;

        
        //roomList.Clear();
    }
    
    
    
    

    private void TabCloseAll()
    {
        
        tabMain.SetActive(false);
        tabRooms.SetActive(false);
        tabCreate.SetActive(false);
    }

    public void StopCoroutineBeforeJoiningNewGame()
    {
        StopCoroutine(End(0.5f));
    }



    private void ClearRoomList()
    {
        if (!tabRooms)
            return;
            
        Transform content = tabRooms?.transform.Find("Scroll View/Viewport/Content");

        if (content)
        {
            foreach (Transform item in content)
                Destroy(item.gameObject);
        }
    }

    private void VerifyUserName()
    {
        if (string.IsNullOrEmpty(userNameField.text))
        {
            myProfile.userName = "User_" + UnityEngine.Random.Range(100, 10000);
        }
        else
        {
            myProfile.userName = userNameField.text;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> list)
    {
        if (list.Count <= 0)
            return;
        
        if (!loadingLobby || !tabMain || !tabCreate || !tabRooms)
            return;
        
        // update list of rooms?
        roomList = list;
        ClearRoomList();
        
       
        

        Transform content = tabRooms?.transform.Find("Scroll View/Viewport/Content");

        // if (roomList.Count <= 0)
        //     return;
        
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList == false)
            {
                Debug.Log($"room [ {room.Name} ] is open");
            }
            else
            {
                Debug.Log($"room [ {room.Name} ] is closed");

                continue;
            }
            
            GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;

            newRoomButton.transform.GetComponent<RoomButtonInfo>().roomName.text = room.Name;
            
            //int playersSelection = maxPlayersDropDown.value;

            newRoomButton.transform.GetComponent<RoomButtonInfo>().playersCounter.text = room.PlayerCount + " / " + room.MaxPlayers;
            
            // send max players through network event
            
            
            if (room.CustomProperties.ContainsKey("map"))
                newRoomButton.transform.GetComponent<RoomButtonInfo>().mapName.text = maps[(int) room.CustomProperties["map"]].name;
            else
                newRoomButton.transform.GetComponent<RoomButtonInfo>().mapName.text = "-----";
            
            Debug.Log("new room buttons name: " + newRoomButton.name);
            
            newRoomButton.GetComponent<RoomButtonInfo>().joinRoomButton.onClick.AddListener( 
                delegate 
                {
                    JoinGameButtonClick(newRoomButton.GetComponent<RoomButtonInfo>(), newRoomButton);
                });
        }

        base.OnRoomListUpdate(roomList);
    }

    public void JoinGameButtonClick(RoomButtonInfo roomButtonInfo, GameObject newRoomButton)
    {
        if (roomButtonInfo.hasBeenClicked == false)
        {
            Debug.Log("delegate used and joining game and updating button has been clicked");
                        
            roomButtonInfo.hasBeenClicked = true;
                        
            JoinRoom(newRoomButton.transform);
        }
    }
    
    

    public void JoinRoom(Transform _button)
    {
        
        //string _roomName = _button.Find("RoomName").GetComponent<TMP_Text>().text;
        string _roomName = _button.GetComponent<RoomButtonInfo>().roomName.text;
        
        
        VerifyUserName();

        RoomInfo roomInfo = null;
        Transform buttonParent = _button.parent;

        
        for (int i = 0; i < buttonParent.childCount; i++)
        {
            if (buttonParent.GetChild(i).Equals(_button))
            {
                
                roomInfo = roomList[i];
                break;
            }
        }

        if (roomInfo != null)
        {
            LoadGameSettings(roomInfo);
            PhotonNetwork.JoinRoom(_roomName);
        }
    }

  

    public void LoadGameSettings(RoomInfo roomInfo)
    {
        if(roomInfo.PlayerCount == 0)
            roomList.Remove(roomInfo);
        
        GameSettings.GameMode = (GameModeSelections)roomInfo.CustomProperties["mode"];
    }

    public void StartGame()
    {
        VerifyUserName();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            
            DataClass.SaveProfile(myProfile);
            
            PhotonNetwork.LoadLevel(maps[currentMap].sceneBuildIndex);
        }
    }
        
    
    
    // this is not tested
    private void EndGame() // maybe deleting soon
    {
        // set state of game
        // state  = GameState.Ending;
        
        
        // set timer to 0 if networked
        
        
        // update timer UI if needed
        
        
        
        // disable Room
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();

            if (!perpetual)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        
        // could show end game UI here if we want
        
        
        // wait X seconds then return to main menu
        StartCoroutine(End(waitTimeTillReturningToMainMenu));
    }
    
    
    
    #region Coroutine_Methods
    
    private IEnumerator End(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (perpetual) // new game?
        {
            Debug.Log("perpetual");
            
            // new match
            // if (PhotonNetwork.IsMasterClient)
            // {
            //     NewMatch_Send();
            // }
        }
        else
        {
            // disconnect
            //PhotonNetwork.AutomaticallySyncScene = false;

            LeavingGame();
        }
    }
    
    #endregion // Coroutine_Methods


    internal void LeavingGame()
    {
        StopCoroutine(DisconnectAndLoad());
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.CurrentRoom.ClearExpectedUsers();
        } 
        
        //PhotonNetwork.LeaveRoom(false);
        //PhotonNetwork.LeaveLobby();

        //PhotonNetwork.Disconnect();

        
        StartCoroutine(DisconnectAndLoad());
        
    }


    private IEnumerator DisconnectAndLoad()
    {
        hasSuccessfullyLeftCurrentRoom = false;
        
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        
        if(PhotonNetwork.InRoom)
            Debug.Log($"**** BEFORE ****   should be out of the game room you were int, current room name is: {PhotonNetwork.CurrentRoom.Name}");
        
        //while (PhotonNetwork.IsConnected)
        while(PhotonNetwork.InRoom)
            yield return null;
        
        
        if(PhotonNetwork.InRoom)
            Debug.Log($"**** AFTER ****   should be out of the game room you were int, current room name is: {PhotonNetwork.CurrentRoom.Name}");
        // can load level here if on room left is not used

        hasSuccessfullyLeftCurrentRoom = true;
    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }
}
