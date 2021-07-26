using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedTimerNew : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public int matchLength = 185;
    public int currentMatchTime;

    private Coroutine timerCoroutine;
    public const byte refreshTimer = 1;
    

    //use for demonstration of effect
    public Text ui_timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void InitializeTimer()
    {
        Debug.Log("InitializingTimer");
        currentMatchTime = matchLength;
        RefreshTimerUI();

        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    private void EndGame()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();

        if (PhotonNetwork.IsMasterClient)
        {

        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;

        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
        }
        else
        {
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent(
            (byte)refreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }

    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }

    public void OnEvent (EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            object[] o = (object[])photonEvent.CustomData;
            RefreshTimer_R(o);
        }
    }

    private void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("0");
        string seconds = (currentMatchTime % 60).ToString("00");
        ui_timer.text = $"{minutes}:{seconds}";
    }
}

