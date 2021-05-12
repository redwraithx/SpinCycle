
using System.Collections.Generic;
<<<<<<< HEAD
using UnityEngine;
using EnumSpace;
=======
using System.IO;
using UnityEngine;
using EnumSpace;
using Photon.Pun;
using Photon.Realtime;

>>>>>>> main

[System.Serializable]

public class LaundryPool : MonoBehaviour
{
<<<<<<< HEAD
    public static LaundryPool poolInstance;
=======
    public static LaundryPool poolInstance = null;
>>>>>>> main

    [SerializeField]
    public GameObject[] pooledItems;
    private bool notEnoughObjectsInPool = true;

    private List<GameObject>[] pool;


<<<<<<< HEAD


=======
>>>>>>> main
    private void Awake()
    {
        poolInstance = this;
    }
<<<<<<< HEAD
    // Start is called before the first frame update
    void Start()
    {
        pool = new List<GameObject>[pooledItems.Length];

        for (int i = 0; i < pooledItems.Length; i++)
        {
            pool[i] = new List<GameObject>();
            GameObject obj = Instantiate(pooledItems[i]);
            obj.SetActive(false);
            pool[i].Add(obj);
        }
    }

    // Update is called once per frame
    public GameObject GetItem(LaundryType type)
=======
    
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
>>>>>>> main
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
<<<<<<< HEAD
            GameObject obj = Instantiate(pooledItems[id]);
=======
            GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", pooledItems[id].name), transform.position, Quaternion.identity, 0);
>>>>>>> main
            obj.SetActive(false);
            pool[id].Add(obj);
            return obj;
        }

        return null;
    }
}
