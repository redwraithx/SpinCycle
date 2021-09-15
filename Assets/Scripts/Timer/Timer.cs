﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.IO;

public class Timer : MonoBehaviour
{
    [Header("For Point Counters"),]
    public TMP_Text player1PointCounter;
    public TMP_Text player2PointCounter;
    public GameObject pointCounters;
    public GameObject background;
    public GameObject panel;


    [Header("Timer Functions"), ] 
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public TMP_Text timeText;
    public Image GameOverImage;
    public Image BlackImage;
    public Animator anim;

    [Header("Autofill Sections"), ] 
    public float player1Points;
    public float player2Points;
    public NetworkedTimerNew networkedTimer;

    [Header("Victory Stand Items"),]
    bool isGameOver;
    public GameObject vStand;
    public GameObject loserVille;
    public TMP_Text pointTextL;
    public TMP_Text pointTextW;
    public TMP_Text victoryText;
    public GameObject vStandPrefab;
    public string modelName;
    public GameObject winnerModel;
    public GameObject loserModel;


    public int GameOverSceneIndex = 0;

    private void Start()
    {
        isGameOver = false;
        networkedTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<NetworkedTimerNew>();
    }
    public void UpdatePoints()
    {

        player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
        player1PointCounter.text = player1Points.ToString();

        if (GameManager.networkLevelManager.playersJoined.Count >= 2)
        {
            player2Points = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points;
            player2PointCounter.text = player2Points.ToString();
        }

    }
    void Update()
    {

        if (networkedTimer.currentMatchTime <= 0.1)
        {
            timerIsRunning = false;
        }
        else
        {
            timerIsRunning = true;
        }
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining = networkedTimer.currentMatchTime - 1f;
                DisplayTime(timeRemaining);
            }
            else
            {
                if (isGameOver == false)
                {
                    if (GameManager.networkLevelManager.playersJoined.Count < 2)
                    {
                        OnePlayerQuit();
                    }
                    else if (GameManager.networkLevelManager.playersJoined.Count == 2)
                    {
                        EndGame();
                    }
                    timeRemaining = 0;
                    timerIsRunning = false;
                    StartCoroutine(Fading());
                }
            }
        }

    }

    public void OnePlayerQuit()
    {
        player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
        pointTextW.text = player1Points.ToString();
        pointTextL.text = "Coward";
        victoryText.text = "Win by disconnect";
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void EndGame()
    {
        player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
        player2Points = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points;

        if (player1Points > player2Points)
        {
            pointTextW.text = player1Points.ToString();
            pointTextL.text = player2Points.ToString();
            victoryText.text = ("Player 1(Host) Wins");
        }
        if (player2Points > player1Points)
        {
            pointTextL.text = player1Points.ToString();
            pointTextW.text = player2Points.ToString();
            victoryText.text = ("Player 2(Joined) Wins");
        }
        else if (player1Points == player2Points)
        {
            pointTextL.text = player1Points.ToString();
            pointTextW.text = player2Points.ToString();
            victoryText.text = ("Tied");
        }
    }
    IEnumerator Fading()
    {
        yield return new WaitForSeconds(1);

        vStandPrefab.gameObject.SetActive(true);
        pointCounters.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);


        isGameOver = true;

    }


}