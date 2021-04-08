
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EnumSpace;
using Photon.Pun;
using Photon.Realtime;


[System.Serializable]

public class LaundryPool : MonoBehaviour
{
    public static LaundryPool poolInstance = null;

    [SerializeField]
    public GameObject[] pooledItems;
    private bool notEnoughObjectsInPool = true;

    private List<GameObject>[] pool;


    private void Awake()
    {
        poolInstance = this;
    }
    
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pool = new List<GameObject>[pooledItems.Length];

            for (int i = 0; i < pooledItems.Length; i++)
            {
                pool[i] = new List<GameObject>();
                GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", pooledItems[i].name), transform.position, Quaternion.identity, 0);
                obj.SetActive(false);
                pool[i].Add(obj);
            }
        }

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject.tag;
    }


public GameObject GetItem(LaundryType type)
    {
        int id = (int)type;
        
        if (pool[id].Count > 0)
        {
            for (int i = 0; i < pool[id].Count; i++)
            {
                if (!pool[id][i].activeInHierarchy)
                {
                    return pool[id][i];
                }
            }
        }

        if (notEnoughObjectsInPool)
        {
            GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", pooledItems[id].name), transform.position, Quaternion.identity, 0);
            obj.SetActive(false);
            pool[id].Add(obj);
            return obj;
        }

        return null;
    }
}
