
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePlaySystems.Utilities;
using System.Collections;

using Photon.Pun;
using Photon.Realtime;


public class LaundryBasket : MonoBehaviourPun
{

    public TMP_Text pointsText = null;
    public int points;
    PlayerPoints playerPoints = null;
    public string pointsToText;
    
    void Start()
    {
        //StartCoroutine (CheckForPlayer());
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
                //playerPoints = PhotonView.Find(other.gameObject.GetComponent<Item>().OwnerID).GetComponent<PlayerPoints>();
                
                bool updatedPlayerPoints = UpdatePlayerPoints(other.gameObject);
                
                if(updatedPlayerPoints)
                    Debug.Log("Players Points where updated");
                else
                    Debug.Log("Players Points were not found to be updated.");
                
                
                points = other.gameObject.GetComponent<Item>().Price;
                Debug.Log(points);

                
                //above two lines aren't needed for this code to run, only for debugging purposes

                playerPoints.Points += other.gameObject.GetComponent<Item>().Price;
                //other.gameObject.GetComponent<Item>().DisableObject();
                PhotonNetwork.Destroy(other.gameObject);
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
                // playerPoints =  PhotonView.Find(other.gameObject.GetComponent<Item>().OwnerID).GetComponent<PlayerPoints>();  // this causes errors randomly, should be done in GameManager
                bool updatedPlayerPoints = UpdatePlayerPoints(other);
                
                if(updatedPlayerPoints)
                    Debug.Log("Players Points where updated");
                else
                    Debug.Log("Players Points were not found to be updated.");
                
               
                points = other.gameObject.GetComponent<Item>().Price;
                Debug.Log(points);

                //above two lines aren't needed for this code to run, only for debugging purposes

                playerPoints.Points += other.gameObject.GetComponent<Item>().Price;
                //other.gameObject.GetComponent<Item>().DisableObject();
                PhotonNetwork.Destroy(other.gameObject);
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



    private bool UpdatePlayerPoints(GameObject other)
    {
        PlayerPoints playerPointsReference = PhotonView.Find(other.gameObject.GetComponent<Item>().OwnerID).GetComponent<PlayerPoints>();

        return playerPoints = playerPointsReference;
    }
    
}
