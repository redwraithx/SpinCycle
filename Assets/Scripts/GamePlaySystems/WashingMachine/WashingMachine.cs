
using UnityEngine;
using UnityEngine.UI;
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
        timer = 100;
        isWashing = true;

        //Add animation trigger here or in timer later
    }

    private void OnTriggerStay(Collider other)
    {

        Debug.Log("Press E to interact with laundry machine");

        //Alter this tag based on machine
        if (other.gameObject.CompareTag("Item"))
        {

            //Revisit keybinds for next part later
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Once player is created, call to destroy the item in their hand here
                WashClothes();

                
                Destroy(other.gameObject);
            }
        }

    }
}
