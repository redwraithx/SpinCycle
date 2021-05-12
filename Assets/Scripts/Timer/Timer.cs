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

    public TMP_Text pointText2;
    public TMP_Text pointText;
    public TMP_Text loserText;

    public GameObject winner;
    public GameObject loser;
    public GameObject vStand;
    public GameObject loserVille;
    public GameObject vStandPrefab;
    

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
                    winner = GameManager.networkLevelManager.playersJoined[0];
                    pointText.text = player1Points.ToString();
                    pointText2.text = ("Abandoned Match");
                    loserText.text = ("Coward");
                }
                else if (GameManager.networkLevelManager.playersJoined.Count == 2)
                {
                    player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
                    player2Points = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points;
                    player1Name = GameManager.networkLevelManager.playersJoined[0].name;
                    player2Name = GameManager.networkLevelManager.playersJoined[1].name;

                    if (player1Points > player2Points)
                    {
                        pointText.text = player1Points.ToString();
                        pointText2.text = player2Points.ToString();
                        winner = GameManager.networkLevelManager.playersJoined[0];
                        loser = GameManager.networkLevelManager.playersJoined[1];
                    }
                    if (player2Points > player1Points)
                    {
                        pointText2.text = player1Points.ToString();
                        pointText.text = player2Points.ToString();
                        winner = GameManager.networkLevelManager.playersJoined[1];
                        loser = GameManager.networkLevelManager.playersJoined[0];
                    }
                    else if (player1Points == player2Points)
                    {
                        pointText.text = ("Tied");
                        loserText.text = ("Tied");
                        winner = GameManager.networkLevelManager.playersJoined[1];
                        loser = GameManager.networkLevelManager.playersJoined[0];
                    }
                }
                timeRemaining = 0;
                timerIsRunning = false;
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
        vStandPrefab.gameObject.SetActive(true);
        winner.gameObject.GetComponent<PlayerMovementCC>().enabled = false;
        winner.gameObject.transform.position = vStand.transform.position;
        winner.gameObject.transform.rotation = vStand.transform.rotation;
        if(GameManager.networkLevelManager.playersJoined.Count == 2)
        {
            loser.gameObject.GetComponent<PlayerMovementCC>().enabled = false;
            loser.gameObject.transform.position = loserVille.transform.position;
            loser.gameObject.transform.rotation = loserVille.transform.rotation;
        }
        //SceneManager.LoadScene(GameOverSceneIndex);

    }


}