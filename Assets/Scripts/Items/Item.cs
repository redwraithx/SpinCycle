
using System;
using System.IO;
using System.Security.AccessControl;
using UnityEngine;

using Photon;
using Photon.Realtime;
using Photon.Pun;


public class GetLocationData
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public PhotonStream stream;
    public PhotonMessageInfo info;

    public GetLocationData(int id, Vector3 position, Quaternion rotation)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}



[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonRigidbodyView))]
//[RequireComponent(typeof(PhotonTransformView))]
public class Item : MonoBehaviourPunCallbacks, IPunObservable, IItem
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAdjustment;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _ownerId;

    private PhotonView _photonView = null;
    private PhotonTransformView _photonTransformView = null;
    [SerializeField] private Vector3 correctPosition = Vector3.zero;
    [SerializeField] private Quaternion correctRotation = Quaternion.identity;

    [SerializeField] private Transform originalParent = null;

    private Rigidbody rb = null;
    
    
    private void Awake()
    {
        if(!_photonView)
            _photonView = GetComponent<PhotonView>();
        
        // if(!_photonTransformView)
        //     _photonTransformView = GetComponent<PhotonTransformView>();

        if (!rb)
            rb = GetComponent<Rigidbody>();

        if (!originalParent)
            originalParent = transform.parent;
        
        gameObject.tag = "Item";

        gameObject.layer = LayerMask.NameToLayer("Items");

        PhotonNetwork.SendRate = 20;
        
        
    }

    private void OnEnable()
    {
        foreach(Collider collider in GetComponents<Collider>())
        {
            if(!collider.enabled)
            {
                collider.enabled = true;
            }
        }
        
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    

    private void Update()
    {
        //UpdatePosition();
        //_photonView.RPC("UpdateObject", RpcTarget.AllBuffered, transform.position, transform.rotation);

    }

    // [PunRPC]
    // public void UpdateObject(Vector3 pos, Quaternion rot)
    // {
    //     transform.position = pos;
    //     transform.rotation = rot;
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (!_photonView)
    //         return;
    //
    // }


    

    public Item(int id, string name, string description, int price, float _time)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        TimeAdjustment = _time;
    }

    public Item()
    {

    }

    public int Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string Name
    {
        get => _name;
        private set => _name = value;
    }

    public string Description
    {
        get => _description;
        private set => _description = value;
    }

    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public float TimeAdjustment
    {
        get => _timeAdjustment;
        private set => _timeAdjustment = value;
    }

    public Sprite sprite
    {
        get => _sprite;
        private set => _sprite = value;
    }

    public int OwnerID
    {
        get => _ownerId;
        set => _ownerId = value;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(transform);
            //stream.SendNext(transform.rotation);
            
            stream.SendNext(rb.useGravity);
            stream.SendNext(rb.constraints);
            stream.SendNext(OwnerID);
            
        }
        else if(stream.IsReading)
        {
            //correctPosition = 
            //var newParent = (Transform) stream.ReceiveNext();
            
            //transform.SetParent(newParent);
            //correctRotation = (Quaternion) stream.ReceiveNext();

            rb.useGravity = (bool) stream.ReceiveNext();
            rb.constraints = (RigidbodyConstraints) stream.ReceiveNext();
            int OwnedBy = (int)stream.ReceiveNext();

            if (OwnedBy != OwnerID)
                OwnerID = OwnedBy;
            
        }
    }

    
    // [PunRPC]
    // public void SetObjectsParent(Transform newParentsTransform)
    // {
    //     Debug.Log("network call item parent");
    //
    //     if (newParentsTransform == null)
    //     {
    //         transform.SetParent(originalParent);
    //     }
    //     else
    //     {
    //         transform.SetParent(newParentsTransform);
    //         
    //     }
    //     
    // }
    //
    // public void UpdateObjectsParent(Transform newParentTransform)
    // {
    //     Debug.Log("updateObjects been called, sending network info");
    //     
    //     photonView.RPC("SetObjectsParent", RpcTarget.AllBuffered, newParentTransform);
    // }


    [PunRPC]
    public void SetObjectsRigidBody(bool hasPickedUpItem = false)
    {
        Debug.Log("network call items been picked up, gravity off, constraints off");

        if (hasPickedUpItem)
        {
            // picked up item
            rb.useGravity = false;

            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            // dropped up item
            rb.useGravity = true;

            rb.constraints = RigidbodyConstraints.None;
        }
    }


    public void UpdateObjectsRigidBody(bool hasPickedUpItem = false)
    {
        Debug.Log("sending RPC call to update rigidbody: gravity and constraints");
        
        photonView.RPC("SetObjectsRigidBody", RpcTarget.AllBuffered, hasPickedUpItem);
    }

    public void DisableObject()
    {
        PhotonView Disable = GetComponent<PhotonView>();
        gameObject.SetActive(false);

    }
    public void EnableObject()
    {
        PhotonView Enable = GetComponent<PhotonView>();
        gameObject.SetActive(true);
    }

    // [PunRPC]
    // public void DestroyObject()//PhotonView view)
    // {
    //     // if (view.ViewID == photonView.ViewID)
    //     // {
    //         Debug.Log("destroy object");
    //         
    //         //Destroy(gameObject);
    //     //}
    // }
    //
    // public void RemoveThisObject()
    // {
    //     photonView.RPC("DestroyObject", RpcTarget.Others, 0);//1, _photonView);
    // }

    


    
    
}



