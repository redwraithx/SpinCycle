using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        creditsTopInitLocation = creditsTop.transform.position;
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
}
