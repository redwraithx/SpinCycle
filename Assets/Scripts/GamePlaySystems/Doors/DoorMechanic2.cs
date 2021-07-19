using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanic2 : MonoBehaviour
{
    public Animator m_Animator;
    public List<GameObject> whatWeHave = new List<GameObject>();
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // m_Animator.SetTrigger("Open");
            m_Animator.SetBool("isOpen", true);
        }
        whatWeHave.Add(other.gameObject);
        //prepare the check array 
        // search all objects to see if we have all 3 parts
        for (int i = 0; i < whatWeHave.Count; i++)
        {
            if (whatWeHave[i].gameObject.tag == "Player")
                count += 1;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        count -= 1;
        if (other.gameObject.tag == "Player" && count == 0)
        {
            //  m_Animator.SetTrigger("Close");
            m_Animator.SetBool("isOpen", false);
        }
        whatWeHave.Remove(other.gameObject);
    }
}
