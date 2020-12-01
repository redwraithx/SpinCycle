using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{

    public GameObject belt;
    public Transform endPoint;
    public float speed;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Laundry")
        {
            collision.transform.position = Vector3.MoveTowards(collision.transform.position, endPoint.position, speed * Time.deltaTime);
        }
    }



}
