using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioTheme : MonoBehaviour
{
    //public int netWorkLobbySceneIndex;
    public static AudioTheme instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        AudioClip gameTheme = Resources.Load<AudioClip>("AudioFiles/Music/MenuTheme/Washed_Up_theme_idea_1(1)");
        GameManager.audioManager.PlayMusic(gameTheme);
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
  /*  void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == netWorkLobbySceneIndex)
        {
            Debug.Log("Not Stopingasdjkahdskajhdsj");
            

            if (GameManager.audioManager.IsMusicPlaying())
            {
                GameManager.audioManager.StopMusic();
            }
        }
    }*/
}
