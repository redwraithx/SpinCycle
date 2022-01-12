using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[System.Serializable]
public class PlayerPoints : MonoBehaviourPun, IPunObservable
{
    public PhotonView _photonView = null;
    public TMP_Text playerPointText = null;
    public int points = 0;
    public GameObject Timer;

    private void Awake()
    {
        Timer = GameObject.FindGameObjectWithTag("MainTimer");

        if (!_photonView)
        {
            _photonView = GetComponent<PhotonView>();
        }
    }

    public int Points
    {
        get => points;
        set
        {
            points = value;
            Debug.Log("New Points Inbound");
            Timer.BroadcastMessage("UpdatePoints");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(points);
        }
        else if (stream.IsReading)
        {
            int netpoints = (int)stream.ReceiveNext();

            if (netpoints > points)
                Points = netpoints;

            Debug.Log(netpoints + "is the sent points value");
        }
    }
}