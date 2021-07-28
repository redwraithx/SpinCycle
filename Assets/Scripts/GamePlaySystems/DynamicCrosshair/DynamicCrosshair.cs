using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DynamicCrosshair : MonoBehaviourPun
{
    public Camera cam;
    public Image crosshair;
    public GameObject player;
    public GameObject selectedObject;

    bool outOfRange = true;
    bool holdingWeapon = false;

    PlayerSphereCast sphereCast;
    Grab grab;
    
    void Start()
    {
        
            player = transform.parent.gameObject;
            sphereCast = player.gameObject.GetComponent<PlayerSphereCast>();
            grab = player.gameObject.GetComponent<Grab>();
        //PlayerSphereCast.ObjectSelected += PlayerSphereCast_ObjectSelected;
        if (photonView.IsMine)
            crosshair.gameObject.SetActive(true);
        else
            crosshair.gameObject.SetActive(false);
        
    }

    private void PlayerSphereCast_ObjectSelected(GameObject obj)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(obj.transform.position);
        crosshair.transform.position = Vector3.MoveTowards(crosshair.transform.position, screenPos, 40);
    }


    void Update()
    {
        selectedObject = sphereCast.currentHitObject;
        outOfRange = sphereCast.outOfRange;
        holdingWeapon = grab.weapon.enabled;

        crosshair.transform.Rotate(0, 0, 70 * Time.deltaTime);

        if(selectedObject != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(selectedObject.transform.position);
            crosshair.transform.position = Vector3.MoveTowards(crosshair.transform.position, screenPos, 40);
        }

        if (!outOfRange && holdingWeapon == false)
        {
            if(photonView.IsMine)
                crosshair.gameObject.SetActive(true);
        }

        else if (outOfRange == true || holdingWeapon == true)
        {
            //crosshair.transform.position = transform.position;
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
