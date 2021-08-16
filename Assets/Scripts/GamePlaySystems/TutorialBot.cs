
using UnityEngine;
using UnityEngine.UI;
public class TutorialBot : MonoBehaviour
{

    public Text instructionText;
    Rigidbody rb;
    public GameObject[] movementAreas;
    public string[] text;
    int currentArea;
    public float speed;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (speed == 0)
            speed = 4;
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");

            return;
        }
        
        // remove this once the level has been fixed
        if (string.IsNullOrEmpty(text[currentArea]))
            return;
        
        instructionText.text = text[currentArea];

        Vector3 playerLookAt = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerLookAt);

        float step = speed * Time.deltaTime;
        rb.MovePosition(Vector3.MoveTowards(transform.position, movementAreas[currentArea].transform.position, step));
        float distance = Vector3.Distance(transform.position, movementAreas[currentArea].transform.position);
        if (distance < 2)
        {
            if(Input.GetMouseButton(0))
            {
                currentArea++;
            }
        }
    }
}
