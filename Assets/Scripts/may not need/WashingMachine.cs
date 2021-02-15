
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class WashingMachine : MonoBehaviour
{

    public GameObject itemSpawnPoint;
    public float timer;
    public bool timerSabotage = false;
    public bool isWashing = false;
    public GameObject cleanLaundry;
    public Slider time;


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
        time.value = timer;
    }

    public void SpawnFinishedLaundry()
    {
        GameObject Clone;
        Clone = (Instantiate(cleanLaundry, itemSpawnPoint.transform.position, Quaternion.identity));
    }

    public void WashClothes()
    {
        timer = 50;
        isWashing = true;

        //Add animation trigger here or in timer later if needed
        
    }

    public void UseMachine(GameObject other)
    {

        Debug.Log("if dirty item, it will be added to the machine");

        //Alter this tag based on machine
        if (other.CompareTag("Item"))
        {
            Debug.Log("we have a dirty item");
            cleanLaundry = other;
                
            //Once player is created, call to destroy the item in their hand here
            WashClothes();

            // we may want to use a bool incase the machine is full we dont destroy or use the object
            Destroy(other);
        }

    }
}
