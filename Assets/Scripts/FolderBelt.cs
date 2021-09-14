using PathCreation;
using UnityEngine;

public class FolderBelt : MonoBehaviour
{

    public PathCreator pathCreator;
    public float speed;
    float distanceTravelled;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("ConveyorObject"))
        {
            pathCreator = GetComponentInParent<PathCreator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartBelt();

    }
        

    public void StartBelt()
    {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        
    }
}
