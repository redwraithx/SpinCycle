using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSphereCast : MonoBehaviour
{
    public static event Action<GameObject> ObjectSelected;

    public GameObject currentHitObject = null;

    public float sphereRadius = 5f;
    public float maxDistance = 0f;
    public LayerMask layerMask;

    private Vector3 origin = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    public bool outOfRange = true;


    private float currentHitDistance;

    public Grab grab;
    public PlayerMovementCC playerMovement;
    public bool justGotHit = false;
    public bool stopDive = false;

    void Start()
    {
        if (maxDistance <= 0)
        {
            maxDistance = 2;
        }

        grab = GetComponent<Grab>();
        playerMovement = GetComponent<PlayerMovementCC>();
    }

    private IEnumerator DiveCoroutine()
    {
        Debug.Log("diving coroutine");

        //GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex] = true;

        float startTime = Time.time; // need to remember this to know how long to dash
        while (Time.time < startTime + 2f)
        {

            // or controller.Move(...), dunno about that script
            yield return null; // this will make Unity stop here and continue next frame
        }


        //yield return new WaitForSeconds(GameManager.networkLevelManager.initialDiveReuseDelay);
        yield return new WaitForSeconds(10f);
        //GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex] = false;





    }
    void Update()
    {






        origin = transform.position;
        direction = transform.forward;

        if (currentHitObject && currentHitObject.gameObject.CompareTag("Wall"))
        {
            stopDive = true;
            playerMovement.cooldown = false;
            justGotHit = true;
        }

        else if (currentHitObject == null && justGotHit == true)
        {
            justGotHit = false;
            stopDive = false;
            //StartCoroutine(DiveCoroutine());
            playerMovement.cooldown = true;
        }





        RaycastHit hit;

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {

            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            if (currentHitObject && !currentHitObject.gameObject.CompareTag("Wall"))
            {
                currentHitObject = null;
                currentHitDistance = maxDistance;
            }
        }

        else
        {
            currentHitObject = null;
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
        else
        {
            outOfRange = true;

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
