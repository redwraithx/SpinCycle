using Photon.Pun;
using System.IO;
using UnityEngine;

public class BombDetonate : MonoBehaviourPun, IPunObservable
{
    public GameObject Bomb;
    public GameObject Radius = null;
    public string radiusName;
    public bool detonated;
    public float timer;
    public float timerAdjust;
    public PhotonView _photonView = null;

    public GameObject debugger;

    private void Start()
    {
        if (!_photonView)
        {
            _photonView = GetComponent<PhotonView>();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (CanDetonateObject(collision))
        {
            if (collision.gameObject.tag == "machine")
            {
                BroadcastMessage("SabotageMachine");
            }

            if (PhotonNetwork.IsMasterClient)
            {
                Radius = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", radiusName), transform.position, transform.rotation);
            }
            detonated = true;
        }
    }

    private bool CanDetonateObject(Collision other)
    {
        if (detonated || transform.parent != null)
            return false;

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("ItemStand")) // may need to reverse this
            return false;

        if (GetComponent<Item>().OwnerID == 0)
            return false;

        return true;
    }

    private void Update()
    {
        if (detonated == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }

        if (timer <= 0)
        {
            if (photonView.Owner.IsMasterClient == false)
            {
                GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
            }

            if (photonView.Owner.IsMasterClient == true)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(detonated);
        }
        if (stream.IsReading)
        {
            bool detonation = (bool)stream.ReceiveNext();

            if (detonated != detonation && detonation == true)
                detonated = detonation;
        }
    }
}