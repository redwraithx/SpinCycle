using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class testingScriptDELETEMEAFTERBUILD5 : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(photonView.Owner.IsMasterClient + " for master client owning this MACHINEBOOST");
    }
}
