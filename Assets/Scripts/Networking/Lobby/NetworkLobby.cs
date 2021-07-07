using System.Collections.Generic;
using UnityEngine;
using TMPro;

using NetworkLobbyGameSettings;
using NetworkProfile;
using NetworkMaps;
using Data;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;



namespace NetworkProfile
{
    [System.Serializable]
    public class ProfileData
    {
        public string userName;

        public ProfileData()
        {
            userName = "";

        }

        public ProfileData(string userName)
        {
            this.userName = userName;

        }

    }
}


namespace NetworkMaps
{
    [System.Serializable]
    public class MapData
    {
        public string name;
        public int scene;
    }
}


public class NetworkLobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField userNameField;
    public TMP_InputField roomNameField;
    public TMP_Text mapValue;
    public TMP_Text modeValue;
    public TMP_Dropdown maxPlayersDropDown;
    //public Text maxPlayersValue;
    public static ProfileData myProfile = new ProfileData();

    public GameObject tabMain;
    public GameObject tabRooms;
    public GameObject tabCreate;

    public GameObject buttonRoom;

    public MapData[] maps;
    public int currentMap = 0;

    public List<RoomInfo> roomList;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        myProfile = DataClass.LoadProfile();
        if (!string.IsNullOrEmpty(myProfile.userName))
        {
            userNameField.text = myProfile.userName;
            
        }
        
        // update current map index
        currentMap = SceneManager.GetActiveScene().buildIndex;

        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server!");

        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect()
    {
        Debug.Log("Trying to connect...");

        PhotonNetwork.GameVersion = "0.0.0"; // we need to set this up
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
            return;
        
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte) maxPlayersDropDown.value;

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

    public void ChangeMap()
    {
        currentMap++;

        if (currentMap >= maps.Length)
            currentMap = 0;

        mapValue.text = "Map: " + maps[currentMap].name.ToUpper();
    }

    public void ChangeMode()
    {
        int newMode = (int) GameSettings.GameMode + 1;

        if (newMode >= System.Enum.GetValues(typeof(GameModeSelections)).Length) 
            newMode = 0;

        GameSettings.GameMode = (GameModeSelections) newMode;

        modeValue.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), newMode);
    }

    // public void ChangeMaxPlayersSlider(float _value)
    // {
    //     maxPlayersValue.text = Mathf.RoundToInt(_value).ToString();
    // }

    private void TabCloseAll()
    {
        tabMain.SetActive(false);
        tabRooms.SetActive(false);
        tabCreate.SetActive(false);
    }

    public void TabOpenMain()
    {
        TabCloseAll();
        tabMain.SetActive(true);
    }

    public void TabOpenRooms()
    {
        TabCloseAll();
        tabRooms.SetActive(true);
    }

    public void TabOpenCreate()
    {
        TabCloseAll();
        tabCreate.SetActive(true);

        roomNameField.text = "";

        currentMap = 0;
        mapValue.text = "Map: " + maps[currentMap].name.ToUpper();

        GameSettings.GameMode = (GameModeSelections) 0;
        modeValue.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), (GameModeSelections) 0);

        // GOING TO CHANGE THIS TO A DROP DOWN MENU I THINK
        //maxPlayersSlider.value = maxPlayersSlider.maxValue;
        
        // // Players selection
        // // int 0 = 1 vs 1
        // // int 1 = 2 vs 2
        // int playersSelection = maxPlayersDropDown.value;
        //
        // if (playersSelection == 1)
        //     maxPlayersValue.text = "2";
        // else
        //     maxPlayersValue.text = "4";
    }

    private void ClearRoomList()
    {
        Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");
        
        foreach(Transform item in content)
            Destroy(item.gameObject);
    }

    private void VerifyUserName()
    {
        if (string.IsNullOrEmpty(userNameField.text))
        {
            myProfile.userName = "Random_User_" + UnityEngine.Random.Range(100, 10000);
        }
        else
        {
            myProfile.userName = userNameField.text;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> list)
    {
        roomList = list;
        ClearRoomList();

        Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");

        // if (roomList.Count <= 0)
        //     return;
        
        foreach (RoomInfo item in roomList)
        {
            GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;

            newRoomButton.transform.GetComponent<RoomButtonInfo>().roomName.text = item.Name;
            newRoomButton.transform.GetComponent<RoomButtonInfo>().playersCounter.text = item.PlayerCount + " / " + item.MaxPlayers;
            
            if (item.CustomProperties.ContainsKey("map"))
                newRoomButton.transform.GetComponent<RoomButtonInfo>().mapName.text = maps[(int) item.CustomProperties["map"]].name;
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
        GameSettings.GameMode = (GameModeSelections)roomInfo.CustomProperties["mode"];
    }

    public void StartGame()
    {
        VerifyUserName();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("starting game");
            
            DataClass.SaveProfile(myProfile);
            
            PhotonNetwork.LoadLevel(maps[currentMap].scene);
        }
    }
    


}
