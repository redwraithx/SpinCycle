using UnityEngine;
using Photon.Pun;

public class OilCanPowerUp1 : MonoBehaviour
{
    public float speedMultiplier = 2.0f;
    public float duration = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    private void Pickup(Collider player)
    {
        //Debug.Log("Picked up a Speed Boost!");

        PlayerMovementCC stats = player.GetComponent<PlayerMovementCC>();
        stats.speedBoost = true;

        PhotonNetwork.Destroy(gameObject);
    }
}