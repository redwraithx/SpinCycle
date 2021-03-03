using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour

    
{

    private Collider collider;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Machine") || collision.gameObject.CompareTag("VendingMachine"))
        {
            Physics.IgnoreCollision(collision.collider, collider);
        }
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

}
