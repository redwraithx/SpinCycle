using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructor : MonoBehaviour
{
    // THIS IS FOR TESTING ONLY
    public Text pointsText = null;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // THIS IS FOR TESTING ONLY
            pointsText.text = (Convert.ToInt32(pointsText.text) + collision.gameObject.GetComponent<TShirt>().Price).ToString();
        
        
            Destroy(collision.gameObject);
        }
    }
}
