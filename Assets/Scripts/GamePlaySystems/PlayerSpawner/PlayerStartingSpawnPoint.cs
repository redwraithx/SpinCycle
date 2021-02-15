using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStartingSpawnPoint : MonoBehaviour
{

    [SerializeField] private Transform spawnPointForPlayer = null;
    [SerializeField] private Transform spawnPointDirectionForPlayer = null;
    [SerializeField] private GameObject playerPrefab = null;

    internal GameObject playerSpawned = null;
    
    [SerializeField] internal bool isPlayerRespawning = false;
  //  [SerializeField] private float respawnTimer = 3f;

    // [SerializeField] private FirstPersonController movementScript = null;
    [SerializeField] private bool hasRigidBodyOnPlayer = false;

    //private GameObject m_RespawnTimerCachedObject;
    //private TextMeshProUGUI m_RespawnUiText;

    private Rigidbody rb = null;
    private CharacterController charController = null;
    
    private void Start()
    {
       // m_RespawnTimerCachedObject = GameObject.FindGameObjectWithTag("RespawnTimer"); //GameManager.Instance.respawnTimer;
        //m_RespawnUiText = GameObject.FindGameObjectWithTag("RespawnTimerText").GetComponent<TextMeshProUGUI>(); //GameManager.Instance.respawnTimerText.GetComponent<TextMeshProUGUI>();
        
        //m_RespawnTimerCachedObject.SetActive(false);
        
        // if(!hasRigidBodyOnPlayer)
        //     //charController = gameObject.GetComponent<CharacterController>();
        //     charController = 
        // else
        //     rb = 
        
        if(!GameObject.FindGameObjectWithTag("Player"))
            RespawnPlayer();

        if (!hasRigidBodyOnPlayer)
        {
            if (charController.enabled == false)
            {
                charController.enabled = true;
            }
        }
    }
    
    
    private void Update()
    {


        if (isPlayerRespawning)
        {
            // respawnTimer -= Time.deltaTime;
            //
            // if (respawnTimer <= 0f)
            // {
            //     Debug.Log("respawn player now!");
            //     isPlayerRespawning = false;
            //             
                 RespawnPlayer();
            //     
            //     
            //     m_RespawnTimerCachedObject.SetActive(false);
            // }
            //
            // if (isPlayerRespawning && respawnTimer > 0f)
            // {
            //     m_RespawnUiText.text = string.Format("Respawning in {0:00}", respawnTimer);
            // }
            
        }
    }
    
    
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        // if (other.gameObject.CompareTag("Water") && !m_RespawnFromWater)
        // {
        //     Debug.Log(other.gameObject.tag + ", respawn from water: " + m_RespawnFromWater);
        //
        //     // if(movementScript)
        //     //     movementScript.enabled = false;
        //     
        //     m_RespawnFromWater = true;
        //
        //     //RespawnPlayer();
        //     isPlayerRespawning = true;
        //
        //     m_RespawnTimerCachedObject.SetActive(true);
        //
        //     respawnTimer = 3f;
        // }
    }

    private void RespawnPlayer()
    {

        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Vector3 lookAtDirection = spawnPointDirectionForPlayer.position - spawnPointForPlayer.position;

            var gameManager = GameManager.Instance;

            if (!gameManager.Player1)
            {
                gameManager.Player1 = Instantiate(playerPrefab, spawnPointForPlayer.position, Quaternion.identity);

                playerSpawned = gameManager.Player1;
            }
            else if (!gameManager.Player2)
            {
                gameManager.Player2 = Instantiate(playerPrefab, spawnPointForPlayer.position, Quaternion.identity);

                playerSpawned = gameManager.Player2;
            }
            else
                return;
            
            if (!playerSpawned)
                return;

            if (playerSpawned.GetComponent<Rigidbody>())
            {
                hasRigidBodyOnPlayer = true;
            }
            else
            {
                hasRigidBodyOnPlayer = false;

                charController = playerSpawned.GetComponent<CharacterController>();
            }


            // move the player to the starting position
            //gameObject.transform.position = playerSpawnPoint.position;

            

            if (!hasRigidBodyOnPlayer)
                if(charController.enabled == true)
                    charController.enabled = false;

            // lookAt direction will face player in the right direction
            Quaternion newRotation = Quaternion.LookRotation(lookAtDirection, playerSpawned.transform.up);

            playerSpawned.transform.rotation = newRotation; //Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);

            if (!hasRigidBodyOnPlayer)
                if(charController.enabled == false)
                    charController.enabled = true;

        }
    }

    
    
}
