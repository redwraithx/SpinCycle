using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   // Add the error checking classes

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    CharacterController cc;
    public Rigidbody rb;
    public float speed = 5f;
    public float jumpSpeed = 7f;
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    public float gravity;

    Vector3 moveDirection;

    enum ControllerType { SimpleMove, Move };
    [SerializeField] ControllerType type;

    // Handles weapon shooting
    public float projectileSpeed;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

        cc = GetComponent<CharacterController>();

        rb = GetComponent<Rigidbody>();

        if (speed <= 0)
        {
            speed = 6.0f;

        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;

        }

        if (rotationSpeed <= 0)
        {
            rotationSpeed = 10.0f;

        }

        if (gravity <= 0)
        {
            gravity = 9.81f;

        }

        moveDirection = Vector3.zero;

        if (projectileSpeed <= 0)
        {
            projectileSpeed = 6.0f;

        }


        //Manually throw the Exception or the System will throw an Exception
    }        

    // Update is called once per frame
    void Update()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Vector3 direction = new Vector3(verticalInput, 0, horizontalInput);

        //cc.Move(direction * speed * Time.deltaTime);





        // Use if not using MouseLook.CS
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float curSpeed = Input.GetAxis("Vertical") * speed;

        cc.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));




        if (cc.isGrounded)
        {
                    moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

                    // Use if not using MouseLook.CS
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

                
        

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        //}

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
