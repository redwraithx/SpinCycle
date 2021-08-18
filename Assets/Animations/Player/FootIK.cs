
using UnityEngine;

public class FootIK : MonoBehaviour
{
    Animator anim;
    public LayerMask layerMask;

    [Range(0, 1f)]
    public float DistanceToGround;

    [SerializeField] private Transform localForward = null;
    private Vector3 localForwardAtFloor;
    

    private void Start()
    {
        anim = GetComponent<Animator>();

        localForwardAtFloor = new Vector3(localForward.position.x, localForward.position.y - 1.5f, localForward.position.z);
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (anim)
        {
            //localForwardAtFloor = localForward.position;
            
            
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            
            
            // get position for local forward at ground level
            RaycastHit localHit;
            Debug.DrawLine(localForward.position, localForwardAtFloor, Color.magenta);
            if(Physics.Raycast(localForward.position, Vector3.down, out localHit, 2.5f, layerMask))
            {
                if (localHit.collider != null)
                {
                    localForwardAtFloor = localHit.point; //localHit.point;
                    Debug.Log("point hit: " + localHit.collider.gameObject.name);
                }
            }

            var direction = localForwardAtFloor - transform.position;
            

            //Left Foot
            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.CompareTag("Ground"))
                {

                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(direction, hit.normal));

                }
            }

            //Right Foot
            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.CompareTag("Ground"))
                {

                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(direction, hit.normal));


                }

            }
        }
    }
}

