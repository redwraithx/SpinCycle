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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (machineScript.laundryTimer > 0 && alembicOn == false)
        {
            laundryAnim.SetActive(true);
            playableDirector.Play();
            alembicOn = true;
        }

        if(machineScript.laundryTimer <= 0)
        {
            laundryAnim.SetActive(false);
            alembicOn = false;
        }
    }
}
