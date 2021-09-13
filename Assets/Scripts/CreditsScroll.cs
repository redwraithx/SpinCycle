using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NetworkProfile;
using PlayerProfileData;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;


public class CreditsScroll : MonoBehaviour
{
    public GameObject creditsScroll;
    public GameObject creditsBottom;
    public GameObject creditsTop;
    public Vector3 creditsTopInitLocation;
    public Vector3 creditsBottomLocation;
    public float speed;

    public TMP_Text endOfCreditsText = null;


    private ProfileData myProfile = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        creditsTopInitLocation = creditsTop.transform.position;

        endOfCreditsText.text = GetAndSetEndOfCredits();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        creditsBottomLocation = creditsBottom.transform.position;

        if(Mathf.Abs(Vector3.Distance(creditsTopInitLocation, creditsBottomLocation)) >= 50)
        {
            creditsScroll.transform.Translate(Vector3.up * speed);
        }
        

    }
    
    
    private string GetCurrentPlayersName()
    {
        myProfile = DataClass.LoadProfile();
        if (myProfile != null && !string.IsNullOrEmpty(myProfile.userName))
        {
            return myProfile.userName;
        }


        return "Player";
    }

    private string GetAndSetEndOfCredits()
    {
        string playerName = GetCurrentPlayersName();

        StringBuilder endingString = new StringBuilder();
        endingString.AppendFormat("\n\n\n\n\n\n\nThank you\n\n{0}\n\nFor Playing Our Game.\n\n\nPress any Key to return to menu\n", playerName);

        return endingString.ToString();
    }
    
}
