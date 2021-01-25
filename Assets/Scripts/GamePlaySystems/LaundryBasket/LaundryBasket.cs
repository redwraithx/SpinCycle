using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaundryBasket : MonoBehaviour
{

    public Text pointsText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            pointsText.text = (Convert.ToInt32(pointsText.text) + collision.gameObject.GetComponent<TShirt>().Price).ToString();

            Destroy(collision.gameObject);
        }
    }
}
