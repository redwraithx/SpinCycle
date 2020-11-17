using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button backToMainButton;
    public Button playButton;
    void Start()
    {
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
    }

   
    void Update()
    {
        
    }
}
