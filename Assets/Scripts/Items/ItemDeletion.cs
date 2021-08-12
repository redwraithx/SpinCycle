using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeletion : MonoBehaviour
{

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer += 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0 && this.transform.parent == null && this.gameObject.GetComponent<Item>().OwnerID != 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0 && this.transform.parent == null && this.gameObject.GetComponent<Item>().OwnerID != 0)
        {
            Debug.Log("Bye Bye" + this.gameObject.name);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
