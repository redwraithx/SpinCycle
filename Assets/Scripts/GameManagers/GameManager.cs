using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    
    
    #region GAMEMANAGER_CORE-EXTENTIONS

    public static AudioManager audioManager = null; 
    
    
    #endregion GAMEMANAGER_CORE-EXTENTIONS
    
    

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public static GameManager Instance
    {
        get { return instance; }
       // private set { _instance = value; }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen2");  
    }
    //function with temp load for sample scene is now in LoadScreen2CanvasManager
}
