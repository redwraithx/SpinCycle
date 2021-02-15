using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private Rigidbody rb = null;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            
        }


        private void Update()
        {

            isGrounded = Physics.CheckSphere(new Vector3(groundDetect.GetSiblingIndex(), groundDetect.position.y, groundDetect.position.z), distanceToGround, groundLayerMask);
            
            
            float _moveX = Input.GetAxis("Horizontal");
            float _moveZ = Input.GetAxis("Vertical");
            
            
            //transform.transform += new Vector3(_moveX, 0f, _moveZ) * (moveSpeed * (moveSpeedMultiplier * Time.deltaTime));
            
            transform.Translate(new Vector3(_moveX, 0f, _moveZ) * (moveSpeed * (moveSpeedMultiplier * Time.deltaTime)));
            
        }
    }

}