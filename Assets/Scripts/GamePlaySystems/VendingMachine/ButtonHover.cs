using System.Collections;
using System.Collections.Generic;
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
    public float RealPrice;
    public Button ThisButton;
    public GameObject SaleItem;
    public GameObject itemSpawnPoint;
    public VendingIndex VendingIndex;
    public bool FirstRun = false;
    public Item saleItem;

    private void Start()
    {
        saleItem = SaleItem.GetComponent<Item>();
        Description.SetActive(false);
        Price.SetActive(false);
        Name.SetActive(false);

        if (ThisButton)
        {
            ThisButton.onClick.AddListener(Buy);
        }

        if (SaleItem)
        {
            VendingIndex = new VendingIndex(saleItem.name, saleItem.Description, saleItem.Price.ToString());
        }
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
        Debug.Log("buyingItemButtonHoverCS");
        GameManager.Instance.points = GameManager.Instance.points -= saleItem.Price;
        GameObject sale = Instantiate(SaleItem, itemSpawnPoint.transform.position, Quaternion.identity);
        // this is filler code so it actually sells stuff while I experiment with indexes in a seperate project
        //index thing gets spawned
    }
    
    
}
