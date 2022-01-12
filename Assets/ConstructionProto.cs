using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstructionProto : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject junkYard;
    public int currentBlock;
    public int currentArray;
    public GameObject[] currentConstructs;
    public GameObject[] boxes;
    public GameObject[] box1Fits;
    public GameObject[] box2Fits;
    public GameObject[] box3Fits;
    public GameObject[] box4Fits;
    public bool constructionMode;
    // Start is called before the first frame update
    void Start()
    {
        constructionMode = true;
        GameObject[,] listsOfConstructs = new GameObject[3, 2];
        currentArray = 1;
        currentBlock = 0;
        currentConstructs = new GameObject[boxes.Length];
        boxes.CopyTo(currentConstructs, 0);
        currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (constructionMode == true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentConstructs[currentBlock].transform.position = junkYard.transform.position;
                if (currentBlock < currentConstructs.Length - 1)
                {
                    currentBlock += 1;
                    currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
                }
                else if (currentBlock >= currentConstructs.Length - 1)
                {
                    currentBlock = 0;
                    currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentConstructs[currentBlock].transform.position = junkYard.transform.position;
                if (currentBlock > 0)
                {
                    currentBlock -= 1;
                    currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
                }
                else if (currentBlock <= 0)
                {
                    currentBlock = 3;
                    currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (currentArray == 1)
            {
                ArraySwitch();
            }
            else if (currentArray >= 2)
            {
                constructionMode = false;
            }
        }
    }


    public void ArraySwitch()
    {
        currentArray = 2;

        if (currentBlock == 0)
        {
            float height = currentConstructs[currentBlock].transform.localScale.y;
            float otherHeight = spawnPoint.transform.position.y;
            spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x, height, spawnPoint.transform.position.z);
            Array.Clear(currentConstructs, 0, currentConstructs.Length);
            currentConstructs = new GameObject[box1Fits.Length];
            box1Fits.CopyTo(currentConstructs, 0);
            currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
        }
        if (currentBlock == 1)
        {
            float height = currentConstructs[currentBlock].transform.localScale.y;
            float otherHeight = spawnPoint.transform.position.y;
            spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x, height, spawnPoint.transform.position.z);
            Array.Clear(currentConstructs, 0, currentConstructs.Length);
            currentConstructs = new GameObject[box2Fits.Length];
            box2Fits.CopyTo(currentConstructs, 0);
            currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
        }
        if (currentBlock == 2)
        {
            float height = currentConstructs[currentBlock].transform.localScale.y;
            float otherHeight = spawnPoint.transform.position.y;
            spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x, height, spawnPoint.transform.position.z);
            Array.Clear(currentConstructs, 0, currentConstructs.Length);
            currentConstructs = new GameObject[box3Fits.Length];
            box3Fits.CopyTo(currentConstructs, 0);
            currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
        }
        if (currentBlock == 3)
        {
            float height = currentConstructs[currentBlock].transform.localScale.y;
            float otherHeight = spawnPoint.transform.position.y;
            spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x, height, spawnPoint.transform.position.z);
            Array.Clear(currentConstructs, 0, currentConstructs.Length);
            currentConstructs = new GameObject[box4Fits.Length];
            box4Fits.CopyTo(currentConstructs, 0);
            currentConstructs[currentBlock].transform.position = spawnPoint.transform.position;
        }
    }
    

}
