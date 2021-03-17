﻿
using System;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;


using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    public CharacterController controller;
    public float Xspeed = 12f;
    public float Zspeed = 10f;
    private float m_moveSpeedMultiplier = 1f;
    private float m_jumpPowerMultiplier = 1f;
    
    public float gravity = -9.8f;
    public float gravityMulitplier = 2f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    [SerializeField] private float rotationSpeed = 0f;

    private Vector3 velocity;
    public bool isGrounded;

    public bool isFrozen;
    public float frozenTimer = 10;
    
    // networking
    internal PhotonView _photonView = null;
    private Vector3 correctPosition = Vector3.zero;
    private Quaternion correctRotation = Quaternion.identity;

    public float MoveSpeed
    {
        get => m_moveSpeedMultiplier;
        private set => m_moveSpeedMultiplier = value;
    }

    public float JumpPower
    {
        get => m_jumpPowerMultiplier;
        set => m_jumpPowerMultiplier = value;
    }

    public void UpdateMoveSpeedMultiplier()
    {
        m_moveSpeedMultiplier = 2f;
    }

    public void UpdateJumpPowerMultiplier()
    {
        m_jumpPowerMultiplier = 3.5f;
    }


    private void Awake()
    {
        if (!_photonView)
            _photonView = GetComponent<PhotonView>();
        
        
        if (!_photonView.IsMine)
        {
            var cam = gameObject.GetComponentInChildren<Camera>();
            cam.gameObject.SetActive(false);

            var disableCamera = GetComponentInChildren<CinemachineFreeLook>();
            disableCamera.gameObject.SetActive(false);
        }

        GameManager.Instance.Player1 = this.gameObject;
    }


    // Start is called before the first frame update
    void Start()
    {
       
        
        
        if (!_photonView.IsMine)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        if(isFrozen == false)
        {
            float moveX = Input.GetAxis("Horizontal") * ((Xspeed * m_moveSpeedMultiplier) * Time.deltaTime);
            float moveZ = Input.GetAxis("Vertical") * ((Zspeed * m_moveSpeedMultiplier) * Time.deltaTime);

            transform.Rotate(0F, moveX * rotationSpeed, 0f);


            Vector3 move = transform.forward * moveZ;

            controller.Move(move);
        }

        if(isFrozen == true)
        {
            frozenTimer -= Time.deltaTime;
            if (frozenTimer <= 0)
            {
                isFrozen = false;
                frozenTimer = 10;
            }

        }


        //Vector3 move = transform.right * moveX + transform.forward * moveZ;

        


        // can we jump?
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Jump();
        }

        velocity.y += (gravity * gravityMulitplier) * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        
    }


    private void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPosition, Time.fixedDeltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctRotation, Time.fixedDeltaTime * 5);
            
            _photonView.RPC("SendMessage", RpcTarget.All, 5, transform.position, transform.rotation);
        }
        
        
        
        
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    


    private float Jump()
    {
        // v = SQRT(h * -2 * g) or velocity = sqrt(jumpHeight * -2 * gravity)
        return (Mathf.Sqrt((jumpHeight * m_jumpPowerMultiplier) * -2 * (gravity * gravityMulitplier)));
    }
    
}
