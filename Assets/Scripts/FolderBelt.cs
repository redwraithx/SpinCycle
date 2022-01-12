using PathCreation;
using UnityEngine;

public class FolderBelt : MonoBehaviour
{
    public LaundryBasket basket;
    public MachineScript machine;
    public PathCreator pathCreator;
    public float speed;
    private float distanceTravelled;

    private void Start()
    {
        if (gameObject.CompareTag("ConveyorObject"))
        {
            pathCreator = GetComponentInParent<PathCreator>();
        }
    }

    private void Update()
    {
        StartBelt();
    }

    public void StartBelt()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
    }

    public void StopSound()
    {
        if (basket)
            basket.GetComponent<LaundryBasket>().StopSound();
        else if (machine)
            machine.GetComponent<MachineScript>().StopSound();
    }
}