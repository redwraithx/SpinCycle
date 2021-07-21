
using UnityEngine;
<<<<<<< HEAD

public class WeaponScript : MonoBehaviour
{
    public Rigidbody projectile;
=======
using EnumSpace;
using GamePlaySystems.Utilities;

public class WeaponScript : MonoBehaviour
{
    public GameObject gun;
    public ItemType itemType;
    public Rigidbody[] projectiles;
    public Rigidbody projectile;
    //public GameObject projectile;
>>>>>>> main
    public float projectileSpeed;
    public int ammo;                        
    public Transform projectileSpawnPoint;  
    public float projectileForce;
<<<<<<< HEAD
=======

    //rotation values
    public float mouseSensitivity = 100f;
    public float yRotation = 0f;
    public Vector3 gunRotation;
>>>>>>> main
    // Start is called before the first frame update
    void Start()
    {
        if (ammo <= 0)
        {
<<<<<<< HEAD
            
=======
>>>>>>> main
            ammo = 20;
        }

        if (projectileForce <= 0)
        {
<<<<<<< HEAD
           
            projectileForce = 3.0f;
        }
=======
            projectileForce = 3.0f;
        }
        
>>>>>>> main


    }

    private void Update()
    {
<<<<<<< HEAD
        if (Input.GetButtonDown("Fire1")) // Set in Edit | Project Settings | Input Manager
        {
            Debug.Log("Firingisdfjhasdkjahsdkjhaksd");
            fire();
        }
    }
    public void fire()
    {
=======
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -45f, 45f);
        gun.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        

        if (Input.GetButtonDown("Fire1")) // Set in Edit | Project Settings | Input Manager
        {
            Debug.Log("Firing");
            fire();
        }

        switch (itemType)
        {
            case ItemType.SabotageWaterGun:
                projectileSpeed = 50f;
                projectile = projectiles[0];
                break;
            case ItemType.SabotageIceGun:
                projectileSpeed = 30;
                projectile = projectiles[1];
                break;
            case ItemType.SabotageSoapGun:
                projectileSpeed = 40;
                projectile = projectiles[2];
                break;
        }
    }
    public void fire()
    {
        
>>>>>>> main
        if (projectileSpawnPoint && projectile)
        {
            // Make bullet
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

<<<<<<< HEAD
            // Shoot bullet
            temp.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
    }

    public int Shoot()
    {
        
        if (projectile && ammo > 0)
        {
            
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;

            
            temp.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
            
            ammo--;
        }
        
        else
        {
            
            Debug.Log("Reload");
        }

        return ammo;
    }
=======
            //GameObject temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            
            // Shoot bullet
            temp.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
            DropWeapon();
            
        }
    }

    public void DropWeapon()
    {
        GetComponentInParent<Grab>().CheckForMouseUp();
        Invoke("DestroyWeapon", 1f);
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    //public int Shoot()
    //{
        
    //    if (projectile && ammo > 0)
    //    {
            
    //        GameObject temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            
    //        temp.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce, ForceMode.Impulse);

    //        Destroy(temp.gameObject, 2.0f);
            
    //        ammo--;
    //    }
        
    //    else
    //    {
            
    //        Debug.Log("Auto Reload if we need this?");
    //    }

    //    return ammo;
    //}
>>>>>>> main
}
