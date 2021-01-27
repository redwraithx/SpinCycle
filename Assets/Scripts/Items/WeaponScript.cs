
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Rigidbody projectile;
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
            Debug.Log("Firingisdfjhasdkjahsdkjhaksd");
            fire();
        }
    }
    public void fire()
    {
        if (projectileSpawnPoint && projectile)
        {
            // Make bullet
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

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
}
