<<<<<<< HEAD
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
=======
﻿using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


>>>>>>> main

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    #region GAMEMANAGER_CORE-EXTENTIONS

<<<<<<< HEAD
    public static AudioManager audioManager = null; 
=======
    public static AudioManager audioManager = null;
    public static NetworkManager networkManager = null;
    public static NetworkLevelManager networkLevelManager = null;
>>>>>>> main
    
    
    #endregion GAMEMANAGER_CORE-EXTENTIONS
    
    #region Tracked_Variables

<<<<<<< HEAD
=======
    
    
>>>>>>> main
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
<<<<<<< HEAD
            Destroy(gameObject);
=======
            DestroyImmediate(gameObject);
>>>>>>> main
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
<<<<<<< HEAD
=======
        networkLevelManager = null;
        
>>>>>>> main
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    
    
    public static GameManager Instance
    {
<<<<<<< HEAD
        get { return _instance; }
       // private set { _instance = value; }
    }

=======
        get => _instance;
        //private set => _instance = value;
    }

    
    //function with temp load for sample scene is now in LoadScreen2CanvasManager
>>>>>>> main
    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen2");  
    }
<<<<<<< HEAD
    //function with temp load for sample scene is now in LoadScreen2CanvasManager
    
    
=======
>>>>>>> main
}
