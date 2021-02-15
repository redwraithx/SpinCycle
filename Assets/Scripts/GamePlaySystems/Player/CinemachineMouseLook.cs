
using UnityEngine;

public class CinemachineMouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    public float xRotation = 0f;

    public Vector3 bodyRotate = Vector3.zero;

    private bool isAlive = true;

    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if (!isAlive)
            return;
        
    }


    internal void SetIsAlive(bool value)
    {
        isAlive = value;
    }
}


