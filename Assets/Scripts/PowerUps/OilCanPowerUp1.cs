using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCanPowerUp1 : MonoBehaviour
{
    public float speedMultiplier = 2.0f;
    public float duration = 5.0f;
    float blinktime = 0;
    float turnOffTime;
    GameObject light; 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    private void Update()
    {
        GameObject light = GameObject.FindGameObjectWithTag("Light");
        if (turnOffTime >= 0)
        {
            turnOffTime -= Time.deltaTime;
        }
        if (turnOffTime < 0)
            light.active = true;

    }

    void Pickup(Collider player)
    {
        Debug.Log("Picked up a Speed Boost!");
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        GameObject light = GameObject.FindGameObjectWithTag("Light");
        light.active = false;
        turnOffTime = Time.deltaTime;
       // light.active = true;
        PlayerMovementCC stats = player.GetComponent<PlayerMovementCC>();
        stats.Xspeed *= speedMultiplier;

        Destroy(gameObject);
    }


}
