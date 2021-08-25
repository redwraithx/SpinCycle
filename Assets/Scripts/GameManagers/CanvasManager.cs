using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using PlayerProfileData;
using NetworkProfile;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button backToMainButton;
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;

    public int MainMenuSceneIndex = 0;

    [SerializeField] private int mainMenuSceneBuildIndex = 1;
    [SerializeField] private bool hasPlayerProfileLoaded = false;
    
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.None;
        
        //these are all just checking to see if you've hit the button and then triggering the scripts in game manager
        if (startButton)
        {
            // This is for checking for tutorial has been played or not
            // if (SceneManager.GetActiveScene().buildIndex == mainMenuSceneBuildIndex)
            // {
            //     // load the player profile
            //     ProfileData profile = DataClass.LoadProfile();
            //     
            //     hasPlayerProfileLoaded = true;
            //     
            //     if(profile != null)
            //     {
            //         Debug.Log("player profile was loaded and is NOT null");
            //         
            //         if (profile.hasPlayedTutorial == false)
            //         {
            //             Debug.Log("player profile shows that the tutorial level has not been played yet.");
            //                 
            //             var startButtonComponentTMP = startButton.GetComponentInChildren<TMP_Text>();
            //
            //             //startButtonComponentTMP.text = "Learn To Play";
            //             
            //             startButton.onClick.AddListener(GameManager.Instance.PlayTutorial);
            //         }
            //         else
            //         {
            //             Debug.Log("players profile show that the tutorial was already played");
            //             
            //             startButton.onClick.AddListener(GameManager.Instance.StartGame);
            //         }
            //     }
            //     else
            //     {
            //         Debug.Log("Players profile has not been created or has become corrupted");
            //         
            //         startButton.onClick.AddListener(GameManager.Instance.StartGame);
            //     }
            // }
            // else
            // {
            //     Debug.Log("did not load player profile as we are not on the main menu scene.");
            //     
                 startButton.onClick.AddListener(GameManager.Instance.StartGame);
            // }
        }

        if (quitButton)
        {
            quitButton.onClick.AddListener(GameManager.Instance.QuitGame);
        }
        if (backToMainButton)
        {
            backToMainButton.onClick.AddListener(GameManager.Instance.ToMain);
        }
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(GameManager.Instance.ToSettings);
        }
        if (creditsButton)
        {
            creditsButton.onClick.AddListener(GameManager.Instance.ToCredits);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == mainMenuSceneBuildIndex)
        {
            // load the player profile
            
            
            // check if player has seen the tutorial
            
            
            // show the tutorial if they have not seen it yet.
            

            
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(MainMenuSceneIndex);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
