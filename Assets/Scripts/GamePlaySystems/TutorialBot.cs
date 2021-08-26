
using UnityEngine;
using UnityEngine.UI;
public class TutorialBot : MonoBehaviour
{

    public Text instructionText;
    Rigidbody rb;
    public GameObject[] movementAreas;
    string[] text = new string[] { "Welcome to Wash Off my robot friend!  Up here in space we do things a little differently, let me show you around the Laundromat.  (click to continue)", "This here chute is where all of the ship's dirty laundry ends up.  In order to score points, you're going to want to take this dirty laundry (right click) ... and clean it!  Simple, right?    (click to continue)", "So the first thing you're going to want to do with dirty laundry... is wash it (left click)!  This here is a washing machine.  A little unorthodox, I know, but like I said, we do things differently up here in space!  Drop your dirty laundry in here, and then wait! (click to continue)", "Once your laundry is all washed up, you're going to have to dry it out.  Nobody likes their pants wet!  Grab your washed laundry, drop it in this machine, and then wait some more!  (click to continue)", "Finally once your laundry is all washed and dried, you're going to want to put it through this here folding machine so it's nice and smooth for our friends upstairs!  (click to continue)", "Once the laundry is all done and ready to go, put it in this here 'Laundry Intake Recepticle' to send it back up to our greatful overlords, and earn points!  Whoever ends the round with the most points wins! (Press P to go back to lobby)" };
    int currentArea = 0;
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
        Debug.Log(text.Length);
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
        
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
