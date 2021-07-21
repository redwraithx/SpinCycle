using UnityEngine;



public class PlayerStartingSpawnPoint : MonoBehaviour
{

    [SerializeField] private Transform spawnPointForPlayer = null;
    [SerializeField] private Transform spawnPointDirectionForPlayer = null;
    [SerializeField] private GameObject playerPrefab = null;

    internal GameObject playerSpawned = null;
    
    [SerializeField] internal bool isPlayerRespawning = false;

    [SerializeField] private bool hasRigidBodyOnPlayer = false;


    private Rigidbody rb = null;
    private CharacterController charController = null;
    
    private void Start()
    {

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
            RespawnPlayer();
        }
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


            if (!hasRigidBodyOnPlayer)
                if(charController.enabled == true)
                    charController.enabled = false;

            // lookAt direction will face player in the right direction
            Quaternion newRotation = Quaternion.LookRotation(lookAtDirection, playerSpawned.transform.up);

            playerSpawned.transform.rotation = newRotation; 

            if (!hasRigidBodyOnPlayer)
                if(charController.enabled == false)
                    charController.enabled = true;

        }
    }

    
    
}
