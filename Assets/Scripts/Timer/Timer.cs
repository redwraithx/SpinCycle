using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;
    public Image GameOverImage;
    public Image BlackImage;
    public Animator anim;
    public float player1Points;
    public float player2Points;
    public string player1Name;
    public string player2Name;
    public TMP_Text pointText;

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
                    player1Name = GameManager.networkLevelManager.playersJoined[0].name;
                    pointText.text = ("One player left, remaining player" + player1Name + "wins with a score of " + player1Points);
                }
                else if (GameManager.networkLevelManager.playersJoined.Count == 2)
                {
                    player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
                    player2Points = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points;
                    player1Name = GameManager.networkLevelManager.playersJoined[0].name;
                    player2Name = GameManager.networkLevelManager.playersJoined[0].name;
                }

                if(player1Points > player2Points)
                {
                    pointText.text = ("player1 wins with a score of " + player1Points);
                }
                if (player2Points > player1Points)
                {
                    pointText.text = ("player2 wins with a score of " + player2Points);
                }
                else if(player1Points == player2Points)
                {
                    pointText.text = ("Tied with a score of " + player1Points);
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