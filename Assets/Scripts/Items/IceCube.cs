using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{

    public List<GameObject> currentHitObjects = new List<GameObject>();
    public GameObject slipperyBit;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartSphereCast", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
/*        if (isShooting == true)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 4.0f, transform.forward, 1.0f, layerMask, QueryTriggerInteraction.UseGlobal);
            foreach (RaycastHit hit in hits)
            {
                currentHitObjects.Add(hit.transform.gameObject);
            }
        }*/
    }


    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 4.0f, transform.forward, 1.0f, layerMask, QueryTriggerInteraction.UseGlobal);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log("hit");
            if (hit.transform.gameObject.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<PlayerMovementCC>().isFrozen = true;
            }

            if (hit.transform.gameObject.layer == 8)
            {
                
                GameObject ice = Instantiate(slipperyBit, transform.position, slipperyBit.transform.rotation);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4.0f);
    }
}
