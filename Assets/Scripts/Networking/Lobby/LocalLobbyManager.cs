

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using NetworkLobbyGameSettings;

public class LocalLobbyManager : MonoBehaviourPun
{
    
    public static LocalLobbyManager localInstance;

    [Header("User InputFields - Text - DropDown References")]
    public TMP_InputField userNameField = null;
    public TMP_InputField roomNameField = null;
    public TMP_Text mapValueText = null;
    public TMP_Text modeValueText = null;
    public TMP_Dropdown maxPlayersDropDown = null;

    [Header("Lobby User UI References")]
    public GameObject loadingLobbyUI = null;
    public GameObject tabMainUI = null;
    public GameObject tabRoomsUI = null;
    public GameObject tabCreateUI = null;

    [Header("Main Tab Button References")]
    public Button tabMainButtonListGames = null;
    public Button tabMainButtonCreateGame = null;
    public Button tabMainButtonBackToMainMenu = null;
    public Button tabMainButtonQuitGame = null;

    [Header("Room List Games button References")]
    public Button tabRoomsBackToMainTabUI = null;

    [Header("Create a new Game Buttons References")]
    public Button tabCreateButtonBackToMainTabUI = null;
    public Button tabCreateButtonMapSelect = null;
    public Button tabCreateButtonModeSelect = null;
    public Button tabCreateButtonCreateGame = null;

    [Header("AudioSource & Click Button Sound Reference")]
    public AudioSource audioSource = null;
    public AudioClip clickSound = null;

    private void Awake()
    {
        if (localInstance)
        {
            DestroyImmediate(gameObject);
        }

        localInstance = this;
    }

    private void Start()
    {
        // tab main button setup
        tabMainButtonListGames.onClick.AddListener(TabOpenRooms); //GameManager.networkManager.TabOpenRooms);
        //tabMainButtonListGames.onClick.AddListener(PlayClickSound);
        tabMainButtonCreateGame.onClick.AddListener(TabOpenCreate); //GameManager.networkManager.TabOpenCreate);
        //tabMainButtonCreateGame.onClick.AddListener(PlayClickSound);
        tabMainButtonBackToMainMenu.onClick.AddListener(GameManager.networkManager.GetComponent<NetworkLobbyMenu>().BackToMainMenu);
        //tabMainButtonQuitGame.onClick.AddListener(PlayClickSound);
        tabMainButtonQuitGame.onClick.AddListener(GameManager.networkManager.GetComponent<NetworkLobbyMenu>().QuitGame);
        //tabMainButtonQuitGame.onClick.AddListener(PlayClickSound);
        
        // rooms list button setup
        tabRoomsBackToMainTabUI.onClick.AddListener(TabOpenMain); //GameManager.networkManager.TabOpenMain);
        //tabRoomsBackToMainTabUI.onClick.AddListener(PlayClickSound);
        
        // create new Game Button setup
        tabCreateButtonBackToMainTabUI.onClick.AddListener(TabOpenMain); //GameManager.networkManager.TabOpenMain);
        //tabCreateButtonBackToMainTabUI.onClick.AddListener(PlayClickSound);
        tabCreateButtonMapSelect.onClick.AddListener(ChangeMap); //GameManager.networkManager.ChangeMap);
        //tabCreateButtonMapSelect.onClick.AddListener(PlayClickSound);
        tabCreateButtonModeSelect.onClick.AddListener(ChangeMode); //GameManager.networkManager.ChangeMode);
        //tabCreateButtonModeSelect.onClick.AddListener(PlayClickSound);
        tabCreateButtonCreateGame.onClick.AddListener(Create); //GameManager.networkManager.Create);
        //tabCreateButtonCreateGame.onClick.AddListener(PlayClickSound);


        maxPlayersDropDown.value = 0;
        
        GameManager.networkManager.StopCoroutineBeforeJoiningNewGame();
    }

    //private void PlayClickSound() => audioSource.PlayOneShot(clickSound);
    
    
    public void TabOpenMain()
    {
        Debug.Log("Tab Open Main Func in local lobby");
        
        TabCloseAll();
        
        tabMainUI.SetActive(true);
    }
    
    public void TabOpenRooms()
    {
        Debug.Log("Tab Open Rooms Func in local lobby");
        
        TabCloseAll();
        
        tabRoomsUI.SetActive(true);
    }

    public void TabOpenCreate()
    {
        Debug.Log("Tab Open Create Func");

        TabCloseAll();

        tabCreateUI.SetActive(true);

        roomNameField.text = "";

        GameManager.networkManager.currentMap = 0;
        mapValueText.text = "Map: " + GameManager.networkManager.maps[GameManager.networkManager.currentMap].name.ToUpper();

        GameSettings.GameMode = (GameModeSelections) 0;
        modeValueText.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), (GameModeSelections) 0);

        // GOING TO CHANGE THIS TO A DROP DOWN MENU I THINK
        //maxPlayersSlider.value = maxPlayersSlider.maxValue;

        // Players selection
        // int 0 = 1 vs 1
        // int 1 = 2 vs 2
        int playersSelection = maxPlayersDropDown.value;
        
        Debug.Log("players in room = " + playersSelection);
        Debug.Log("test");

        if (GameManager.networkManager.maxPlayersDropDown)
        {
            GameManager.networkManager.maxPlayersDropDown.value = playersSelection;
        }
        else
        {
            GameManager.networkManager.maxPlayersDropDown = maxPlayersDropDown;
            
        }

        if (playersSelection == 0)
        {
            Debug.Log("2 players selected");


        }
        else
        {
            Debug.Log("solo game");
        }
    
    }
    
    
    // internal void ResetEndGameScenario()
    // {
    //     // if (SceneManager.GetActiveScene().buildIndex != networkLobbySceneIndex)
    //     //     return;
    //     
    //     ResetCurrentEndGameTimer();
    //     endGameState = EndGameState.ReadyAndWaiting;
    //
    //     isUpdatingToNewPhase = false;
    //     
    //     Debug.Log("has reset end game stuff in ResetEndGameScenario()");
    // }
    
    
    private void TabCloseAll()
    {
        Debug.Log("Tab Close All Func in local lobby");
        
        tabMainUI.SetActive(false);
        tabRoomsUI.SetActive(false);
        tabCreateUI.SetActive(false);
    }
    
    public void ChangeMap()
    {
        GameManager.networkManager.currentMap++;

        if (GameManager.networkManager.currentMap >= GameManager.networkManager.maps.Length)
            GameManager.networkManager.currentMap = 0;

        mapValueText.text = "Map: " + GameManager.networkManager.maps[GameManager.networkManager.currentMap].name.ToUpper();
    }
    
    
    public void ChangeMode()
    {
        int newMode = (int) GameSettings.GameMode + 1;

        if (newMode >= System.Enum.GetValues(typeof(GameModeSelections)).Length) 
            newMode = 0;

        GameSettings.GameMode = (GameModeSelections) newMode;

        modeValueText.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), newMode);
    }
    
    public void Create()
    {
        Debug.Log("Create Game Button Func");
        
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
        properties.Add("map", GameManager.networkManager.currentMap);
        properties.Add("mode", (int) GameSettings.GameMode);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomNameField.text, options);
    }
}
