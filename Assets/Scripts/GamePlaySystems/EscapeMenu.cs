using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public Button returnButton;
    public Button quitButton;
    public GameObject menu;
    public GameObject debugger;
    // Start is called before the first frame update
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

        if(Input.GetKeyDown(KeyCode.L))
        {
            if (debugger.activeInHierarchy == true)
            {
                debugger.SetActive(false);
            }
            if (debugger.activeInHierarchy == false)
            {
                debugger.SetActive(true);
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
    }
    public void Return()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menu.SetActive(false);
    }

    public void Quit()
    {
        //Photon if statement
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
