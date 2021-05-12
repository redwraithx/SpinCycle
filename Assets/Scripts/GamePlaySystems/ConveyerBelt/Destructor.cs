<<<<<<< HEAD
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
=======
﻿using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

>>>>>>> main

public class Destructor : MonoBehaviour
{

<<<<<<< HEAD
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {


            collision.gameObject.SetActive(false);
=======
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            collision.gameObject.GetComponent<Item>().DisableObject();
>>>>>>> main
        }
    }
}
