using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    #region GAMEMANAGER_CORE-EXTENTIONS

    public static AudioManager audioManager = null;
    public static NetworkLobby networkManager = null;
    public static NetworkLevelManager networkLevelManager = null;
    public static UIDebugger uiDebugger = null;
    
    
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
    
    

    void Awake()
    {

        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

    }

    // checking to see if the player hit escape when in the game level
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "GameLevelScene")
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }
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

    
    //function with temp load for sample scene is now in LoadScreen2CanvasManager
    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen2");  
    }
}
