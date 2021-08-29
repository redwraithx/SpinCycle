using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class IceCube : MonoBehaviourPun
{

    public List<GameObject> currentHitObjects = new List<GameObject>();
    public LayerMask layerMask;
    public BoxCollider boxCollider;

    bool spawnedPatch = false;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider.enabled = false;
        Invoke("StartSphereCast", 0.3f);
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

        Debug.Log(transform.position);
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
                if (spawnedPatch == false)
                {
                    //GetComponentInChildren<VFX_IceBullet_DissolveIn>().InsantiateCollideVFX(collision);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "IcePatch"), transform.position, transform.rotation);
                    spawnedPatch = true;
                }
            }

            if (hit.transform.gameObject.layer == 8)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    if(spawnedPatch == false)
                    {
                        //GetComponentInChildren<VFX_IceBullet_DissolveIn>().InsantiateCollideVFX(collision);
                        PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "IcePatch"), transform.position, transform.rotation);
                        spawnedPatch = true;
                    }

                }

            }
        }
        GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4.0f);
    }


    void StartSphereCast()
    {
        boxCollider.enabled = true;
    }

   
}
