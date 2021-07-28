using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DrawProjection : MonoBehaviourPun
{
    public WeaponScript weaponScript;
    public GameObject spawnPoint;
    LineRenderer lineRenderer;

    //number of points on line
    public int numPoints = 50;

    //distance between line points
    public float timeBetweenPoints = 0.3f;

    //the physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (weaponScript == null)
            lineRenderer.enabled = false;
        else
        {
            if(photonView.IsMine)
            {
                lineRenderer.enabled = true;
                Debug.Log("Calculating");
                lineRenderer.positionCount = numPoints;
                List<Vector3> points = new List<Vector3>();
                Vector3 startingPosition = spawnPoint.transform.position;
                Vector3 startingVelocity = spawnPoint.transform.forward * weaponScript.projectileSpeed;
                for (float t = 0; t < numPoints; t += timeBetweenPoints)
                {
                    Vector3 newPoint = startingPosition + t * startingVelocity;
                    newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
                    points.Add(newPoint);

                    /*if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
                    {
                        lineRenderer.positionCount = points.Count;
                        break;
                    }*/
                }

                lineRenderer.SetPositions(points.ToArray());
            }
            
        }
            


       
        

    }
}
