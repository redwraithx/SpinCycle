
using UnityEngine;

public class CinemachineMouseLook : MonoBehaviour
{

<<<<<<< HEAD
=======

    public LayerMask layerMask;

>>>>>>> main
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    public float xRotation = 0f;

<<<<<<< HEAD
    public Vector3 bodyRotate;

    private bool isAlive = true;

    // Start is called before the first frame update
=======
    public Vector3 bodyRotate = Vector3.zero;

    private bool isAlive = true;

    private TransparentWalls currentTransparentWall;

    

    
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

<<<<<<< HEAD
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //
        // xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation, -90f, 70f);
        //
        // transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //
        // if (mouseX > 0.01f || mouseX < 0.01f)
        // {
        //     bodyRotate = Vector3.up * mouseX;
        //     Quaternion newQuad = Quaternion.Euler(bodyRotate);
        //     
        //     
        //     playerBody.Rotate(bodyRotate, Space.World);
        // }

        
        
=======
        Vector3 direction = playerBody.position - transform.position;
        float length = Vector3.Distance(playerBody.position, transform.position);
        Debug.DrawRay(transform.position, direction.normalized * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, layerMask))
        {
            
            TransparentWalls transparentWall = currentHit.transform.parent.gameObject.GetComponent<TransparentWalls>();
            if (transparentWall)
            {
                Debug.Log("hitting wall");
                if (currentTransparentWall && currentTransparentWall.gameObject != transparentWall.gameObject)
                {
                    currentTransparentWall.ChangeTransparency(false);
                }
                transparentWall.ChangeTransparency(true);
                currentTransparentWall = transparentWall;
            }
        }
        else
        {
            if (currentTransparentWall)
            {
                currentTransparentWall.ChangeTransparency(false);
            }
        }


>>>>>>> main
    }


    internal void SetIsAlive(bool value)
    {
        isAlive = value;
    }
}


