using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroGameText : MonoBehaviour
{
    public TextMeshProUGUI countDown;
    public float tempTime = 5f;
    public int timeTranslate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rewrite this when networking is working
       /* if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 185)
        {
            countDown.text = "5!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 184)
        {
            countDown.text = "4!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 183)
        {
            countDown.text = "3!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 182)
        {
            countDown.text = "2!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 181)
        {
            countDown.text = "1!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 180)
        {
            countDown.text = "GO!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime <= 178)
        {
            gameObject.SetActive(false);
        }*/

        tempTime -= Time.deltaTime;
        timeTranslate = Mathf.RoundToInt(tempTime);
        if (timeTranslate > 0)
            countDown.text = timeTranslate + "!";
        else if (tempTime > -2)
            countDown.text = "GO!";
        else
            gameObject.SetActive(false);
    }
}
