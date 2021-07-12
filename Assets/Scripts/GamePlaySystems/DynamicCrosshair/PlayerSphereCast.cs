using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereCast : MonoBehaviour
{
    public static event Action<GameObject> ObjectSelected;

    public GameObject currentHitObject = null;

    public float sphereRadius = 0f;
    public float maxDistance = 0f;
    public LayerMask layerMask;

    private Vector3 origin = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    public bool outOfRange = true;
    public bool itemInHand = false;

    private float currentHitDistance;

    public Grab grab;

    
    void Start()
    {
        if (maxDistance <= 0)
        {
            maxDistance = 10;
        }

        grab = GetComponent<Grab>();
    }


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

        if (currentHitObject != null && currentHitObject.gameObject.CompareTag("Item"))
        {
            grab.itemToPickUp = currentHitObject;
            grab.canPickUpItem = true;
        }

        else
        {
            grab.itemToPickUp = null;
            grab.canPickUpItem = false;
        }

        if (currentHitObject && currentHitObject.gameObject.CompareTag("Machine"))
        {
            grab.objectToInteractWith = currentHitObject;
        }

        // else if (currentHitObject && currentHitObject.gameObject.CompareTag("UsableObjects"))
        // {
        //     if (itemInHand == false)
        //         grab.objectToInteractWith = currentHitObject;
        // }

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

        if (currentHitObject)
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

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
