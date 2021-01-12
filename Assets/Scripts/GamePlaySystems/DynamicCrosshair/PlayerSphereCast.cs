using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereCast : MonoBehaviour
{
    public static event Action<GameObject> ObjectSelected;

    public GameObject currentHitObject;

    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;

    public bool outOfRange = true;
    public bool itemInHand = false;

    private float currentHitDistance;

    public Grab grab;

    // Start is called before the first frame update
    void Start()
    {
        if (maxDistance <= 0)
        {
            maxDistance = 10;
        }

        grab = GetComponent<Grab>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemInHand == false)
        {
            maxDistance = 3;
            sphereRadius = 1;
            layerMask = LayerMask.GetMask("Items");
        }
        else
        {
            maxDistance = 7;
            sphereRadius = 3;
            layerMask = LayerMask.GetMask("Usable Objects");
        }

        origin = transform.position;
        direction = transform.forward;

        if (currentHitObject != null && currentHitObject.gameObject.tag == "Item")
        {
            grab.itemToPickUp = currentHitObject;
            grab.canPickUpItem = true;
        }

        else
        {
            grab.itemToPickUp = null;
            grab.canPickUpItem = false;
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

        if (currentHitObject != null)
        {
            if (Vector3.Distance(transform.position, currentHitObject.transform.position) <= maxDistance)
            {
                outOfRange = false;
                if (ObjectSelected != null)
                    ObjectSelected(currentHitObject);
            }
            else
            {
                outOfRange = true;
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
