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
            Debug.Log("request ownership from host");

            if (isItemHeld)
                return;
            
            // get ownership of the object were about to pickup
            base.photonView.RequestOwnership();

            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            

            isItemHeld = true;

        }

        public void RequestTransferOwnershipToHost()
        {
            Debug.Log("give ownership back to host");
        
            // return ownership back to master client.
            base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
            
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            
            // reset the ability to get the item 
            isItemHeld = false;
        }
        
    }

    
    
}