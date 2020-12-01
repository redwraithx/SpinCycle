using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{

    public GameObject itemSpawnPoint;
    public float timer;
    public bool timerSabotage = false;
    public bool isWashing = false;
    public GameObject cleanLaundry;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject cleanLaundryPrefab;
    void Update()
    {
        if (timerSabotage == false)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0 && isWashing == true)
            {
                SpawnFinishedLaundry();
                isWashing = false;
            }
        }
   
    }

    public void SpawnFinishedLaundry()
    {
        GameObject Clone;
        Clone = (Instantiate(cleanLaundryPrefab, itemSpawnPoint.transform.position, Quaternion.identity));
    }

    public void WashClothes()
    {
        timer = 100;
        isWashing = true;

        //Add animation trigger here or in timer later
    }

    private void OnTriggerStay(Collider other)
    {

        Debug.Log("Press E to interact with laundry machine");

        //Alter this tag based on machine
        if (other.gameObject.CompareTag("Sock"))
        {

            //Revisit keybinds for next part later
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Once player is created, call to destroy the item in their hand here
                WashClothes();

            }
        }

    }
}
