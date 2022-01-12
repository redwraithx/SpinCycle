using UnityEngine;

public class MachineConveyor : MonoBehaviour
{
    public LaundryBasket basket;
    public MachineScript machine;
    public GameObject conveyorObjectPrefab;
    public Transform spawnPoint;
    public Transform spline;

    public bool isRunning = false;

    public void SpawnObject()
    {
        if (isRunning == true)
        {
            return;
        }
        isRunning = true;
        Debug.Log("Spawing.............");
        GameObject obj = Instantiate(conveyorObjectPrefab, spawnPoint);
        obj.transform.SetParent(spline);

        if (basket)
            obj.GetComponent<FolderBelt>().basket = basket;
        else if (machine)
            obj.GetComponent<FolderBelt>().machine = machine;
    }

    public void ResetConveyor()
    {
        isRunning = false;
    }
}