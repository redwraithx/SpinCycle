using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIDebugger : MonoBehaviourPun
{
    public GameObject P1;
    public Text P1Points;
    public Text P1Name;

    public GameObject P2;
    public Text P2Points;
    public Text P2Name;

    public Text[] itemInfo;
    public int itemsStored;
    public Text iInfoFull;
    public Text[] machineInfo;
    public int machinesStored;
    public Text mInfoFull;

    public Text fpsCounter;
    public int fps;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.uiDebugger = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.networkLevelManager.playersJoined.Count > 0)
        {
            P1 = GameManager.networkLevelManager.playersJoined[0];
            P1Points.text = GameManager.networkLevelManager.playersJoined[0].GetComponent<PlayerPoints>().points.ToString();
            P1Name.text = GameManager.networkLevelManager.playersJoined[0].GetComponent<PhotonView>().Owner.NickName;



            P2 = GameManager.networkLevelManager.playersJoined[1];
            P2Points.text = GameManager.networkLevelManager.playersJoined[1].GetComponent<PlayerPoints>().points.ToString();
            P2Name.text = GameManager.networkLevelManager.playersJoined[1].GetComponent<PhotonView>().Owner.NickName;
        }

        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fps = (int)current;
        fpsCounter.text = fps.ToString();
    }

    public void ItemInfo(int id, string name, bool owned)
    {

        itemsStored += 1;

        if (itemsStored <= 6)
        {
            itemInfo[itemsStored -= 1].text = (id + " " + name + "is owned by master?" + owned);
        }
        else if(itemsStored >6)
        {
            iInfoFull.text = "Full On Items";
        }
    }

    public void MachineInfo(int id, bool effect)
    {
        machinesStored += 1;
        if (machinesStored <= 6)
        {

            machineInfo[machinesStored -= 1].text = (id + "is effect applied? " + effect);
        }
        else if(machinesStored > 6)
        {
            mInfoFull.text = "Full On Machines";
        }
    }
}
