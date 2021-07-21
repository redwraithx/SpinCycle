using System.Collections;
<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;
using TMPro;
=======
using UnityEngine;
>>>>>>> main


public class WeaponPickUpScript : MonoBehaviour
{
    public GameObject weapon;               
    public GameObject weaponAttach;     
    public float weaponDropForce;      
    //public TMP_Text ammoText;
    
    void Start()
    {
        
        weapon = null;

        
        //ammoText.text = string.Empty;

       
        if (!weaponAttach)
        {
            
            weaponAttach = GameObject.Find("WeaponPlacement");
        }

        if (weaponDropForce <= 0)
        {
            weaponDropForce = 5.0f;

            Debug.Log("WeaponDropForce not set on " + name + ". Defaulting to " + weaponDropForce);
        }
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            
            if (weapon)
            {
                
                weaponAttach.transform.DetachChildren();

                
                StartCoroutine(EnableCollisions(1.0f));

                
                weapon.GetComponent<Rigidbody>().isKinematic = false;

                
                weapon.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * weaponDropForce, ForceMode.Impulse);

               
                //ammoText.text = string.Empty;
            }
        }

        
        if (Input.GetButtonDown("Fire1"))
        {
            
            if (weapon)
            {
               
                //ammoText.text = weapon.Shoot().ToString();
            }
        }
    }

    void OnTriggerEnter(Collider hit)
    {
       
            
        if (!weapon && hit.gameObject.CompareTag("Weapon") || !weapon && hit.gameObject.CompareTag("Item"))
        {

<<<<<<< HEAD
            weapon = hit.gameObject;//hit.gameObject.GetComponent<WeaponScript>();
=======
            weapon = hit.gameObject;
>>>>>>> main



            weapon.GetComponent<Rigidbody>().isKinematic = true;

            
            weapon.transform.position = weaponAttach.transform.position;

            
            weapon.transform.SetParent(weaponAttach.transform);

            
            weapon.transform.localRotation = weaponAttach.transform.localRotation;

            

            Physics.IgnoreCollision(weapon.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());
        }

       
    }

    IEnumerator EnableCollisions(float timeToDisable)
    {
        if (weapon.gameObject.GetComponent<WeaponScript>() != null)

        {

<<<<<<< HEAD
        yield return new WaitForSeconds(timeToDisable);


        Physics.IgnoreCollision(weapon.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);


        weapon = null;
=======
            yield return new WaitForSeconds(timeToDisable);


            Physics.IgnoreCollision(weapon.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);


            weapon = null;
>>>>>>> main
        }
        else
        {
            weapon = null;
            yield return null;
        }
    }
}
