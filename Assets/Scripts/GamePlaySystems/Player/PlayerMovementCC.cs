
using System;
using System.Collections;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviourPun
{
    public int numberOfKeyPressed = 0;
    public Camera cinemachineCamera;
    public CinemachineVirtualCamera shoulderCam;
    public Animator characterAnimator;
    public CharacterController controller;
    public GrabAndHold grabHold;
    public float Xspeed = 12f;
    public float Zspeed = 10f;
    private float m_moveSpeedMultiplier = 1f;
    private float m_jumpPowerMultiplier = 1f;
    Vector3 movementDirection;
    float mouseRotation = 0f;
    bool captureMouseRotation = false;

    //using this bool to transition between move states
    bool shoulderCamActive = true;

    public float slowedXspeed = 4f;
    public float slowedZspeed = 3f;
    public bool isGrabbed = false;
    public Vector3 enemyGrab;


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


        // if (!_photonView.IsMine)
        // {
        //     var cam = gameObject.GetComponentInChildren<Camera>();
        //     cam.gameObject.SetActive(false);
        //
        //     var disableCamera = GetComponentInChildren<CinemachineFreeLook>();
        //     disableCamera.gameObject.SetActive(false);
        // }

        GameManager.Instance.Player1 = this.gameObject;

        if (!rb)
            rb = GetComponent<Rigidbody>();

        if (!characterAnimator)
            characterAnimator = GetComponentInChildren<Animator>();
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


        if (isFrozen == false)
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


            /*Vector3 movement;
            movement = cinemachineCamera.transform.right * Input.GetAxis("Horizontal") * (Xspeed * m_moveSpeedMultiplier) * Time.deltaTime;
            movement += cinemachineCamera.transform.forward * Input.GetAxis("Vertical") * (Zspeed * m_moveSpeedMultiplier) * Time.deltaTime;
            movement.y = 0.0f;*/




            //rotate based on camera
            /*Quaternion lookRotation = cinemachineCamera.transform.rotation;
            lookRotation.x = 0f;
            lookRotation.z = 0f;         
            transform.rotation = lookRotation;*/


            Vector3 move = cinemachineCamera.transform.forward * moveZ;
            move += cinemachineCamera.transform.right * moveX;

            Vector3 targetPosition = controller.transform.position + move;

            movementDirection = targetPosition - controller.transform.position;

            if (!isGrabbed)
            {
                controller.enabled = true;
                controller.Move(move);
            }
            else
            {
                controller.enabled = false;
                transform.position = enemyGrab;
            }

            if (shoulderCam.isActiveAndEnabled == true)
            {
                shoulderCamActive = true;
                if (!captureMouseRotation)
                {
                    mouseRotation = transform.rotation.y;
                    captureMouseRotation = true;
                }
                float mouseY = (Input.GetAxis("Mouse X") * -1) * 300 * Time.deltaTime;
                mouseRotation -= mouseY;
                //Debug.Log(mouseRotation);
                transform.rotation = Quaternion.Euler(0f, mouseRotation, 0f);
            }
            else if (shoulderCam.isActiveAndEnabled == false && shoulderCamActive == true)
            {
                Invoke("RotationTransition", 1.0f);

            }
            else if (move.sqrMagnitude > Mathf.Epsilon)
            {
                captureMouseRotation = false;
                Quaternion syncRotation = Quaternion.identity;
                syncRotation = Quaternion.LookRotation(movementDirection);
                syncRotation.x = 0;
                syncRotation.z = 0;
                transform.rotation = syncRotation;
            }



        }


        if (Input.GetKeyDown("w") || Input.GetKeyDown("s"))

        {
            numberOfKeyPressed += 1;
            characterAnimator.SetBool("Run", true);
        }
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d"))
        {
            numberOfKeyPressed += 1;
            characterAnimator.SetBool("Run", true);
        }

        if (Input.GetKeyUp("w") || Input.GetKeyUp("s"))
        {

            if (numberOfKeyPressed == 1)

                characterAnimator.SetBool("Run", false);

            if (numberOfKeyPressed != 0)
            {
                numberOfKeyPressed -= 1;
            }

        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            if (numberOfKeyPressed == 1)

                characterAnimator.SetBool("Run", false);

            if (numberOfKeyPressed != 0)
            {
                numberOfKeyPressed -= 1;
            }

        }

        if (isFrozen == true)
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
            if (canDive)
                StartCoroutine(DiveCoroutine());

            //Input a way to let go of the player when diving.
            //grabHold.isBeingGrabbed = false;
            //grabHold.isHoldingOtherPlayer = false;

            //GetComponent<GrabAndHold>().BeingReleased();
            SpeedUp();

        }



        // can we jump?
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Jump();
        }


        if (Input.GetKeyDown("space"))
        {
            characterAnimator.SetBool("Jump", true);
        }
        if (Input.GetKeyUp("space"))
        {
            characterAnimator.SetBool("Jump", false);
        }

        velocity.y += (gravity * gravityMulitplier) * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown("p"))
        {
            characterAnimator.SetBool("Attack", true);
        }
        if (Input.GetKeyUp("p"))
        {
            characterAnimator.SetBool("Attack", false);
        }
    }


    private void FixedUpdate()
    {
        if (!_photonView)
            return;

        if (!_photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPosition, Time.fixedDeltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctRotation, Time.fixedDeltaTime * 5);

            _photonView.RPC("SendMessage", RpcTarget.AllBuffered, 5, transform.position, transform.rotation);
        }

    }

    void RotationTransition()
    {
        shoulderCamActive = false;
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
            correctPosition = (Vector3)stream.ReceiveNext();
            correctRotation = (Quaternion)stream.ReceiveNext();
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
        while (Time.time < startTime + _initialDashTime)
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
