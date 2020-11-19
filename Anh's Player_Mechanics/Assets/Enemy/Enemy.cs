using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]       // Used for trigger based path following
public class Enemy : MonoBehaviour
{
    // Used to keep a reference to Components
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rigidbody;
    private Transform player;
    public float dist;
    public float moveSpeed;
    public float howClose = 5f;
    // Used to keep a reference to the target Agent should move towards
    public GameObject target;

    // Used to tell Script if the path should be automagically made or manually
    // - Set to true to autogen
    // - Set to false to manually create the path
    public bool autoGenPath;

    // Used to tell the agent what tags to use to create different paths for different agents
    public string pathName;

    // Stores all pathNodes for current NavMeshAget to follow in Scene
    public GameObject[] path;

    // Keeps track of what pathNode the NavMesh Agent is moving towards in the Array 
    public int pathIndex;

    enum EnemySate { Chase, Patrol };
    [SerializeField] EnemySate state;

    enum PatrolType { DistanceBased, TriggerBased };
    [SerializeField] PatrolType patrolType;

    // Used to tell the NavMesh Agent when to switch to the next pathNode in the Array
    public float distanceToNextNode;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        animator.applyRootMotion = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody.isKinematic = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Checks if a tag name exists or sets a default one
        if (string.IsNullOrEmpty(pathName))
            pathName = "PathNode";

        // Used to determine how close the agent can get to a target before moving to the next one
        if (distanceToNextNode <= 0)
            distanceToNextNode = 1.0f;

        // Check if Enmey in a Chasing State
        if (state == EnemySate.Chase)
            // Find a target tagged as Player if not set
            target = GameObject.FindWithTag("Player");

        // Check if Enmey is in a Patrol State
        else if (state == EnemySate.Patrol)
        {
            // Finds all PathNodes in the Scene and adds them into an Array
            // - Added in reverse order that they were placed in the Scene
            // - Delete this line and manually add them in the Inspector
            if (autoGenPath)
                path = GameObject.FindGameObjectsWithTag(pathName);

            if (path.Length > 0)
                target = path[pathIndex];
        }

        // If a target exists, tell NavMeshAgent to move towards it
        if (target)
            agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
       

        if (state == EnemySate.Patrol && patrolType == PatrolType.DistanceBased)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));

            Debug.DrawLine(transform.position, target.transform.position, Color.red);

            // Check distance from the Enemy to target Node
            // - If less than distanceToNextNode move to next node
            //if(Vector3.Distance(transform.position, target.transform.position) < distanceToNextNode)
            //if((transform.position - target.transform.position).magnitude < distanceToNextNode)
            if (agent.remainingDistance < distanceToNextNode)
            {
                // Check if there are PathNodes in array
                if(path.Length > 0)
                {
                    // Move to the next node
                    pathIndex++;

                    // Method 1: Reset path back to the beginning and start over
                    pathIndex %= path.Length;

                    // Method 2: Reset path back to the beginning and start over
                    //if (pathIndex >= path.Length)
                    //    pathIndex = 0;

                    // Update target to next pathNode in array
                    target = path[pathIndex];
                }
            }
        }

        // If a target exists, tell NavMeshAgent to move towards it
        if (target)
        {          
            agent.SetDestination(target.transform.position);
        }

        // Tell the animator to play animations
        animator.SetBool("IsGrounded", !agent.isOnOffMeshLink);
        animator.SetFloat("Speed", transform.TransformDirection(agent.velocity).z);
    }


    // Add Collider to Enemy
    // Add Rigidbody to Enemy
    // - Rigidbody should be set to IsKinematic
    void OnTriggerEnter(Collider other)
    {
        // Should only happen if it is a "PathNode" object

    }
}
