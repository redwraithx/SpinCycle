using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DynamicCrosshair : MonoBehaviourPun
{
    public Camera cam;
    public Image crosshair;
    public Sprite[] crosshairArray;
    public GameObject player;
    public GameObject selectedObject;

    bool outOfRange = true;
    bool holdingWeapon = false;
    int arrayIterator = 0;

    PlayerSphereCast sphereCast;
    Grab grab;

    float tickWait;
    
    void Start()
    {
        
            player = transform.parent.gameObject;
            sphereCast = player.gameObject.GetComponent<PlayerSphereCast>();
            grab = player.gameObject.GetComponent<Grab>();
        //PlayerSphereCast.ObjectSelected += PlayerSphereCast_ObjectSelected;
        if (photonView.IsMine)
            crosshair.transform.gameObject.SetActive(true);
        else
            crosshair.transform.gameObject.SetActive(false);
        
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

       // crosshair.transform.Rotate(0, 0, 70 * Time.deltaTime);

        if(selectedObject != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(selectedObject.transform.position);
            crosshair.transform.position = Vector3.MoveTowards(crosshair.transform.position, screenPos, 40);
            //crosshair.transform.LookAt(cam.transform);    
        }

        if (!outOfRange && holdingWeapon == false && grab.itemInHand == false)
        {
            if(photonView.IsMine)
                crosshair.transform.gameObject.SetActive(true);
        }

        else if (outOfRange == true || holdingWeapon == true || grab.itemInHand == true)
        {
            //crosshair.transform.position = transform.position;
            crosshair.transform.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(tickWait < 1)
        {
            tickWait++;
            return;
        }
        
        if(crosshair.gameObject.activeInHierarchy == true)
        {
            if (arrayIterator < crosshairArray.Length - 1)
                arrayIterator++;
            else
                arrayIterator = 0;

            crosshair.sprite = crosshairArray[arrayIterator];
        }

        tickWait = 0;
    }

    public void ActivateCrosshair(GameObject obj)
    {

    }

    public void DeactivateCrosshair()
    {

    }
    
}
