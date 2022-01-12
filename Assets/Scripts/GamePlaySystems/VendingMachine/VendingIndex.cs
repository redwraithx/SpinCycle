using UnityEngine;

[System.Serializable]
public class VendingIndex : MonoBehaviour
{
    public string Name;
    public string Price;
    public string Description;
    public Sprite Sprite;

    public VendingIndex(string name, string desc, string price, Sprite sprite)
    {
        Name = name;
        Price = price;
        Description = desc;
        Sprite = sprite;
    }
}