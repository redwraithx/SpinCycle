﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDetonate : MonoBehaviour
{
    public GameObject Bomb;
    public GameObject Radius;
    public bool detonated;
    public float timer;
    public float startingTimer;
    // Start is called before the first frame update
    void Start()
    {
        Radius.SetActive(false);
        startingTimer += 1;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            if(startingTimer <= 0)
            {
                Bomb.SetActive(false);
                Radius.SetActive(true);
                timer = 10;
                detonated = true;
            }
        }

    }

    private void Update()
    {

        if (detonated == true)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }

        if(startingTimer >= 0)
        {
            startingTimer -= Time.deltaTime;
        }
        
    }
}
