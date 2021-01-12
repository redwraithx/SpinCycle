using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public GameObject Description;
    public GameObject Price;
    public GameObject Image;
    public float RealPrice;
    public Button ThisButton;

    private void Start()
    {
        if (ThisButton)
        {
            ThisButton.onClick.AddListener(Buy);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Description.SetActive(true);
        Price.SetActive(true);
        Image.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Description.SetActive(false);
        Price.SetActive(false);
        Image.SetActive(false);
    }
    
    public void Buy()
    {
        GameManager.Instance.points = GameManager.Instance.points -= RealPrice;
    }
}
