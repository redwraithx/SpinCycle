using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    #region GAMEMANAGER_CORE-EXTENTIONS

    public static AudioManager audioManager = null;
    public static NetworkLobby networkManager = null;
    public static NetworkLevelManager networkLevelManager = null;

    
    
    #endregion GAMEMANAGER_CORE-EXTENTIONS
    
    #region Tracked_Variables

    
    
    [SerializeField] private GameObject player1;

    public GameObject Player1
    {
        get => player1 ? player1 : null;
        set
        {
            if(!player1)
                player1 = value;
        }
    }
    
    [SerializeField] private GameObject player2;
    public GameObject Player2
    {
        get => player2 != null ? player2 : null;
        set => player2 = value;
    }

    #endregion // Tracked_Variables

    #region Levels_Selector_List


    internal enum BuildSettingLevelNames
    {
        TitleScreen,
        MainMenuScene,
        SettingsMenu,
        LoadingScreen1,
        Credits,
        NetworkLobby,
        LobbyWaitingRoomScene,
        AssetTesting,
        TutorialLevel,
        
    }
    
    
    #endregion // Levels_Selector_List

    void Awake()
    {

        if (Instance)
        {
            DestroyImmediate(gameObject);

            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        networkManager = null;
        networkLevelManager = null;

    }

    public void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }
    //loads the game level
    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScreen1"); 
    }
    
    
    //leaves the game
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    
    
    // to get to the main menu
    public void ToMain()
    {
        networkLevelManager = null;
        
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    
    
    public static GameManager Instance
    {
        get => _instance;
        //private set => _instance = value;
    }


    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
}
