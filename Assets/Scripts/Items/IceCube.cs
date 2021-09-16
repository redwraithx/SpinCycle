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
    bool playingAudio = false;

    //public GameObject icePatch;
    // Start is called before the first frame update
    void Start()
    {
        //boxCollider.enabled = false;
/*        Invoke("StartSphereCast", 0.02f);*/
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
        if(collision.gameObject.tag != "Item")
        {
            if(playingAudio == false)
            {
                AudioClip freezeCollison = Resources.Load<AudioClip>("AudioFiles/SoundFX/Sabotages/FreezeGun/freeze");
                GameManager.audioManager.PlaySfx(freezeCollison);
                playingAudio = true;
            }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4);
            //RaycastHit[] hits = Physics.SphereCastAll(transform.position, 4.0f, transform.forward, 1.0f, layerMask, QueryTriggerInteraction.UseGlobal);

            foreach (var hitCollider in hitColliders)
            {

                Debug.Log("hit");
                if (hitCollider.transform.gameObject.tag == "Player")
                {
                    /*foreach(GameObject player in GameManager.networkLevelManager.playersJoined)
                    {
                        if(hitCollider.transform.gameObject == player)
                        {
                            player.transform.gameObject.GetComponent<PlayerMovementCC>().isFrozen = true;
                        }
                    }*/

                    foreach (GameObject player in GameManager.networkLevelManager.playersJoined)
                    {
                        if (player.GetComponent<PhotonView>().ViewID == hitCollider.transform.gameObject.GetComponent<PhotonView>().ViewID)
                        {
                            player.GetComponent<PlayerMovementCC>().isFrozen = true;
                        }
                    }
/*                    hitCollider.transform.gameObject.GetPhotonView();
                    hitCollider.transform.gameObject.GetComponent<PlayerMovementCC>().isFrozen = true;*/

                }

            }

            //Instantiate(icePatch, transform.position, transform.rotation);
            if (PhotonNetwork.IsMasterClient)
            {
                if (spawnedPatch == false)
                {
                    //GetComponentInChildren<VFX_IceBullet_DissolveIn>().InsantiateCollideVFX(collision);
                    PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "IcePatch"), transform.position, transform.rotation);
                    spawnedPatch = true;
                }

            }
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
            PhotonNetwork.Destroy(gameObject);
            //Destroy(gameObject);
        }

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
