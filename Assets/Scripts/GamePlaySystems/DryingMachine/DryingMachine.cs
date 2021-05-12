using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DryingMachine : MonoBehaviour
{
    public GameObject itemSpawnPoint;
    public float timer;
    public bool timerSabotage = false;
    public bool isDrying = false;
    public GameObject cleanLaundry;
    public Slider time;

    // Update is called once per frame
    void Update()
    {
        if (timerSabotage == false)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0 && isDrying == true)
            {
                SpawnFinishedLaundry();
                isDrying = false;
            }
        }
        time.value = timer;
    }

    public void SpawnFinishedLaundry()
    {
        GameObject Clone;
        Clone = (Instantiate(cleanLaundry, itemSpawnPoint.transform.position, Quaternion.identity));
    }

    public void DryClothes()
    {
        timer = 100;
        isDrying = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DryClothes();

                Destroy(other.gameObject);
            }
        }
    }
}
