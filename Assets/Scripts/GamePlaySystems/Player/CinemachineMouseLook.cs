
using UnityEngine;

public class CinemachineMouseLook : MonoBehaviour
{


    public LayerMask layerMask;

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    public float xRotation = 0f;

    public Vector3 bodyRotate = Vector3.zero;

    private bool isAlive = true;

    private TransparentWalls currentTransparentWall;

    

    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if (!isAlive)
            return;

        Vector3 direction = playerBody.position - transform.position;
        float length = Vector3.Distance(playerBody.position, transform.position);
        Debug.DrawRay(transform.position, direction.normalized * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, layerMask))
        {
            TransparentWalls transparentWall = currentHit.transform.GetComponent<TransparentWalls>();
            if (transparentWall)
            {
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


    }


    internal void SetIsAlive(bool value)
    {
        isAlive = value;
    }
}


