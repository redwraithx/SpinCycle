using UnityEngine;

public class MachineConveyor : MonoBehaviour
{
    public GameObject conveyorObjectPrefab;
    public Transform spawnPoint;
    public Transform spline;

    public bool isRunning = false;


    public void SpawnObject()
    {
        if(isRunning == true)
        {
            return;
        }
        isRunning = true;
        Debug.Log("Spawing.............");
        GameObject obj = Instantiate(conveyorObjectPrefab, spawnPoint);

        obj.transform.SetParent(spline);

       
    }

    public void ResetConveyor()
    {
        isRunning = false;
    }

}
