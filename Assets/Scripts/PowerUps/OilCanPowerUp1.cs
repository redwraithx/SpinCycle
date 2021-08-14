using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OilCanPowerUp1 : MonoBehaviour
{
    public float speedMultiplier = 2.0f;
    public float duration = 5.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        Debug.Log("Picked up a Speed Boost!");

        PlayerMovementCC stats = player.GetComponent<PlayerMovementCC>();
        stats.speedBoost = true;

        PhotonNetwork.Destroy(gameObject);
    }
}
