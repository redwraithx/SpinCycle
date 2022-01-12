using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using NetworkLobbyGameSettings;

public class LocalLobbyManager : MonoBehaviourPun
{
    public static LocalLobbyManager localInstance;

    //[Header("Network Lobby max games")]
    public int MAXNumberOfRooms = 6;

    //[Header("Players Max Character Name Count component reference")]
    private const int MAXUserNameLength = 12;

    [Header("User Information Message Box")]
    public GameObject messageBoxMainUIRef = null;

    public TMP_Text msgBoxMainTabUI_TMP = null;
    [Space] public GameObject messageBoxCreateUIRef = null;
    public TMP_Text msgBoxCreateTabUI_TMP = null;
    private const string MsgBoxText_MissingUserName = "UserName is missing!";
    private const string MsgBoxText_MaxHostedGames = "Max hosted games reached!";
    private const string MsgBoxText_RoomNameRequired = "Room Name is required!";

    [Header("User InputFields - Text - DropDown References")]
    public TMP_InputField userNameField = null;

    public TMP_InputField roomNameField = null;
    public TMP_Text mapValueText = null;
    public TMP_Text modeValueText = null;
    public TMP_Dropdown maxPlayersDropDown = null;

    [Header("Lobby User UI References")]
    public GameObject tabMainUI = null;

    public GameObject tabRoomsUI = null;
    public GameObject tabCreateUI = null;
    public GameObject loadingLobbyUI = null;

    [Header("Main Tab Button References")]
    public Button tabMainButtonListGames = null;

    public Button tabMainButtonCreateGame = null;
    public Button tabMainButtonBackToMainMenu = null;
    public Button tabMainButtonQuitGame = null;

    [Header("Room List Games button References")]
    public Button tabRoomsBackToMainTabUI = null;

    [Header("Create a new Game Buttons References")]
    public Button tabCreateButtonMapSelect = null;

    public Button tabCreateButtonModeSelect = null;
    public Button tabCreateButtonBackToMainTabUI = null;
    public Button tabCreateButtonCreateGame = null;

    [Header("AudioSource & Click Button Sound Reference")]
    public AudioSource audioSource = null;

    public AudioClip clickSound = null;

    [Header("Max Hosted games reached Container object")]
    public GameObject messageBoxGO = null;

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
        // set username max characters
        userNameField.characterLimit = MAXUserNameLength;

        // tab main button setup
        tabMainButtonListGames.onClick.AddListener(TabOpenRooms); //GameManager.networkManager.TabOpenRooms);
        tabMainButtonListGames.onClick.AddListener(PlayClickSound);
        tabMainButtonCreateGame.onClick.AddListener(TabOpenCreate); //GameManager.networkManager.TabOpenCreate);
        tabMainButtonCreateGame.onClick.AddListener(PlayClickSound);
        tabMainButtonBackToMainMenu.onClick.AddListener(GameManager.networkManager.GetComponent<NetworkLobbyMenu>().BackToMainMenu);
        tabMainButtonBackToMainMenu.onClick.AddListener(PlayClickSound);
        tabMainButtonQuitGame.onClick.AddListener(GameManager.networkManager.GetComponent<NetworkLobbyMenu>().QuitGame);
        tabMainButtonQuitGame.onClick.AddListener(PlayClickSound);

        // rooms list button setup
        tabRoomsBackToMainTabUI.onClick.AddListener(TabOpenMain); //GameManager.networkManager.TabOpenMain);
        tabRoomsBackToMainTabUI.onClick.AddListener(PlayClickSound);

        // create new Game Button setup
        tabCreateButtonBackToMainTabUI.onClick.AddListener(TabOpenMain); //GameManager.networkManager.TabOpenMain);
        tabCreateButtonBackToMainTabUI.onClick.AddListener(PlayClickSound);
        tabCreateButtonMapSelect.onClick.AddListener(ChangeMap); //GameManager.networkManager.ChangeMap);
        tabCreateButtonMapSelect.onClick.AddListener(PlayClickSound);
        tabCreateButtonModeSelect.onClick.AddListener(ChangeMode); //GameManager.networkManager.ChangeMode);
        tabCreateButtonModeSelect.onClick.AddListener(PlayClickSound);
        tabCreateButtonCreateGame.onClick.AddListener(Create); //GameManager.networkManager.Create);
        tabCreateButtonCreateGame.onClick.AddListener(PlayClickSound);

