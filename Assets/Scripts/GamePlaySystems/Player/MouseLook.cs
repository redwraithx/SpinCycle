
using UnityEngine;

namespace RedWraith.Player
{

    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;

        public Transform playerBody;

        public float xRotation = 0f;

        public Vector3 bodyRotate;

        private bool isAlive = true;

<<<<<<< HEAD
        // Start is called before the first frame update
=======
>>>>>>> main
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

<<<<<<< HEAD
        // Update is called once per frame
=======
>>>>>>> main
        void Update()
        {
            if (!isAlive)
                return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 70f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            if (mouseX > 0.01f || mouseX < 0.01f)
            {
                bodyRotate = Vector3.up * mouseX;
                Quaternion newQuad = Quaternion.Euler(bodyRotate);

                
                playerBody.Rotate(bodyRotate, Space.World);
            }
<<<<<<< HEAD
        
             

=======
>>>>>>> main

        }

        internal void SetIsAlive(bool value)
        {
            isAlive = value;
        }
    }

}
