using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


public class Destructor : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            collision.gameObject.GetComponent<Item>().DisableObject();
        }
    }
}
