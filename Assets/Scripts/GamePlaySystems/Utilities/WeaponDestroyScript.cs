using UnityEngine;
using System.Collections;
using Photon.Pun;

public class WeaponDestroyScript : MonoBehaviourPun
{
    internal bool hasFired = false;
    [Range(0.5f, 5f)] public float delayedTime = 1f;

    public LineRenderer lineRenderer;

    private void LateUpdate()
    {
        if (hasFired)
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
                    Invoke("DestroyGun", 1f);
                }
            }
        }
    }

    private void DestroyGun()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private IEnumerator DelayedDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
        PhotonNetwork.Destroy(gameObject);
    }
}