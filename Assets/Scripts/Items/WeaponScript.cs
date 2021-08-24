
using System.IO;
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;
using Photon.Pun;
using Photon.Realtime;

public class WeaponScript : MonoBehaviourPun
{
    public GameObject gun;
    public WeaponDestroyScript destroyGun = null;
    public ItemType itemType;
    public Rigidbody[] projectiles;
    public Rigidbody projectile;
    //public GameObject projectile;
    public float projectileSpeed;
    public int ammo;                        
    public Transform projectileSpawnPoint;  
    public float projectileForce;
    private bool hasFired = false; 

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

        if (Input.GetButtonDown("Fire1")) // Set in Edit | Project Settings | Input Manager
        {
            Debug.Log("Firing");
                fire();
        }
                             
    }
    public void fire()
    {


        if (!destroyGun || destroyGun.hasFired)
            return;

        if (projectileSpawnPoint && projectile)
        {
            destroyGun.hasFired = true;

            // Make bullet
            GameObject bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "Snowball"), projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Shoot bullet
            //bullet.GetComponent<IceCube>().Shoot(projectileSpawnPoint.forward, projectileSpeed);

        }



    }

}
