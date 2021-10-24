using PathCreation;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

public class Follower : MonoBehaviour
{

    public PathCreator pathCreator;
    public float speed = 0f;
    float distanceTravelled = 0f;

    /* private IEnumerator SplineTubs()
     {

         yield return new WaitForSeconds(5f);
         Debug.Log("Playing");
         StartCoroutine(SplineTubs());
     }*/
    private void Update()
    {
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        distanceTravelled += speed * Time.deltaTime;
    }
    private void Start()
    {
        //RandomNumber();
        //StartCoroutine("SplineTubs");

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            RandomNumber();
        }
    }

    public void RandomNumber()
    {
        speed = Random.Range(2f, 6f);
    }
}
