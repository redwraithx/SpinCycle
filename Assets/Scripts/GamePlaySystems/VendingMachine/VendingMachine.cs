
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class VendingMachine : MonoBehaviour /*IVendingMachine*/
{
    //add index for button hover
    public GameObject VendingUI;
    public GameObject Canvas;
    public ButtonHover[] buttonHoverScripts;
    public Button closeButton;

    [SerializeField] internal GameObject currentUser = null;
    [SerializeField] private CinemachineBrain currentObjectMouseLook = null;

    [SerializeField] private float closeUIDistance = 4f;


    void Start()
    {
        if (closeButton)
        {
            closeButton.onClick.AddListener(CloseUI);
        }

        Canvas.GetComponent<CanvasGroup>().alpha = 1;
        Canvas.GetComponent<CanvasGroup>().interactable = true;
        Canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        VendingUI.SetActive(false);
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
        Debug.Log("press E to spawn item trigger (stay)");

        if (other.gameObject.CompareTag("Player"))
        {
            currentUser = other.gameObject;
            currentObjectMouseLook = currentUser.GetComponentInChildren<CinemachineBrain>();
        }

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    currentUser = null;

    //    if (VendingUI.activeInHierarchy)
    //    {
    //        CloseUI();
    //    }
    //    if (!VendingUI.activeInHierarchy)
    //    {
    //        CloseUI();
    //    }
    //}

    private void Update()
    {
        
        if(currentUser && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("User using vending machine trigger");
            VendingUI.SetActive(true);

            foreach (var buttonHoverScript in buttonHoverScripts)
            {
                
                Debug.Log($"button {buttonHoverScript.Name}");
                
                buttonHoverScript.Description.SetActive(false);
                buttonHoverScript.Price.SetActive(false);
                buttonHoverScript.Name.SetActive(false);
            }
            
            
            // enable mouse
            Cursor.lockState = CursorLockMode.None;
            
            // disable mouse camera view
            currentObjectMouseLook.enabled = false;
        }

        // is the user at the vending machine? if not and UI is open close it.
        if (!currentUser || Vector3.Distance(currentUser.transform.position, transform.position) > closeUIDistance)
        {
            if (VendingUI.activeInHierarchy)
                CloseUI();
        }
    }


private void CloseUI()
    {
        // enable mouse camera movement
        currentObjectMouseLook.enabled = true;
        
        
        foreach (var buttonHoverScript in buttonHoverScripts)
        {
                
            Debug.Log($"button {buttonHoverScript.Name}");
                
            buttonHoverScript.Description.SetActive(false);
            buttonHoverScript.Price.SetActive(false);
            buttonHoverScript.Name.SetActive(false);
        }
        
        VendingUI.SetActive(false);
        
        
        
        Cursor.lockState = CursorLockMode.Locked;
        
        // clear current object
        currentUser = null;
        
    }
    
}

