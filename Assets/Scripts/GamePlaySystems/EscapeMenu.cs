using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public Button returnButton;
    public Button quitButton;
    public GameObject menu;
    public GameObject emoteMenu;
    void Start()
    {
        menu.SetActive(false);

        if(returnButton)
        {
            returnButton.onClick.AddListener(Return);
        }
        
        if(quitButton)
        {
            quitButton.onClick.AddListener(Quit);
        }

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeInHierarchy == true)
            {
                Return();
            }
            if (menu.activeInHierarchy == false)
            {
                #if UNITY_EDITOR
                    return;
                #else
                    OpenMenu();
                #endif
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(menu.activeInHierarchy == true)
            {
                Return();
            }
            if (menu.activeInHierarchy == false)
            {
                #if UNITY_EDITOR
                    OpenMenu();
                #else
                    return;
                #endif
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            emoteMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyUp(KeyCode.G))
        {
            emoteMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Return()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menu.SetActive(false);
    }

    public void Quit()
    {
        //Photon if statement
        //SceneManager.LoadScene("MainMenuScene")
        
        Debug.Log($"network manager status: {GameManager.networkManager}");
        Debug.Log($"audio manager status: {GameManager.audioManager}");

        if(GameManager.networkManager)
            GameManager.networkManager.LeavingGame();
    }

    public void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        menu.SetActive(true);
    }

}
