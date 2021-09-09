using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WasherAlembic : MonoBehaviour
{
    public MachineScript machineScript;
    public PlayableDirector playableDirector;
    bool alembicOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (machineScript.laundryTimer > 0 && alembicOn == false)
        {
            playableDirector.Play();
            alembicOn = true;
        }

        if(machineScript.laundryTimer <= 0)
        {
            alembicOn = false;
        }
    }
}
