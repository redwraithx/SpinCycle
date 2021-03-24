using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPSphereFunctions : MonoBehaviour
{
    private GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.gameObject;
        Debug.Log(thisObject + "IS THE ITEM");
    }

    // Update is called once per frame
    void Update()
    {
        if(!thisObject.transform.IsChildOf(transform))
        {
            Debug.Log("sphere isnt child");
            thisObject.transform.GetComponent<Rigidbody>().isKinematic = false;
            thisObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }
    }
}
