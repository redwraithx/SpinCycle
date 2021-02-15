
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
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float moveX = Input.GetAxis("Horizontal") * ((Xspeed * m_moveSpeedMultiplier) * Time.deltaTime);
        float moveZ = Input.GetAxis("Vertical") * ((Zspeed * m_moveSpeedMultiplier) * Time.deltaTime);;

        //Vector3 move = transform.right * moveX + transform.forward * moveZ;

        transform.Rotate(0F, moveX * rotationSpeed, 0f);


        Vector3 move = transform.forward * moveZ;
        
        controller.Move(move);


        // can we jump?
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Jump();
        }

        velocity.y += (gravity * gravityMulitplier) * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }


    private float Jump()
    {
        // v = SQRT(h * -2 * g) or velocity = sqrt(jumpHeight * -2 * gravity)
        return (Mathf.Sqrt((jumpHeight * m_jumpPowerMultiplier) * -2 * (gravity * gravityMulitplier)));
    }
    
}
