
using System;
using UnityEngine;

using Photon;
using Photon.Realtime;
using Photon.Pun;
using TreeEditor;


public class GetLocationData
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public GetLocationData(int id, Vector3 position, Quaternion rotation)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}



public class Item : MonoBehaviour, IItem
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAdjustment;

    private PhotonView _photonView = null;
    private PhotonTransformView _photonTransformView = null;
    private Vector3 correctPosition = Vector3.zero;
    private Quaternion correctRotation = Quaternion.identity;
    
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _photonTransformView = GetComponent<PhotonTransformView>();
        
        gameObject.tag = "Item";

        gameObject.layer = LayerMask.NameToLayer("Items");
    }

    private void Start()
    {
        
    }


    [PunRPC]
    private void UpdateMyPosition(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctPosition = (Vector3) stream.ReceiveNext();
            correctRotation = (Quaternion) stream.ReceiveNext();
        }
    }


    private void FixedUpdate()
    {
        if (!_photonView)
            return;

        UpdateLocation();
    }

    [PunRPC]
    private void UpdateLocation()
    {
        
        if (_photonView.IsMine) 
        {
            transform.position = Vector3.Lerp(transform.position, correctPosition, Time.fixedDeltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctRotation, Time.fixedDeltaTime * 5);
            
            //_photonView.RPC("OnPhotonSerializeView", RpcTarget.AllBuffered, 5, transform.position, transform.rotation);
            
            _photonView.RPC("UpdateMyPosition", RpcTarget.AllBuffered,  transform.position, transform.rotation );
            
        }

    }

    // [PunRPC]
    // public void OnOwnershipRequest()
    // {
    //     _photonView.TransferOwnership();
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
        private set => _price = value;
    }

    public float TimeAdjustment
    {
        get => _timeAdjustment;
        private set => _timeAdjustment = value;
    }
}



