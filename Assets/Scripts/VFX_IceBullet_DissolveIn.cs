using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_IceBullet_DissolveIn : MonoBehaviour
{
    #region Variables
    // Game Objects
    [SerializeField] GameObject snowball;
    [SerializeField] GameObject snowballOuterVFX;
    [SerializeField] GameObject iceTrailVFX;
    [SerializeField] GameObject chargeVFX;
    [SerializeField] GameObject collideVFX;

    //Materials
    private Material snowballMaterial;
    private Material snowballOuterMaterial;
    
    //VFX Handlers
    [SerializeField] private float speed = 1;
    [SerializeField] private float spawnSpeedDivider = 1;
    [SerializeField] private float spinSpeed = 1;
    private bool isDissolvingIn = false;
    private float fade = 0;

    #endregion
    void Awake() 
    {
        if(snowball != null)
        {
            snowballMaterial = snowball.GetComponent<Renderer>().material;
        }

        if(snowballOuterVFX != null)
        {
            snowballOuterMaterial = snowballOuterVFX.GetComponent<Renderer>().material;
        }

        //Enable the charge VFX
        chargeVFX.SetActive(true);
    }

    void Update()
    {
        // Runs until fade is completed
        if(fade < 1)
        {
            //Dissolves the ice chunk in
            DissolveIn();
        }

        //Turns on effect trail for snowball when the dissolve is completed
        if(fade >= 1)
        {
            iceTrailVFX.SetActive(true);
        }

        snowball.transform.Rotate(360 * GetTime() * spinSpeed, 0, 0);
    }

    public void DissolveIn()
    {
        float dissolveInSpeed = speed / spawnSpeedDivider;

        isDissolvingIn = true;

        if (isDissolvingIn)
        {
            fade += GetTime() * dissolveInSpeed;
            if (fade >= 1)
            {
                fade = 1;
                isDissolvingIn = false;
            }

            //Gets the Snowball Material and fades it in over time. 
            snowballMaterial.SetFloat("dissolveIn", fade);
            snowballOuterMaterial.SetFloat("dissolveIn", fade);
        }
    }

    float GetTime()
    {
        return Time.deltaTime;
    }

    public void InsantiateCollideVFX(Collision collision)
    {
        Debug.Log("SNOWBALL HIT");
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        Instantiate(collideVFX, position, rotation);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
