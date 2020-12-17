﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public Camera cam;
    public Image crosshair;
    public GameObject player;

    bool outOfRange = true;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSphereCast.ObjectSelected += PlayerSphereCast_ObjectSelected;
    }

    private void PlayerSphereCast_ObjectSelected(GameObject obj)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(obj.transform.position);
        crosshair.transform.position = Vector3.MoveTowards(crosshair.transform.position, screenPos, 10);
    }

    // Update is called once per frame
    void Update()
    {
        outOfRange = player.gameObject.GetComponent<PlayerSphereCast>().outOfRange;

        crosshair.transform.Rotate(0, 0, 50 * Time.deltaTime);


        if (!outOfRange)
        {
            crosshair.gameObject.SetActive(true);
        }

        else
        {
            crosshair.gameObject.SetActive(false);
        }
    }

    public void ActivateCrosshair(GameObject obj)
    {

    }

    public void DeactivateCrosshair()
    {

    }
}
