using UnityEngine;

public class FanScript : MonoBehaviour
{
    public MachineScript machineScript;
    public Animator anim;
    public GameObject activatedParticles;

    private void Update()
    {
        if (machineScript.laundryTimer > 0)
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