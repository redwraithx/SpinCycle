
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;

public class WeaponScript : MonoBehaviour
{
    public GameObject gun;
    public ItemType itemType;
    public Rigidbody[] projectiles;
    public Rigidbody projectile;
    //public GameObject projectile;
    public float projectileSpeed;
    public int ammo;                        
    public Transform projectileSpawnPoint;  
    public float projectileForce;

    //rotation values
    public float mouseSensitivity = 100f;
    public float yRotation = 0f;
    public Vector3 gunRotation;
    // Start is called before the first frame update
    void Start()
    {
        if (ammo <= 0)
        {
            ammo = 20;
        }

        if (projectileForce <= 0)
        {
            projectileForce = 3.0f;
        }
        


    }

    private void Update()
    {
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
        
        if (projectileSpawnPoint && projectile)
        {
            // Make bullet
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            //GameObject temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            
            // Shoot bullet
            temp.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
            
        }
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
}
