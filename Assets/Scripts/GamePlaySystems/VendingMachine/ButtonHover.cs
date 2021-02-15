using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


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

    public GameObject VendingUI;
    

    private void Start()
    {
        Debug.Log("sldfkjsdlkfjlsdkjf");
        
        playerPoints = GameObject.Find("PlayerCC").GetComponent<PlayerPoints>();
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
        }
    }

    internal void OpenVendingMachine()
    {
        Debug.Log("OnEnable");
        DescriptionText.text = "";
        PriceText.text = "";
        NameText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        
        if (!FirstRun)
        {
            DescriptionText.text = VendingIndex.Description;
            PriceText.text = VendingIndex.Price;
            NameText.text = VendingIndex.Name;
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
        RealPrice = RealPrice += Convert.ToInt32(VendingIndex.Price); //int.Parse(VendingIndex.Price);
        
        
        if (RealPrice <= playerPoints.Points)
        {
            Debug.Log("buyingItemButtonHoverCS");
            //playerPoints.points -= RealPrice;
            playerPoints.Points = -RealPrice;
            GameObject sale = Instantiate(SaleItemGameObject, itemSpawnPoint.transform.position, Quaternion.identity);

            VendingUI.SetActive(false);

            // this is filler code so it actually sells stuff while I experiment with indexes in a seperate project
            //index thing gets spawned
        }
    }
    
    
}
