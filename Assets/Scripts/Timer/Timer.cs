using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;
    public Image GameOverImage;
    public Image BlackImage;
    public Animator anim;
    public Text winnerText;
    public Text pointText;
    public float player1Points;
    public float player2Points;
    public string player1Name;
    public string player2Name;

    public int GameOverSceneIndex = 0;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;

    }

    void Update()
    {
        //if (GameManager.networkLevelManager.playersJoined.Count == 2)
        //{
        //    Debug.Log("2 players in scene and the clock is ticking");
        //    timerIsRunning = true;
        //}

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                if (GameManager.networkLevelManager.playersJoined.Count < 2)
                {
                    player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
                    Debug.Log("remaining player wins with a score of " + player1Points);
                }
                else if (GameManager.networkLevelManager.playersJoined.Count == 2)
                {
                    player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
                    player2Points = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points;
                    Debug.Log(Mathf.Max(player1Points, player2Points));
                }


                Debug.Log("Time has run out! May want to use a UI element here");
                timeRemaining = 0;
                timerIsRunning = false;
                GameOverImage.gameObject.SetActive(true);
                StartCoroutine(Fading());
            }
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => BlackImage.color.a == 1);
        SceneManager.LoadScene(GameOverSceneIndex);

    }
}