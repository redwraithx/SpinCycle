using System;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour /*IVendingMachine*/
{
    //add index for button hover
    public GameObject VendingUI;
    public Button closeButton;
    void Start()
    {
        if (closeButton)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("press E to spawn item trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("User using vending machine trigger");
                VendingUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
    }


    private void CloseUI()
    {
        VendingUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
}

