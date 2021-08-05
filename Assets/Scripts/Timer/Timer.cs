using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System.IO;

public class Timer : MonoBehaviour
{
    [Header("For Point Counters"),]
    public TMP_Text player1PointCounter;
    public TMP_Text player2PointCounter;
    public GameObject pointCounters;


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
    public string player1Name;
    public string player2Name;
    public GameObject winner;
    public GameObject loser;
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
        // Starts the timer automatically
        networkedTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<NetworkedTimerNew>();
    }
    public void UpdatePoints()
    {
        Debug.Log("Points updating");

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
                        player1Points = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points;
                        player1Name = GameManager.networkLevelManager.playersJoined[0].name;
                        winner = GameManager.networkLevelManager.playersJoined[0];
                        pointTextW.text = player1Points.ToString();
                        pointTextL.text = "Coward";
                        victoryText.text = "Win by disconnect";
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
        player1Name = GameManager.networkLevelManager.playersJoined[0].name;
        player2Name = GameManager.networkLevelManager.playersJoined[1].name;

        if (player1Points > player2Points)
        {
            pointTextW.text = player1Points.ToString();
            pointTextL.text = player2Points.ToString();
            winner = GameManager.networkLevelManager.playersJoined[0];
            loser = GameManager.networkLevelManager.playersJoined[1];
        }
        if (player2Points > player1Points)
        {
            pointTextL.text = player1Points.ToString();
            pointTextW.text = player2Points.ToString();
            winner = GameManager.networkLevelManager.playersJoined[1];
            loser = GameManager.networkLevelManager.playersJoined[0];
        }
        else if (player1Points == player2Points)
        {
            pointTextL.text = player1Points.ToString();
            pointTextW.text = player2Points.ToString();
            victoryText.text = ("Tied");
            winner = GameManager.networkLevelManager.playersJoined[1];
            loser = GameManager.networkLevelManager.playersJoined[0];
        }
    }
    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => BlackImage.color.a == 1);

        vStandPrefab.gameObject.SetActive(true);
        pointCounters.gameObject.SetActive(false);
        winner.gameObject.GetComponent<PlayerMovementCC>().enabled = false;
        if (PhotonNetwork.IsMasterClient)
        {
            winnerModel = PhotonNetwork.Instantiate(Path.Combine("NetworkedSceneObjects", modelName), vStand.transform.position, vStand.transform.rotation);
        }
        if (GameManager.networkLevelManager.playersJoined.Count == 2)
        {
            loser.gameObject.GetComponent<PlayerMovementCC>().enabled = false;
            if (PhotonNetwork.IsMasterClient)
            {
                loserModel = PhotonNetwork.Instantiate(Path.Combine("NetworkedSceneObjects", modelName), loserVille.transform.position, loserVille.transform.rotation);
            }
        }

        isGameOver = true;
        //SceneManager.LoadScene(GameOverSceneIndex);

    }


}