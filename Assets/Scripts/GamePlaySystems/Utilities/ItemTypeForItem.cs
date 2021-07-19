using UnityEngine;
using EnumSpace;
<<<<<<< HEAD
=======
using Photon.Pun;
using Photon.Realtime;

>>>>>>> main

namespace GamePlaySystems.Utilities
{

<<<<<<< HEAD
    public class ItemTypeForItem : MonoBehaviour
    {

        public ItemType itemType;
        public LaundryType laundryType;

        public enum ItemType
        {
            None,
            ClothingDirty,
            ClothingWet,
            ClothingUnfolded,
            ClothingDone,
            SabotageWaterGun,
            SabotageClothing
        }
    }

    
=======
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
            Debug.Log("give ownership back to host");
        
            // return ownership back to master client.
            base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
            
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            
            // reset the ability to get the item 
            isItemHeld = false;
        }
        
    }

    
    
>>>>>>> main
}