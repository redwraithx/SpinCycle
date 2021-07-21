
using UnityEngine;
using UnityEngine.UI;

using GamePlaySystems.Utilities;
<<<<<<< HEAD

public class LaundryBasket : MonoBehaviour
=======
using System.Collections;

using Photon.Pun;
using Photon.Realtime;


public class LaundryBasket : MonoBehaviourPun
>>>>>>> main
{

    public Text pointsText = null;
    public int points;
    PlayerPoints playerPoints = null;
    public string pointsToText;
    
<<<<<<< HEAD
    // Start is called before the first frame update
    void Start()
    {
        playerPoints = GameObject.Find("PlayerCC").GetComponent<PlayerPoints>();
        points += playerPoints.points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* public void DepositLaundry(GameObject other)
     {
         if (other.CompareTag("Item"))
         {
             Debug.Log($"we have a item {other.GetComponent<ItemTypeForItem>().itemType}");
             points += other.gameObject.GetComponent<TShirt>().Price;

         }
     }*/
    
    

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "Item")
    //     {
    //         if (collision.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
    //         {
    //             points += collision.gameObject.GetComponent<TShirt>().Price;
    //             Debug.Log(points);
    //             collision.gameObject.SetActive(false);
    //         }
    //     }
    // }
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += other.gameObject.GetComponent<Item>().Price;
                Debug.Log(points);
                other.gameObject.SetActive(false);
=======
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
                other.gameObject.GetComponent<Item>().DisableObject();
>>>>>>> main
            }
        }
    }
    
<<<<<<< HEAD
    public void AddClothing(GameObject other)
    {
        if (other.tag == "Item")
        {
            if (other.gameObject.GetComponent<ItemTypeForItem>().itemType == ItemTypeForItem.ItemType.ClothingUnfolded)
            {
                points += other.gameObject.GetComponent<Item>().Price;
                
                Debug.Log(points);
                
                other.gameObject.SetActive(false);

                pointsToText = points.ToString();
                Debug.Log(pointsToText);
                pointsText.text = pointsToText;
                playerPoints.points += points;
            }
        }
    }
=======
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
                other.gameObject.GetComponent<Item>().DisableObject();
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
>>>>>>> main
}
