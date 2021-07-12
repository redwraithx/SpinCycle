using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofWorld : MonoBehaviour
{
    public float reloacateFallenObjectHeight = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
            return;
        
        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, reloacateFallenObjectHeight, other.gameObject.transform.position.z);

        Debug.Log(other.gameObject.name);
    }
}
