using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class VendingMachine : MonoBehaviour /*IVendingMachine*/
{
    //add index for button hover
    public GameObject VendingUI;
    public Button closeButton;

    [SerializeField] private GameObject currentObject = null;


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
            currentObject = other.gameObject;
        }

    }
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("press E to spawn item trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            currentObject = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        currentObject = null;

        if (VendingUI.activeInHierarchy)
        {
            CloseUI();
        }
    }

    private void Update()
    {
        
        if(currentObject && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("User using vending machine trigger");
            VendingUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }


private void CloseUI()
    {
        VendingUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
}

