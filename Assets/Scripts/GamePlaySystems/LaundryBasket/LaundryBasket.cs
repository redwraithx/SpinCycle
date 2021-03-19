﻿
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;
using System.Collections;

public class LaundryBasket : MonoBehaviour
{

    public Text pointsText = null;
    public int points;
    PlayerPoints playerPoints = null;
    public string pointsToText;
    
    void Start()
    {
        StartCoroutine (CheckForPlayer());
    }

    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemType.ClothingDone)
            {
                points = other.gameObject.GetComponent<Item>().Price;
                Debug.Log(points);

                //above two lines aren't needed for this code to run, only for debugging purposes

                playerPoints.Points += other.gameObject.GetComponent<Item>().Price;
                other.gameObject.SetActive(false);
            }
        }
    }
    
    // below is the function to click items in which is needed for when the actual models come in place, above is dropping them in
    public void AddClothing(GameObject other)
    {
        if (other.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemType.ClothingDone)
            {
                points = other.gameObject.GetComponent<Item>().Price;                
                Debug.Log(points);

                //above two lines aren't needed for this code to run, only for debugging purposes

                playerPoints.Points += other.gameObject.GetComponent<Item>().Price;
                other.gameObject.SetActive(false);

                //pointsToText = points.ToString();
                //Debug.Log(pointsToText);
                //pointsText.text = pointsToText;
                //playerPoints.Points += points;
            }
        }
    }


    IEnumerator CheckForPlayer()
    {
        yield return new WaitForSeconds(2f);

            if(!GameManager.Instance.Player1)
            {
                StartCoroutine (CheckForPlayer());
            }
            else
            {
                playerPoints = GameManager.Instance.Player1.GetComponent<PlayerPoints>();
                StopCoroutine(CheckForPlayer());
            }
    }
}
