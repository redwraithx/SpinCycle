using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;

[System.Serializable]

public class LaundryPool : MonoBehaviour
{
    public static LaundryPool poolInstance;

    [SerializeField]
    public GameObject[] pooledItems;
    private bool notEnoughObjectsInPool = true;

    private List<GameObject>[] pool;




    private void Awake()
    {
        poolInstance = this;
    }
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
            GameObject obj = Instantiate(pooledItems[id]);
            obj.SetActive(false);
            pool[id].Add(obj);
            return obj;
        }

        return null;
    }
}
