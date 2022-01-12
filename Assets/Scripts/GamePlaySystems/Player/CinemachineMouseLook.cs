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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!isAlive)
            return;

        Vector3 direction = playerBody.position - transform.position;
        float length = Vector3.Distance(playerBody.position, transform.position);
        //Debug.DrawRay(transform.position, direction.normalized * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, layerMask))
        {
            TransparentWalls transparentWall = currentHit.transform.parent.gameObject.GetComponent<TransparentWalls>();
            if (transparentWall)
            {
                //Debug.Log("hitting wall");
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