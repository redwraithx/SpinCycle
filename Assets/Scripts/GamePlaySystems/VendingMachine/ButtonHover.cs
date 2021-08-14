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
    [Header("Parts of the Buttons")]
    public GameObject Description;
    public GameObject Price;
    public GameObject Name;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI NameText;
    public Button ThisButton;

    [Header("Parts of the Machine")]
    public GameObject SaleItemGameObject;
    public GameObject itemSpawnPoint;
    public VendingIndex VendingIndex;
    public GameObject VendingUI;
    public GameObject BuyItemZone;

    [Header("Autofill Items")]
    public bool FirstRun = false;
    public Item saleItem;
    PlayerPoints playerPoints = null;
    public string networkItemToSpawn = "";


    [Header("External View Items")]

    public Image displayButton;
    //public GameObject Description2;
    //public GameObject Price2;
    //public GameObject Name2;
    //public TextMeshProUGUI DescriptionText2;
    //public TextMeshProUGUI PriceText2;
    //public TextMeshProUGUI NameText2;

    [Header("Math")]
    public int RealPrice = 0;

    private void Start()
    {
        playerPoints = null;//GameManager.Instance.Player1.GetComponent<PlayerPoints>();
        saleItem = SaleItemGameObject.GetComponent<Item>();
        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);
        //Description2.SetActive(false);
        //Price2.SetActive(false);
        //Name2.SetActive(false);

        if (ThisButton)
        {
            ThisButton.onClick.AddListener(Buy);
        }

        if (SaleItemGameObject)
        {
            VendingIndex = new VendingIndex(saleItem.name, saleItem.Description, saleItem.Price.ToString(), saleItem.sprite);
            //If an error with the vending machine is pointing out this line make sure all objects listed in the vending machine have the right item setup
        }

        ThisButton.GetComponent<Image>().sprite = VendingIndex.Sprite;

        if (displayButton != null)
        {
            displayButton.GetComponent<Image>().sprite = VendingIndex.Sprite;
        }
    }

    internal void OpenVendingMachine()
    {
        Debug.Log("OnEnable");
        DescriptionText.text = "";
        PriceText.text = "";
        NameText.text = "";
        //DescriptionText2.text = "";
        //PriceText2.text = "";
        //NameText2.text = "";
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
            //DescriptionText2.text = VendingIndex.Description;
            //PriceText2.text = VendingIndex.Price;
            //NameText2.text = VendingIndex.Name;
            networkItemToSpawn = VendingIndex.Name;
            FirstRun = true;
        }
        Description.SetActive(true);
        Price.SetActive(true);
        Name.SetActive(true);
        //Description2.SetActive(true);
        //Price2.SetActive(true);
        //Name2.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exit");

        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);
        //Description2.SetActive(false);
        //Price2.SetActive(false);
        //Name2.SetActive(false);
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
            //Description2.SetActive(false);
            //Price2.SetActive(false);
            //Name2.SetActive(false);
            BuyItemZone.BroadcastMessage("CloseUI");

        }
    }

    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} has been disabled");
        
        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);
        //Description2.SetActive(false);
        //Price2.SetActive(false);
        //Name2.SetActive(false);
    }
}
