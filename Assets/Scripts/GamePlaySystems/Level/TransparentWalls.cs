using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWalls : MonoBehaviour
{
    public Material[] material;
    private bool transparent = false;
    private MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();


    }

    public void ChangeTransparency(bool transparent)
    {
        if (this.transparent == transparent) return;

        this.transparent = transparent;

        if (transparent)
        {
            rend.material = material[1];
        }
        else
        {
            rend.material = material[0];
        }


    }
}
