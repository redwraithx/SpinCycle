using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{

    public MachineScript machineScript;
    public Animator anim;
    public GameObject activatedParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(machineScript.laundryTimer > 0)
        {
            anim.SetBool("isActive", true);
            if (activatedParticles != null) { activatedParticles.SetActive(true); }
        }
        else
        {
            anim.SetBool("isActive", false);
            if (activatedParticles != null) { activatedParticles.SetActive(false); }
        }
    }
}
