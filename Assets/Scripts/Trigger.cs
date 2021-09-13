
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public MachineConveyor conveyor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ConveyorObject"))
        {
            Destroy(other.gameObject);
            Debug.Log("Destoryed.........");
            conveyor.ResetConveyor();
        }
    }
}
