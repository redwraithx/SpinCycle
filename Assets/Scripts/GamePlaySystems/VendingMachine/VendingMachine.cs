﻿
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class VendingMachine : MonoBehaviour /*IVendingMachine*/
{
    //add index for button hover
    public GameObject VendingUI;
<<<<<<< HEAD
=======
    public ButtonHover[] buttonHoverScripts;
>>>>>>> main
    public Button closeButton;

    [SerializeField] internal GameObject currentUser = null;
    [SerializeField] private CinemachineBrain currentObjectMouseLook = null;


    void Start()
    {
        if (closeButton)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("press E to spawn item trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            currentUser = other.gameObject;
        }

    }
    
    private void OnTriggerStay(Collider other)
    {
<<<<<<< HEAD
        Debug.Log("press E to spawn item trigger");
=======
        Debug.Log("press E to spawn item trigger (stay)");
>>>>>>> main

        if (other.gameObject.CompareTag("Player"))
        {
            currentUser = other.gameObject;
            currentObjectMouseLook = currentUser.GetComponentInChildren<CinemachineBrain>();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        currentUser = null;

        if (VendingUI.activeInHierarchy)
        {
            CloseUI();
        }
<<<<<<< HEAD
=======
        if (!VendingUI.activeInHierarchy)
        {
            CloseUI();
        }
>>>>>>> main
    }

    private void Update()
    {
        
        if(currentUser && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("User using vending machine trigger");
            VendingUI.SetActive(true);
<<<<<<< HEAD
=======

            foreach (var buttonHoverScript in buttonHoverScripts)
            {
                
                Debug.Log("button haslddf");
                buttonHoverScript.Description.SetActive(false);
                buttonHoverScript.Price.SetActive(false);
                buttonHoverScript.Name.SetActive(false);
            }
            
>>>>>>> main
            
            // enable mouse
            Cursor.lockState = CursorLockMode.None;
            
            // disable mouse camera view
            currentObjectMouseLook.enabled = false;
        }
    }


private void CloseUI()
    {
        // enable mouse camera movement
        currentObjectMouseLook.enabled = true;
        
        VendingUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        
        // clear current object
        currentUser = null;
        
    }
    
}

