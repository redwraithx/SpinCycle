using PathCreation;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public PathCreator pathCreator;
    public float speed = 10;
    float distanceTravelled;

    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
    }
}
