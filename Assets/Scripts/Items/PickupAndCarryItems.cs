using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndCarryItems : MonoBehaviour
{
    RaycastHit hit;
    GameObject _items;
    public Transform grabPos;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out hit, 5) && hit.transform.GetComponent<Rigidbody>())
        {
            _items = hit.transform.gameObject;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _items = null;
        }
        if(_items)
        {
            _items.GetComponent<Rigidbody>().velocity = 10 * (grabPos.position - _items.transform.position);
        }
    }
}
