<<<<<<< HEAD
﻿using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerPoints : MonoBehaviour
{
    public Text playerPointText;
    public int points;

    private void Awake()
    {

    }
    private void Update()
    {
        playerPointText.text = points.ToString();
=======
﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class  PlayerPoints : MonoBehaviour
{
    public Text playerPointText = null;
    public int points = 0;

    public int Points
    {
        get => points;
        set
        {
            points = value;
            
           playerPointText.text = points.ToString();
        }
    }

    private void Start()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            //playerPointText = GameObject.FindWithTag("PointsCounter").GetComponent<Text>();
        }
    }
    private void Update()
    {
        
>>>>>>> main
    }
}