        maxPlayersDropDown.value = 0;

        GameManager.networkManager.StopCoroutineBeforeJoiningNewGame();
    }

    private void Update()
    {
        if (tabMainUI.activeInHierarchy)
        {
            if (CheckForValidUserName())
            {
                if (messageBoxMainUIRef.activeInHierarchy == true)
                {
                    msgBoxMainTabUI_TMP.text = "";
                    messageBoxMainUIRef.SetActive(false);
                }

                if (tabMainButtonListGames.interactable == false)
                {
                    UpdateButtonClickableValue(tabMainButtonListGames, true);
                    tabMainButtonListGames.interactable = true;
                }

                bool maxHostedGamesReached = CheckForMaxRooms();

                if (maxHostedGamesReached)
                {
                    if (tabMainButtonCreateGame.interactable == true)
                    {
                        UpdateButtonClickableValue(tabMainButtonCreateGame, false);
                        tabMainButtonCreateGame.interactable = false;
                    }

                    if (!messageBoxMainUIRef.activeInHierarchy)
                    {
                        messageBoxMainUIRef.SetActive(true);
                        msgBoxMainTabUI_TMP.text = MsgBoxText_MaxHostedGames;
                    }
                }
                else
                {
                    if (tabMainButtonCreateGame.interactable == false)
                    {
                        UpdateButtonClickableValue(tabMainButtonCreateGame, true);
                        tabMainButtonCreateGame.interactable = true;
                    }
                }
            }
            else
            {
                if (!messageBoxMainUIRef.activeInHierarchy)
                {
                    messageBoxMainUIRef.SetActive(true);
                    msgBoxMainTabUI_TMP.text = MsgBoxText_MissingUserName;
                }

                // disable list rooms button
                if (tabMainButtonListGames.interactable == true)
                {
                    UpdateButtonClickableValue(tabMainButtonListGames, false);
                    tabMainButtonListGames.interactable = false;
                }

                // disable create game button
                if (tabMainButtonCreateGame.interactable == true)
                {
                    UpdateButtonClickableValue(tabMainButtonCreateGame, false);
                    tabMainButtonCreateGame.interactable = false;
                }
            }
        }

        if (tabCreateUI.activeInHierarchy)
        {
            bool atMaxHostedGames = CheckForMaxRooms();

            if (atMaxHostedGames && roomNameField.text.Length < 3)
            {
                //Debug.Log("max hosts & room name is less then 3 - show room name is required");
                // max hosted servers running
                if (tabCreateButtonCreateGame.interactable == true)
                {
                    UpdateButtonClickableValue(tabCreateButtonCreateGame, false);
                    tabCreateButtonCreateGame.interactable = false;
                }

                if (!messageBoxCreateUIRef.activeInHierarchy)
                {
                    messageBoxCreateUIRef.SetActive(true);
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_RoomNameRequired;
                }
                else
                {
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_RoomNameRequired;
                }
            }
            else if (atMaxHostedGames && roomNameField.text.Length >= 3)
            {
                //Debug.Log("max hosts & room name is les then or equal to 3 - show max hosts detected");

                // max hosted servers running
                if (tabCreateButtonCreateGame.interactable == true)
                {
                    UpdateButtonClickableValue(tabCreateButtonCreateGame, false);
                    tabCreateButtonCreateGame.interactable = false;
                }

                if (!messageBoxCreateUIRef.activeInHierarchy)
                {
                    messageBoxCreateUIRef.SetActive(true);
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_MaxHostedGames;
                }
                else
                {
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_MaxHostedGames;
                }
            }
            else if (!atMaxHostedGames && roomNameField.text.Length < 3)
            {
                if (tabCreateButtonCreateGame.interactable == true)
                {
                    UpdateButtonClickableValue(tabCreateButtonCreateGame, false);
                    tabCreateButtonCreateGame.interactable = false;
                }

                if (!messageBoxCreateUIRef.activeInHierarchy)
                {
                    messageBoxCreateUIRef.SetActive(true);
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_RoomNameRequired;
                }
                else
                {
                    msgBoxCreateTabUI_TMP.text = MsgBoxText_RoomNameRequired;
                }
            }
            else
            {
                if (tabCreateButtonCreateGame.interactable == false)
                {
                    UpdateButtonClickableValue(tabCreateButtonCreateGame, true);
                    tabCreateButtonCreateGame.interactable = true;
                }

                if (messageBoxCreateUIRef.activeInHierarchy)
                {
                    msgBoxCreateTabUI_TMP.text = "";
                    messageBoxCreateUIRef.SetActive(false);
                }
            }
        }
    }

    private bool CheckForValidUserName()
    {
        //Debug.Log("Username length: " + userNameField.text.Length);

        return userNameField.text.Length >= 3 && userNameField.text.Length <= MAXUserNameLength;
    }

    private void UpdateButtonClickableValue(Button btn, bool isEnabled)
    {
        if (isEnabled)
        {
            btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, 1f);
        }
        else
        {
            btn.image.color = new Color(btn.image.color.r, btn.image.color.g, btn.image.color.b, 0.45f);
        }
    }

    private void PlayClickSound() => audioSource.PlayOneShot(clickSound);

    public void TabOpenMain()
    {
        //Debug.Log("Tab Open Main Func in local lobby");

        TabCloseAll();

        tabMainUI.SetActive(true);

        // check if we have to many games running currently
        if (PhotonNetwork.CountOfRooms >= MAXNumberOfRooms)
        {
            //Debug.Log("if");
            if (tabMainButtonCreateGame.IsActive())
            {
                //Debug.Log("disable button");

                tabMainButtonCreateGame.image.color = new Color(tabMainButtonCreateGame.image.color.r, tabMainButtonCreateGame.image.color.g, tabMainButtonCreateGame.image.color.b, 0.45f);
                tabMainButtonCreateGame.enabled = false;
            }
        }
        else
        {
            //Debug.Log("else");
            if (!tabMainButtonCreateGame.IsActive())
            {
                //Debug.Log("enable button");

                tabMainButtonCreateGame.image.color = new Color(tabMainButtonCreateGame.image.color.r, tabMainButtonCreateGame.image.color.g, tabMainButtonCreateGame.image.color.b, 1f);
                tabMainButtonCreateGame.enabled = true; // this was false
            }
        }
    }

    public void TabOpenRooms()
    {
        //Debug.Log("Tab Open Rooms Func in local lobby");

        TabCloseAll();

        tabRoomsUI.SetActive(true);
    }

    public void TabOpenCreate()
    {
        //Debug.Log("Tab Open Create Func");

        TabCloseAll();

        tabCreateUI.SetActive(true);

        roomNameField.text = "";

        GameManager.networkManager.currentMap = 0;
        mapValueText.text = "Map: " + GameManager.networkManager.maps[GameManager.networkManager.currentMap].name.ToUpper();

        GameSettings.GameMode = (GameModeSelections)0;
        modeValueText.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), (GameModeSelections)0);

        // GOING TO CHANGE THIS TO A DROP DOWN MENU I THINK
        //maxPlayersSlider.value = maxPlayersSlider.maxValue;

        // Players selection
        // int 0 = 1 vs 1
        // int 1 = 2 vs 2
        int playersSelection = maxPlayersDropDown.value;

        //Debug.Log("players in room = " + playersSelection);

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
        //Debug.Log("Tab Close All Func in local lobby");

        tabMainUI.SetActive(false);
        tabRoomsUI.SetActive(false);
        tabCreateUI.SetActive(false);
    }

    public void ChangeMap()
    {
        if (CheckForMaxRooms())
        {
            // show msg that creating rooms is disabled due to max games running
            StartCoroutine(ShowMessageForMaxGamesReached());

            //TabOpenMain();

            return;
        }

        GameManager.networkManager.currentMap++;

        if (GameManager.networkManager.currentMap >= GameManager.networkManager.maps.Length)
            GameManager.networkManager.currentMap = 0;

        mapValueText.text = "Map: " + GameManager.networkManager.maps[GameManager.networkManager.currentMap].name.ToUpper();
    }

    public void ChangeMode()
    {
        if (CheckForMaxRooms())
        {
            // show msg that creating rooms is disabled due to max games running
            StartCoroutine(ShowMessageForMaxGamesReached());

            //TabOpenMain();

            return;
        }

        int newMode = (int)GameSettings.GameMode + 1;

        if (newMode >= System.Enum.GetValues(typeof(GameModeSelections)).Length)
            newMode = 0;

        GameSettings.GameMode = (GameModeSelections)newMode;

        modeValueText.text = "Mode: " + System.Enum.GetName(typeof(GameModeSelections), newMode);
    }

    public void Create()
    {
        //Debug.Log("Create Game Button Func");

        // do we have max rooms? while creating a room?
        if (CheckForMaxRooms())
        {
            // show msg that creating rooms is disabled due to max games running
            StartCoroutine(ShowMessageForMaxGamesReached());

            //TabOpenMain();

            return;
        }

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

        options.MaxPlayers = (byte)currentGamesMaxPlayers;

        options.CustomRoomPropertiesForLobby = new string[]
        {
            "map",
            "mode"
        };

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("map", GameManager.networkManager.currentMap);
        properties.Add("mode", (int)GameSettings.GameMode);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomNameField.text, options);
    }

    private bool CheckForMaxRooms()
    {
        if (PhotonNetwork.CountOfRooms >= MAXNumberOfRooms)
        {
            //Debug.Log("max rooms have been met while you were making a room, try again later");

            //if (tabMainButtonCreateGame.IsActive())
            //{
            //Debug.Log("disable create game button in main tab view");

            if (tabMainUI.activeInHierarchy && tabMainButtonCreateGame.interactable == true)
            {
                tabMainButtonCreateGame.image.color = new Color(tabMainButtonCreateGame.image.color.r, tabMainButtonCreateGame.image.color.g, tabMainButtonCreateGame.image.color.b, 0.45f);

                tabMainButtonCreateGame.interactable = false;
            }
            if (tabCreateUI.activeInHierarchy && tabCreateButtonCreateGame.interactable == true)
            {
                tabCreateButtonCreateGame.image.color = new Color(tabCreateButtonCreateGame.image.color.r, tabCreateButtonCreateGame.image.color.g, tabCreateButtonCreateGame.image.color.b, 0.45f);

                tabCreateButtonCreateGame.interactable = false;
            }
            //}

            // switch to main
            //TabOpenMain();}

            return true; // max rooms has been met, can not make a new game.
        }

        // enable the button for creating a game if it is not active for main
        // if (tabMainButtonCreateGame.image.color == new Color(tabMainButtonCreateGame.image.color.r, tabMainButtonCreateGame.image.color.g, tabMainButtonCreateGame.image.color.b, 0.45f))
        // {
        //     tabMainButtonCreateGame.image.color = new Color(tabMainButtonCreateGame.image.color.r, tabMainButtonCreateGame.image.color.g, tabMainButtonCreateGame.image.color.b, 1f);
        //     tabMainButtonCreateGame.enabled = true;
        // }

        return false; // max rooms not met
    }

    private IEnumerator ShowMessageForMaxGamesReached()
    {
        // show dialogue for max servers reached
        messageBoxGO.SetActive(true);

        yield return new WaitForSeconds(2f);

        // hide max servers msg
        messageBoxGO.SetActive(false);

        // enable main lobby view
        TabOpenMain();
    }
}