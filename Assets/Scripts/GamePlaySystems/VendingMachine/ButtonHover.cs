using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class ButtonHover : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public GameObject Description;
    public GameObject Price;
    public GameObject Name;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI NameText;
    public int RealPrice = 0;
    public Button ThisButton;
    public GameObject SaleItemGameObject;
    public GameObject itemSpawnPoint;
    public VendingIndex VendingIndex;
    public bool FirstRun = false;
    public Item saleItem;
    PlayerPoints playerPoints = null;
    public string networkItemToSpawn = "";

    public GameObject VendingUI;

    public GameObject BuyItemZone;




    private void Start()
    {
        playerPoints = GameManager.Instance.Player1.GetComponent<PlayerPoints>();
        saleItem = SaleItemGameObject.GetComponent<Item>();
        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);

        if (ThisButton)
        {
            ThisButton.onClick.AddListener(Buy);
        }

        if (SaleItemGameObject)
        {
            VendingIndex = new VendingIndex(saleItem.name, saleItem.Description, saleItem.Price.ToString());
            //If an error with the vending machine is pointing out this line make sure all objects listed in the vending machine have the right item setup
        }
    }

    internal void OpenVendingMachine()
    {
        Debug.Log("OnEnable");
        DescriptionText.text = "";
        PriceText.text = "";
        NameText.text = "";
    }
    //if the following functions dont work make sure there is no ui items blocking the vending UI ESPECIALLY THE BLACK SCREEN OBJECT, disable raycast :)
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        
        if (!FirstRun)
        {
            DescriptionText.text = VendingIndex.Description;
            PriceText.text = VendingIndex.Price;
            NameText.text = VendingIndex.Name;
            networkItemToSpawn = VendingIndex.Name;
            FirstRun = true;
        }
        Description.SetActive(true);
        Price.SetActive(true);
        Name.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exit");

        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);
    }
    
    public void Buy()
    {
        RealPrice = RealPrice += Convert.ToInt32(VendingIndex.Price); 
        
        
        if (RealPrice <= playerPoints.Points)
        {
            Debug.Log("buyingItemButtonHoverCS");
            playerPoints.Points -= RealPrice;
            GameObject sale = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), itemSpawnPoint.transform.position, Quaternion.identity);
            Description.SetActive(false);
            Price.SetActive(false);
            Name.SetActive(false);
            BuyItemZone.BroadcastMessage("CloseUI");

        }
    }

    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} has been disabled");
        
        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);
    }
}
