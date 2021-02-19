using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairToolUse : MonoBehaviour, IRepairToolUse
{
    public void UseItem()
    {
        Debug.Log("repair tool goes away");
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
