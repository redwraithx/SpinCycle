
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;

public class WeaponScript : MonoBehaviour
{
    public ItemType itemType;
    public Rigidbody[] projectiles;
    public Rigidbody projectile;
    //public GameObject projectile;
    public float projectileSpeed;
    public int ammo;                        
    public Transform projectileSpawnPoint;  
    public float projectileForce;
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
        if (Input.GetButtonDown("Fire1")) // Set in Edit | Project Settings | Input Manager
        {
            Debug.Log("Firing");
            fire();
        }

        switch (itemType)
        {
            case ItemType.SabotageWaterGun:
                projectile = projectiles[0];
                break;
            case ItemType.SabotageIceGun:
                projectile = projectiles[1];
                break;
            case ItemType.SabotageSoapGun:
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
