﻿using UnityEngine;
using System.Collections;
using Photon.Pun;


public class WeaponDestroyScript : MonoBehaviourPun
{
    internal bool hasFired = false;
    [Range(0.5f, 5f)] public float delayedTime = 2f;

    void LateUpdate()
    {
        if (hasFired && transform.parent == null)
        {
            Debug.Log("Destroyed Gun");

            if (GetComponent<PhotonView>().Owner.IsMasterClient)
            {
                StartCoroutine(DelayedDestroy(delayedTime));
            }
                
        }


    }

    private IEnumerator DelayedDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        PhotonNetwork.Destroy(gameObject);
    }
}
