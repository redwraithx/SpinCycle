using UnityEngine;
using TMPro;

public class IntroGameText : MonoBehaviour
{
    public TextMeshProUGUI countDown;
    public float tempTime = 5f;
    public int timeTranslate;

    private void Update()
    {
        //rewrite this when networking is working
        if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 600)
        {
            countDown.text = "5!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 599)
        {
            countDown.text = "4!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 598)
        {
            countDown.text = "3!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 597)
        {
            countDown.text = "2!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 596)
        {
            countDown.text = "1!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime == 595)
        {
            countDown.text = "GO!";
        }
        else if (GameManager.networkLevelManager.timer.GetComponent<NetworkedTimerNew>().currentMatchTime <= 594)
        {
            gameObject.SetActive(false);
        }

        /*tempTime -= Time.deltaTime;
        timeTranslate = Mathf.RoundToInt(tempTime);
        if (timeTranslate > 0)
            countDown.text = timeTranslate + "!";
        else if (tempTime > -2)
            countDown.text = "GO!";
        else
            gameObject.SetActive(false);*/
    }
}