
using System;
using System.Collections;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviourPun
{
    public CharacterController controller;
    public GrabAndHold grabHold;
    public float Xspeed = 12f;
    public float Zspeed = 10f;
    private float m_moveSpeedMultiplier = 1f;
    private float m_jumpPowerMultiplier = 1f;

    public float slowedXspeed = 4f;
    public float slowedZspeed = 3f;
    public bool isGrabbed = false;


    public float gravity = -9.8f;
    public float gravityMulitplier = 2f;
    public float jumpHeight = 3f;


    public float diveSpeed = 100f;
    public float diveMultiplier = 1f;
    public float _dashTime = 0f;
    public float _initialDashTime = 2f;
    public Vector3 originalVel = Vector3.zero;
    public Rigidbody rb = null;
    public Transform playerModelTransform = null;
    public int playerDiveIndex = 0;


    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    [SerializeField] private float rotationSpeed = 0f;

    private Vector3 velocity;
    public bool isGrounded;

    private bool canDive = true;
    public float diveReuseDelayTime = 1f;

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

        if (!rb)
            rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"player dive inde: {playerDiveIndex}");
        //GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex] = false;
        canDive = true;

        if (diveReuseDelayTime < 1f)
            diveReuseDelayTime = 1f;
        
        if (!_photonView.IsMine)
            this.enabled = false;

        if (!grabHold)
            grabHold = GetComponent<GrabAndHold>();
            
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
            float moveX;
            float moveZ;

            if (!isGrabbed)
            {
                moveX = Input.GetAxis("Horizontal") * ((Xspeed * m_moveSpeedMultiplier) * Time.deltaTime);
                moveZ = Input.GetAxis("Vertical") * ((Zspeed * m_moveSpeedMultiplier) * Time.deltaTime);
            }
            else
            {
                moveX = Input.GetAxis("Horizontal") * ((slowedXspeed * m_moveSpeedMultiplier) * Time.deltaTime);
                moveZ = Input.GetAxis("Vertical") * ((slowedZspeed * m_moveSpeedMultiplier) * Time.deltaTime);
            }
                



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

        Vector3 newVec = Vector3.zero;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("diving maybe");
            
            
            //Diving(true);

            //if(!GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex])
            if(canDive)
                StartCoroutine(DiveCoroutine());

            //Input a way to let go of the player when diving.
            grabHold.isBeingGrabbed = false;
            grabHold.isHoldingOtherPlayer = false;

            GetComponent<GrabAndHold>().BeingReleased();
            SpeedUp();

        }



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
        if (!_photonView)
            return;

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


    private void Diving(bool canDive)
    {
        if (!canDive || !isGrounded)
            return;
        
        //Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
        
        //rb.AddForce(transform.forward * (diveMultiplier * diveSpeed), ForceMode.Force);

        //controller.Move(localForward * (diveSpeed * diveMultiplier * Time.deltaTime));

        controller.enabled = false;
        Debug.Log("controller off");
        
        //rb.AddForce(playerModelTransform.up * (diveSpeed * diveMultiplier), ForceMode.Force);
        transform.Translate(transform.forward * (diveSpeed * diveMultiplier), Space.World);
        
        controller.enabled = true;
        Debug.Log("controller on");

        

    }
    

    private IEnumerator DiveCoroutine()
    {
        Debug.Log("diving coroutine");
        
        //GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex] = true;
        canDive = false;
        
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + _initialDashTime)
        {
            transform.Translate(transform.forward * (diveSpeed * Time.deltaTime), Space.World);
            // or controller.Move(...), dunno about that script
            yield return null; // this will make Unity stop here and continue next frame
        }


        //yield return new WaitForSeconds(GameManager.networkLevelManager.initialDiveReuseDelay);
        yield return new WaitForSeconds(diveReuseDelayTime);

        //GameManager.networkLevelManager.isPlayersDiveDelayEnabled[playerDiveIndex] = false;
        canDive = true;

        


    }

    private float Jump()
    {
        // v = SQRT(h * -2 * g) or velocity = sqrt(jumpHeight * -2 * gravity)
        return (Mathf.Sqrt((jumpHeight * m_jumpPowerMultiplier) * -2 * (gravity * gravityMulitplier)));
    }
    
    public void SlowDown()
    {
        isGrabbed = true;
    }

    public void SpeedUp()
    {
        isGrabbed = false;


    }

}
