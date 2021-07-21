using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereCast : MonoBehaviour
{
    public static event Action<GameObject> ObjectSelected;

<<<<<<< HEAD
    public GameObject currentHitObject;

    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;
=======
    public GameObject currentHitObject = null;

    public float sphereRadius = 0f;
    public float maxDistance = 0f;
    public LayerMask layerMask;

    private Vector3 origin = Vector3.zero;
    private Vector3 direction = Vector3.zero;
>>>>>>> main

    public bool outOfRange = true;
    public bool itemInHand = false;

    private float currentHitDistance;

    public Grab grab;

<<<<<<< HEAD
    // Start is called before the first frame update
=======
    
>>>>>>> main
    void Start()
    {
        if (maxDistance <= 0)
        {
            maxDistance = 10;
        }

        grab = GetComponent<Grab>();
    }

<<<<<<< HEAD
    // Update is called once per frame
=======

>>>>>>> main
    void Update()
    {
        if (itemInHand == false)
        {
            maxDistance = 2;
            sphereRadius = 0.5f;
            layerMask = LayerMask.GetMask("Items");
        }
        else
        {
            maxDistance = 4;
            sphereRadius = 1;
            layerMask = LayerMask.GetMask("Usable Objects");
        }

        origin = transform.position;
        direction = transform.forward;

<<<<<<< HEAD
        if (currentHitObject != null && currentHitObject.gameObject.tag == "Item")
=======
        if (currentHitObject != null && currentHitObject.gameObject.CompareTag("Item"))
>>>>>>> main
        {
            grab.itemToPickUp = currentHitObject;
            grab.canPickUpItem = true;
        }

        else
        {
            grab.itemToPickUp = null;
            grab.canPickUpItem = false;
        }

<<<<<<< HEAD
        if (currentHitObject != null && currentHitObject.gameObject.tag == "Machine")
=======
        if (currentHitObject && currentHitObject.gameObject.CompareTag("Machine"))
>>>>>>> main
        {
            grab.objectToInteractWith = currentHitObject;
        }

<<<<<<< HEAD
        else if (currentHitObject != null && currentHitObject.gameObject.tag == "UsableObjects")
        {
            if (itemInHand == false)
                grab.objectToInteractWith = currentHitObject;
        }
=======
        // else if (currentHitObject && currentHitObject.gameObject.CompareTag("UsableObjects"))
        // {
        //     if (itemInHand == false)
        //         grab.objectToInteractWith = currentHitObject;
        // }
>>>>>>> main

        else
        {
            grab.objectToInteractWith = null;
        }

        

        RaycastHit hit;

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }

        else
        {
            currentHitDistance = maxDistance;
        }

<<<<<<< HEAD
        if (currentHitObject != null)
=======
        if (currentHitObject)
>>>>>>> main
        {
            if (Vector3.Distance(transform.position, currentHitObject.transform.position) <= maxDistance)
            {
                outOfRange = false;
                grab.outOfRange = false;
                if (ObjectSelected != null)
                    ObjectSelected(currentHitObject);
            }
            else
            {
                outOfRange = true;
                grab.outOfRange = true;
            }

        }
    }

<<<<<<< HEAD
=======
    
>>>>>>> main
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
