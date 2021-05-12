using System;
<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> main
using UnityEngine;


namespace RedWraith.Player
{

    public class PlayerMovementRigidbody : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float moveSpeedMultiplier = 1f;
        [SerializeField] private float distanceToGround = 0.2f;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Transform groundDetect = null;
        [SerializeField] private bool isGrounded = false;
<<<<<<< HEAD
=======

>>>>>>> main
        
        private Rigidbody rb = null;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            
<<<<<<< HEAD
            
=======
>>>>>>> main
        }


        private void Update()
        {
<<<<<<< HEAD

            isGrounded = Physics.CheckSphere(new Vector3(groundDetect.GetSiblingIndex(), groundDetect.position.y, groundDetect.position.z), distanceToGround, groundLayerMask);
            
            
=======
            isGrounded = Physics.CheckSphere(new Vector3(groundDetect.GetSiblingIndex(), groundDetect.position.y, groundDetect.position.z), distanceToGround, groundLayerMask);
            
>>>>>>> main
            float _moveX = Input.GetAxis("Horizontal");
            float _moveZ = Input.GetAxis("Vertical");
            
            
<<<<<<< HEAD
            //transform.transform += new Vector3(_moveX, 0f, _moveZ) * (moveSpeed * (moveSpeedMultiplier * Time.deltaTime));
            
            transform.Translate(new Vector3(_moveX, 0f, _moveZ) * (moveSpeed * (moveSpeedMultiplier * Time.deltaTime)));
            
        }
=======
            transform.Translate(new Vector3(_moveX, 0f, _moveZ) * (moveSpeed * (moveSpeedMultiplier * Time.deltaTime)));
            
        }
        
>>>>>>> main
    }

}