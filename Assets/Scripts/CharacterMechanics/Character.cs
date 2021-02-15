using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    private CharacterController cc = null;
    public Rigidbody rb;
    public float speed = 5f;
    public float jumpSpeed = 7f;
    public float rotationSpeed = 0f; 
    public float gravity = 0f;

    private Vector3 moveDirection = Vector3.zero;

    enum ControllerType
    {
        SimpleMove, 
        Move
    };
    
    
    [SerializeField] ControllerType type;

    // Handles weapon shooting
    public float projectileSpeed;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;


    void Start()
    {

        cc = GetComponent<CharacterController>();

        rb = GetComponent<Rigidbody>();

        if (speed <= 0)
            speed = 6.0f;

        if (jumpSpeed <= 0)
            jumpSpeed = 8.0f;

        if (rotationSpeed <= 0)
            rotationSpeed = 10.0f;

        if (gravity <= 0)
            gravity = 9.81f;

        
        moveDirection = Vector3.zero;

        if (projectileSpeed <= 0)
        {
            projectileSpeed = 6.0f;

        }


    }        

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float curSpeed = Input.GetAxis("Vertical") * speed;

        cc.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));


        if (cc.isGrounded)
        {
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;
                        
        }

        if (Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        cc.Move(moveDirection * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.LeftControl))
            if (Input.GetButtonDown("Fire1")) // Set in Edit | Project Settings | Input Manager
            {
                fire();
            }

    }

    public void fire()
    {
        if (projectileSpawnPoint && projectilePrefab)
        {
            // Make bullet
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Shoot bullet
            temp.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
    }
}
