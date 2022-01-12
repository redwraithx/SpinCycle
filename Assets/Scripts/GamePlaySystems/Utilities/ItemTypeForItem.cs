using UnityEngine;
using EnumSpace;
using Photon.Pun;
using Photon.Realtime;

namespace GamePlaySystems.Utilities
{
    public class ItemTypeForItem : MonoBehaviourPunCallbacks
    {
        public ItemType itemType;
        public LaundryType laundryType;

        private bool isItemHeld = false;

        private void Start()
        {
            PhotonNetwork.SendRate = 22;
            PhotonNetwork.SerializationRate = 22;
        }

        public void RequestOwnership()
        {
            if (!isItemHeld)
            {
                // get ownership of the object were about to pickup
                base.photonView.RequestOwnership();

                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                isItemHeld = true;
            }
        }

        public void RequestTransferOwnershipToHost()
        {
            // return ownership back to master client.
            if (!this.gameObject.GetComponent<BombDetonate>())
            {
                base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
            }

            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            // reset the ability to get the item
            isItemHeld = false;
        }
    }
}