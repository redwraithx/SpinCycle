
using System.IO;
using UnityEngine;
using EnumSpace;
using GamePlaySystems.Utilities;
using Photon;
using Photon.Pun;

public class WeaponScript : MonoBehaviourPun
{
    public GameObject gun;
    public WeaponDestroyScript destroyGun = null;
    public ItemType itemType;
    public Rigidbody[] projectiles;
    public Rigidbody projectile;
    //public GameObject projectile;
    public float projectileSpeed = 50f;
    public int ammo;                        
    public Transform projectileSpawnPoint;  
    public float projectileForce;
    private bool hasFired = false; 

    //rotation values
    public float mouseSensitivity = 100f;
    public float yRotation = 0f;
    public Vector3 gunRotation;

    //bool for gun fx
    public bool isFiring = false;
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
        if(gun.GetComponent<ItemTypeForItem>().itemType == ItemType.SabotageSoapGun)
        {
          Debug.Log("Weapon Script For Soap Bomb");
            return;
        }


        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -45f, 45f);
        gun.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        projectileSpawnPoint = gun.transform.GetChild(0).transform;

        //if(gun && !destroyGun)
        //{
        //    destroyGun = gun.GetComponent<WeaponDestroyScript>();
        //}
        //else
        //{
        //    destroyGun = null;
        //}

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
            if(photonView.IsMine)
            {
                Debug.Log("Firing");
                fire();
            }

        }
                             
    }
    public void fire()
    {


        if (!destroyGun || destroyGun.hasFired)
            return;

        if (projectileSpawnPoint && projectile)
        {
            destroyGun.hasFired = true;
            Quaternion rotation = Quaternion.LookRotation(projectileSpawnPoint.transform.forward, Vector3.up);
            photonView.RPC("RPCShoot", RpcTarget.MasterClient, projectileSpawnPoint.transform.position, rotation);
            //PhotonNetwork.Destroy(gun);

        }





    }

    [PunRPC]
    public void RPCShoot(Vector3 spawnPoint, Quaternion spawnRotation)
    {
        isFiring = true;

        GameObject iceCube = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", "IceCube"), spawnPoint, spawnRotation);
        iceCube.GetComponent<Rigidbody>().AddForce(iceCube.transform.forward * projectileSpeed, ForceMode.Impulse);
        AudioClip freezeGunSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Sabotages/FreezeGun/freeze");
        //iceCube.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
        GameManager.audioManager.PlaySfx(freezeGunSound);
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
