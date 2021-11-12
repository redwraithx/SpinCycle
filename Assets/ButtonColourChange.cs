using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColourChange : MonoBehaviour
{
    public Button thisButton;

    private void Start()
    {
        thisButton.GetComponent<Image>().color = Color.red;
    }

    

    public void ColourChange()
    {
        if(thisButton.GetComponent<Image>().color == Color.red)
        {
            thisButton.GetComponent<Image>().color = Color.green;
            thisButton.GetComponentInChildren<Text>().text = "Ready";
            thisButton.GetComponentInChildren<Text>().color = Color.black;
        }
        else if (thisButton.GetComponent<Image>().color == Color.green)
        {
            thisButton.GetComponent<Image>().color = Color.red;
            thisButton.GetComponentInChildren<Text>().text = "Push To Ready";
            thisButton.GetComponentInChildren<Text>().color = Color.white;
        }
    }
}
