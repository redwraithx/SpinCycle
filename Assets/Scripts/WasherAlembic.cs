using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WasherAlembic : MonoBehaviour
{
    public MachineScript machineScript;
    public PlayableDirector playableDirector;
    public GameObject laundryAnim;
    bool alembicOn = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetMachine", 3f);

    }

    // Update is called once per frame
    void Update()
    {
       if(machineScript != null)
        {
            if (machineScript.laundryTimer > 0 && alembicOn == false)
            {
                laundryAnim.SetActive(true);
                playableDirector.Play();
                alembicOn = true;
            }

            if (machineScript.laundryTimer <= 0)
            {
                laundryAnim.SetActive(false);
                alembicOn = false;
            }
        }

    }


    void GetMachine()
    {
        GameObject washer = null;
        float distance = Mathf.Infinity;
        foreach(GameObject washMachine in GameObject.FindGameObjectsWithTag("WashingMachine"))
        {
            float washerDistance = Vector3.Distance(transform.position, washMachine.transform.position);
            if(washerDistance < distance)
            {
                distance = washerDistance;
                washer = washMachine;
            }


        }

        if(washer != null)
        {
            transform.parent = washer.transform;
            transform.localPosition = Vector3.zero;
        }


        machineScript = transform.parent.gameObject.GetComponentInChildren<MachineScript>();


    }
}
