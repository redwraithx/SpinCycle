<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;

>>>>>>> main

public class ConveyerBelt : MonoBehaviour
{

    public GameObject belt;
    public Transform endPoint;
    public float speed;

    private void OnCollisionStay(Collision collision)
    {
<<<<<<< HEAD
        if (collision.gameObject.tag == "Item")
=======
        if (collision.gameObject.CompareTag("Item"))
>>>>>>> main
        {
            collision.transform.position = Vector3.MoveTowards(collision.transform.position, endPoint.position, speed * Time.deltaTime);
        }
    }



}
