using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dissapear", 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Dissapear()
    {
        Destroy(gameObject);
    }

}

