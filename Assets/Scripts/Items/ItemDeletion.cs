using Photon.Pun;
using UnityEngine;

public class ItemDeletion : MonoBehaviour
{
    public float timer;

    private void Start()
    {
        timer += 30;
    }

    private void Update()
    {
        if (timer > 0 && this.transform.parent == null && this.gameObject.GetComponent<Item>().OwnerID != 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && this.transform.parent == null && this.gameObject.GetComponent<Item>().OwnerID != 0)
        {
            //Debug.Log("Bye Bye" + this.gameObject.name);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}