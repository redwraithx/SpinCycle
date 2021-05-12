<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
using UnityEngine.SceneManagement;
>>>>>>> main
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button backToMainButton;
    public Button playButton;
    public Button settingsButton;
<<<<<<< HEAD
=======

    public int MainMenuSceneIndex = 0;
>>>>>>> main
    
    
    void Start()
    {
<<<<<<< HEAD
=======
        Cursor.lockState = CursorLockMode.None;
        
>>>>>>> main
        //these are all just checking to see if you've hit the button and then triggering the scripts in game manager
        if (startButton)
        {
            startButton.onClick.AddListener(GameManager.Instance.StartGame);
        }

        if (quitButton)
        {
            quitButton.onClick.AddListener(GameManager.Instance.QuitGame);
        }
        if (backToMainButton)
        {
            backToMainButton.onClick.AddListener(GameManager.Instance.ToMain);
        }
        if (playButton)
        {
            playButton.onClick.AddListener(GameManager.Instance.PlayGame);
        }
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(GameManager.Instance.ToSettings);
        }
    }

<<<<<<< HEAD
   
    void Update()
    {
        
    }
=======
    public void LoadMainMenu()
    {
        Debug.Log("goto main menu");
        SceneManager.LoadSceneAsync(MainMenuSceneIndex);
    }

    public void ExitGame()
    {
        Debug.Log("exitgame button");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


>>>>>>> main
}
