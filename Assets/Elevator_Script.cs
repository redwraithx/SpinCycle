using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Script : MonoBehaviour
{
    public GameObject Elevator1;

    public float delayElevatorTimer1;

    public float elevatorSpeed1;

    public bool isElevator1GoingUp = false;
    public bool isElevator2GoingUp = false;
    public bool isDelayTimerRunning = false;

    public Vector3 topPos1;
    public Vector3 botPos1;

    // Start is called before the first frame update
    void Start()
    {
        botPos1 = Elevator1.transform.position;
        topPos1 = Elevator1.transform.position;
        topPos1.y += 9.4f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isElevator1GoingUp == true && delayElevatorTimer1 <= 0)
        {
            Elevator1.transform.Translate(new Vector3(0, 0, 1) * elevatorSpeed1);
        }
        else if (isElevator1GoingUp == false && delayElevatorTimer1 <= 0)
        {
            Elevator1.transform.Translate(new Vector3(0, 0, -1) * elevatorSpeed1);
        }

        if (Mathf.Abs(Vector3.Distance(topPos1, Elevator1.transform.position)) <= 0.1f)
        {
            isElevator1GoingUp = false;

            if(isDelayTimerRunning == false)
            {
                delayElevatorTimer1 += 2f;

                isDelayTimerRunning = true;
            }
        }

        if (Mathf.Abs(Vector3.Distance(botPos1, Elevator1.transform.position)) <= 0.1f)
        {
            isElevator1GoingUp = true;

            if (isDelayTimerRunning == false)
            {
                delayElevatorTimer1 += 2f;

                isDelayTimerRunning = true;
            }
        }

        if (delayElevatorTimer1 > 0)
        {          
            delayElevatorTimer1 -= Time.deltaTime;
        }
        else if(delayElevatorTimer1 < 0)
        {
            isDelayTimerRunning = false;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject);

            //other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.transform.SetParent(this.transform);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            other.gameObject.transform.parent = null;
        }
    }
}
