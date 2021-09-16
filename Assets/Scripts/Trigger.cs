
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public MachineConveyor conveyor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ConveyorObject"))
        {
            other.GetComponent<FolderBelt>().StopSound();
            Destroy(other.gameObject);

            Debug.Log("Destoryed.........");
            conveyor.ResetConveyor();
        }
    }
}
