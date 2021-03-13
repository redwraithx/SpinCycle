
using System;
using System.IO;
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



public class Item : MonoBehaviourPunCallbacks, IPunObservable, IItem
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAdjustment;

    private PhotonView _photonView = null;
    private PhotonTransformView _photonTransformView = null;
    [SerializeField] private Vector3 correctPosition = Vector3.zero;
    [SerializeField] private Quaternion correctRotation = Quaternion.identity;
    
    
    
    
    private void Awake()
    {
        if(!_photonView)
            _photonView = GetComponent<PhotonView>();
        
        if(_photonTransformView)
            _photonTransformView = GetComponent<PhotonTransformView>();
        
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
    }



    private void Update()
    {
        //UpdatePosition();
        //_photonView.RPC("UpdateObject", RpcTarget.AllBuffered, transform.position, transform.rotation);

    }

    [PunRPC]
    public void UpdateObject(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    private void FixedUpdate()
    {
        if (!_photonView)
            return;

    }


    

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    
    
    
}



