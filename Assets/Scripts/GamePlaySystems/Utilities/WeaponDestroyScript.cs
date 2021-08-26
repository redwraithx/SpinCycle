using UnityEngine;
using System.Collections;
using Photon.Pun;


public class WeaponDestroyScript : MonoBehaviourPun
{
    internal bool hasFired = false;
    [Range(0.5f, 5f)] public float delayedTime = 2f;

    public LineRenderer lineRenderer;

    void LateUpdate()
    {
        if(hasFired)
        {
            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }

        if (hasFired && transform.parent == null)
        {
            
            Debug.Log("Destroyed Gun");

            if (photonView.Owner.IsMasterClient == false)
            {
                GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
            }

            if (photonView.Owner.IsMasterClient == true)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(DelayedDestroy(delayedTime));
                }
            }
                
        }


    }

    private IEnumerator DelayedDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        PhotonNetwork.Destroy(gameObject);
    }
}
