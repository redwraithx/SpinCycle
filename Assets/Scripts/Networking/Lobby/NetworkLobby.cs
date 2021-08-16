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
                Debug.Log("unlock and show cursor Func");
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
    
    
            if (!userNameField)
            {
                Debug.Log("get userNameField Ref");
                
                userNameField = LocalLobbyManager.localInstance.userNameField;
            }
    
            if (!roomNameField)
            {
                Debug.Log("get roomNameField Ref");
                
                roomNameField = LocalLobbyManager.localInstance.roomNameField;
            }
    
            if (!mapValue)
            {
                Debug.Log("get mapValue Ref");
                
                mapValue = LocalLobbyManager.localInstance.mapValueText;
            }
    
            if (!modeValue)
            {
                Debug.Log("get modeValue Ref");
                
                modeValue = LocalLobbyManager.localInstance.modeValueText;
            }
    
            if (!maxPlayersDropDown)
            {
                Debug.Log("Get maxPlayersDropDown Ref");
                
                maxPlayersDropDown = LocalLobbyManager.localInstance.maxPlayersDropDown;
            }
            
            
            if (!loadingLobby)
            {
                Debug.Log("get loadingLobby Ref");
                
                loadingLobby = LocalLobbyManager.localInstance.loadingLobbyUI;
    
                HideLoadingLobby();
            }
    
            if (!tabMain)
            {
                Debug.Log("get tabMain Ref");
                
                tabMain = LocalLobbyManager.localInstance.tabMainUI;
                
                tabMain.SetActive(false);
            }
    
            if (!tabRooms)
            {
                Debug.Log("get tabRooms Ref");
                
                tabRooms = LocalLobbyManager.localInstance.tabRoomsUI;
                
                tabRooms.SetActive(false);
            }
    
            if (!tabCreate)
            {
                Debug.Log("get tabCreate Ref");
                
                tabCreate = LocalLobbyManager.localInstance.tabCreateUI;
                
                tabCreate.SetActive(false);
            }
    
            Debug.Log($"references found <> loadingLobby: {loadingLobby}, tabMain: {tabMain}, tabRooms: {tabRooms}, tabCreate: {tabCreate}");
            if (loadingLobby && tabMain && tabCreate && tabRooms)
            {
                Debug.Log("got all references for network lobby");
                
                
                
                hasLobbyLoaded = true;
                
                ResetTabsForLobby();
                
                // load player data if it exists
                myProfile = DataClass.LoadProfile();
                if (myProfile != null && !string.IsNullOrEmpty(myProfile.userName))
                {
                    userNameField.text = myProfile.userName;
                }
                
                if(!PhotonNetwork.IsConnected)
                    Connect();
            }
        }
    }

    private bool CheckForLobbyHasLoaded() => !hasLobbyLoaded && SceneManager.GetActiveScene().buildIndex == networkLobbySceneIndex;

    
    
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server!");
        
        //PhotonNetwork.JoinLobby();
        
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
        
        Debug.Log($"you have joined: {PhotonNetwork.CurrentLobby.Name}");
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
        Debug.Log("Reset Tabs For Lobby Func");
        
        TabCloseAll();
        
        ShowLoadingLobby();
    }
    
    private void ShowLoadingLobby()
    {
        Debug.Log("Show Loading Lobby Func");
        
        loadingLobby.SetActive(true);
    }

    private void HideLoadingLobby()
    {
        Debug.Log("Hide Loading Lobby Func");
        
        loadingLobby.SetActive(false);
    }
    
    private void RemoveOldLobbyReferences()
    {
        Debug.Log("network lobby reference links are set to null");


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
        Debug.Log("disabling lobby tabs: Main, rooms, create");
        
        tabMain.SetActive(false);
        tabRooms.SetActive(false);
        tabCreate.SetActive(false);
    }

    public void StopCoroutineBeforeJoiningNewGame()
    {
        StopCoroutine(End(0.5f));
    }

    // public void TabOpenMain()
    // {
    //     TabCloseAll();
    //     tabMain.SetActive(true);
    // }

    // public void TabOpenRooms()
    // {
    //     TabCloseAll();
    //     tabRooms.SetActive(true);
    // }

    // public void TabOpenCreate()
    // {
    //     TabCloseAll();
    //     tabCreate.SetActive(true);
    //
    //     roomNameField.text = "";
    //
    //     currentMap = 0;
    //     mapValue.text = "Map: " + maps[currentMap].name.ToUpper();
    //
    //     GameSettings.GameMode = (GameModeSelections) 0;
    //     modeValue.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), (GameModeSelections) 0);
    //
    //     // GOING TO CHANGE THIS TO A DROP DOWN MENU I THINK
    //     //maxPlayersSlider.value = maxPlayersSlider.maxValue;
    //     
    //     // Players selection
    //     // int 0 = 1 vs 1
    //     // int 1 = 2 vs 2
    //     int playersSelection = maxPlayersDropDown.value;
    //
    //     if (playersSelection == 0)
    //     {
    //         Debug.Log("2 players selected");
    //         
    //         
    //     }
    //     // else
    //     //     Debug.Log("4");
    //     
    // }

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
        
        
        // // verify list is valid
        // foreach (RoomInfo roomInfo in roomList)
        // {
        //     if (roomInfo.PlayerCount == 0)
        //     {
        //         Debug.Log("removing dead room");
        //         
        //         roomList.Remove(roomInfo);
        //     }
        // }
        

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
            
            newRoomButton.GetComponent<RoomButtonInfo>().joinRoomButton.onClick.AddListener( delegate { JoinRoom(newRoomButton.transform); });
        }

        base.OnRoomListUpdate(roomList);
    }
    
    

    public void JoinRoom(Transform _button)
    {
        Debug.Log("Join Room Func Entered");
        
        //string _roomName = _button.Find("RoomName").GetComponent<TMP_Text>().text;
        string _roomName = _button.GetComponent<RoomButtonInfo>().roomName.text;
        
        Debug.Log("join room name: " + _roomName);
        
        VerifyUserName();

        RoomInfo roomInfo = null;
        Transform buttonParent = _button.parent;

        Debug.Log("button child count: " + buttonParent.childCount);
        
        for (int i = 0; i < buttonParent.childCount; i++)
        {
            if (buttonParent.GetChild(i).Equals(_button))
            {
                Debug.Log("roomInfo name: " + roomList[i].Name + ", found");
                
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

    // public void LeaveRoom(GameObject gameRoomGameObject)
    // {
    //     if (PhotonNetwork.CurrentRoom == null)
    //         return;
    //     
    //     // need to add functionality here to leave a room you maybe in or hosting
    //     Debug.Log("Connection status: " + PhotonNetwork.CurrentRoom);
    //     
    //     PhotonNetwork.LeaveRoom();
    //     
    //     Destroy(gameRoomGameObject);
    // }
    //
    // public void LeaveRoomList()
    // {
    //     Debug.Log("leave room list func entered");
    //     
    //     if (PhotonNetwork.CurrentRoom == null)
    //         return;
    //
    //     Debug.Log("leaving room now");
    //     
    //     PhotonNetwork.LeaveRoom();
    // }

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
            Debug.Log("starting game");
            
            DataClass.SaveProfile(myProfile);
            
            PhotonNetwork.LoadLevel(maps[currentMap].sceneBuildIndex);
        }
    }
        
    
    
    // this is not tested
    private void EndGame()
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
